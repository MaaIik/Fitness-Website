import os
import time
from contextlib import contextmanager

import docker
import faker
import pytest
import requests
from pathlib import Path
from selenium import webdriver
from selenium.webdriver.support.expected_conditions import staleness_of
from selenium.webdriver.support.ui import WebDriverWait


@pytest.fixture()
def fake():
    return faker.Faker()


@pytest.fixture()
def app(request):
    chrome_ip = None

    dotnet = None
    dotnet_ip = None
    dotnet_image = None

    driver = None

    assert 'IMAGE' in os.environ, 'IMAGE not set. Required.'
    dotnet_image = os.environ['IMAGE']

    assert 'NETWORK' in os.environ, 'NETWORK is not set. Required.'
    network = os.environ['NETWORK']

    assert 'HUB' in os.environ, 'HUB is not set. Required.'
    chrome_ip = os.environ['HUB']

    container_timeout = os.environ['CONTAINER_TIMEOUT'] if 'CONTAINER_TIMEOUT' in os.environ else 30
    page_load_timeout = os.environ['PAGE_LOAD_TIMEOUT'] if 'PAGE_LOAD_TIMEOUT' in os.environ else 10
    element_timeout = os.environ['ELEMENT_TIMEOUT'] if 'ELEMENT_TIMEOUT' in os.environ else 5

    # Start / configure containers
    try:
        dotnet = start_container(dotnet_image, network)
        assert dotnet, f'Failed to start { dotnet_image } container.'

        dotnet_ip = dotnet.attrs['NetworkSettings']['Networks'][network]['IPAddress']

        dotnet_url = f'http://{ dotnet_ip }'
        chrome_url = f'http://{ chrome_ip }:4444/wd/hub'

        assert wait_for_server_header(dotnet_url, container_timeout, 'Server', 'Kestrel'), \
            'Failed waiting for .NET URL availability.'
        assert wait_for_server_header(chrome_url, container_timeout, 'Server', 'Jetty'), \
            'Failed waiting for Selenium URL availability.'

        driver = webdriver.Remote(chrome_url, webdriver.DesiredCapabilities.CHROME)
        assert driver, f'Failed to connect to Selenium. URL: { chrome_url }'

        driver.set_page_load_timeout(page_load_timeout)
        driver.implicitly_wait(element_timeout)
        driver.set_window_size(1920, 1080)

        yield App(driver, dotnet_url, page_load_timeout, element_timeout)
    except Exception as e:
        print(e)
    finally:
        if driver:
            driver.save_screenshot(f'/results/screenshots/{request.node.name}.png')
            driver.quit()

        if dotnet:
            print(dotnet.logs().decode('utf-8'))
            dotnet.remove(force=True)


class App:
    def __init__(self, driver: webdriver.Chrome, dotnet_url, page_load_timeout, element_timeout):
        self.driver = driver
        self._dotnet_url = dotnet_url
        self._page_load_timeout = page_load_timeout
        self._element_timeout = element_timeout

    @contextmanager
    def wait(self, timeout=None):
        timeout = timeout if timeout else self._page_load_timeout
        old_page = self.driver.find_element_by_tag_name('html')
        yield
        try:
            WebDriverWait(self.driver, timeout).until(staleness_of(old_page))
        except Exception as e:
            pytest.fail('Page failed to refresh when it was expected to. Check screenshots for clues.')

        self.driver.implicitly_wait(0)
        error = self.find('.error-code')
        self.driver.implicitly_wait(self._element_timeout)
        assert not error, f'Request failed with a network error: { error[0].text }'

    def get(self, url=''):
        url = f'{ self._dotnet_url }/{ url }'
        success = False

        # Retry 3 times
        for _ in range(3):
            try:
                with self.wait(self._page_load_timeout):
                    self.driver.get(url)

                success = True
                break
            except Exception:
                print('Page failed to load, retrying...')

        assert success, f'Failed to load page. Slow response? URL: { url }'

        return self.driver

    def find(self, what, page=False):
        if type(what) is list:
            if not page:
                what = [f'main { w }' for w in what]
            search = ', '.join(what)
        else:
            search = what if page else f'main { what }'

        try:
            return self.driver.find_elements_by_css_selector(search)
        except Exception:
            return False

    @staticmethod
    def read_file(path):
        return Path(path).read_text()

    @staticmethod
    def one(elements, attribute, value):
        return next(iter(App.all(elements, attribute, value)), None)

    @staticmethod
    def all(elements, attribute, value):
        return [e for e in elements if value in e.get_attribute(attribute).split()]


def start_container(image, network):
    try:
        client = docker.from_env()

        container = client.containers.run(image, network=network,
                                          shm_size='2g', detach=True, init=True, auto_remove=True)
        container.reload()

        return container
    except Exception as e:
        print(f'Failed to start image: { image }: { e }')
        return None


def wait_for_server_header(url, timeout, header_key, header_value):
    start_time = time.time()
    while time.time() - start_time < timeout:
        try:
            res = requests.get(url, timeout=5)

            if header_key in res.headers and header_value in res.headers[header_key]:
                return True
        except Exception:
            print(f'Still waiting for server, retrying { url }...')
            time.sleep(1)

    return False
