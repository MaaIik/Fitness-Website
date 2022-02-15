#!/usr/bin/python3

import os

import faker
import pytest


class Form:
    def __init__(self, app):
        form = app.find(['#Name', '#Title', '#Message', '#Submit'])

        self.name = app.one(form, 'id', 'Name')
        self.title = app.one(form, 'id', 'Title')
        self.message = app.one(form, 'id', 'Message')
        self.button = app.one(form, 'id', 'Submit')

        assert all([self.name, self.title, self.message, self.button]), 'Failed to find one or more UI elements'
        self.elements = [self.name, self.title, self.message]

    def post(self, name, title, message):
        self.name.send_keys(name)
        self.title.send_keys(title)
        self.message.send_keys(message)
        self.button.click()


@pytest.fixture()
def form(app):
    app.get('Guestbook/Add')
    return Form(app)


class TestAssignment3:
    def test_check_for_models(self):
        path = 'solutions/assignment_3/Models'
        assert os.path.isdir(path), 'Failed to find Models directory'
        assert len(os.listdir()) > 1, 'Failed to find any models other than the default'

    def test_add_link(self, app):
        app.get('Guestbook')
        links = [link.get_attribute('href') for link in app.find('a')]

        assert links, 'Failed to find any links on the Guestbook/Index page, looking for add link.'
        assert any(link.endswith('/Guestbook/Add') for link in links),\
            'Failed to find link to Guestbook/Add page in Guestbook/Index'

    def test_post_method(self, app):
        app.get('Guestbook/Add')
        forms = app.find('form')

        assert forms, 'Failed to find any forms on the page'
        assert any(f.get_attribute('method') == 'post' for f in forms), 'Failed to find any forms using post method'

    def test_add_post(self, app, fake):
        for i in range(3):
            test_post = [fake.name(), fake.text(50)[:-1].replace('\n', ' '), fake.text(200).replace('\n', ' ')]

            app.get('Guestbook/Add')

            form = Form(app)
            form.post(test_post[0], test_post[1], test_post[2])

            app.get('Guestbook')

            for data in test_post:
                assert data in app.driver.page_source, f'Failed to find "{ data }" in the guestbook after post'

    @pytest.mark.parametrize("test_post", [
        ['', 'First post title', 'First post message'],
        ['Lisa larsen', '', 'First post message'],
        ['Lisa Larsen', 'First post title', ''],
    ])
    def test_missing_fields(self, app, form, test_post):
        url = app.driver.current_url
        form.post(test_post[0], test_post[1], test_post[2])
        assert app.driver.current_url == url, 'URL changed even though the form was incomplete.'

        form = Form(app)

        for i in range(3):
            assert form.elements[i].get_attribute('value') == test_post[i],\
                f'{ form.elements[i].get_attribute("id") }' \
                f'did not keep its original value in the form even if the post was incomplete.'

        app.get('Guestbook')

        assert not any([data in app.driver.page_source for data in test_post if data]),\
            'Found data from incomplete form in the guestbook.'

    def test_field_sizes(self, app, form):
        sizes = [(0, 0), (5, 50), (20, 200)]

        for i in [1, 2]:  # Name doesn't have a size requirement
            assert form.elements[i].get_attribute('data-val-minlength-min') == str(sizes[i][0]) or\
                form.elements[i].get_attribute('data-val-length-min') == str(sizes[i][0]),\
                f'Incorrect minimum value for { form.elements[i].get_attribute("id") }'

            assert form.elements[i].get_attribute('data-val-maxlength-max') == str(sizes[i][1]) or\
                form.elements[i].get_attribute('data-val-length-max') == str(sizes[i][1]),\
                f'Incorrect maximum value for { form.elements[i].get_attribute("id") }'


if __name__ == "__main__":
    pytest.main()
