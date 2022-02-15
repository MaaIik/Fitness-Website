#!/usr/bin/python3

import pytest


class TestAssignment1:
    def test_basic_tags(self, app):
        app.get()

        required_tags = {'div', 'p', 'h2', 'h3', 'em', 'strong'}
        have_tags = {t.tag_name for t in app.find('*')}

        missing_tags = required_tags - have_tags
        assert not missing_tags, 'Missing one or more required tags: %s' % ', '.join(missing_tags)

    def test_columns(self, app):
        app.get()

        divs = app.find('div')
        assert divs, 'Failed to find any divs'
        columns = [t.get_attribute('class') for t in divs if t.get_attribute('class').startswith('col-')]
        assert len(columns) >= 2, 'Failed to find at least two bootstrap columns'

    def test_ordered_list(self, app):
        app.get()

        assert app.find('ol'), 'Failed to find any ordered lists'
        assert app.find('ol > li'), 'Failed to find any ordered list items'

    def test_unordered_list(self, app):
        app.get()

        assert app.find('ul'), 'Failed to find any unordered lists'
        assert app.find('ul > li'), 'Failed to find any unordered list items'

    def test_table(self, app):
        app.get()

        assert app.find('table'), 'Failed to find any tables'
        assert app.find('table.table'), 'Failed to find a table with the correct class'

    def test_search(self, app):
        app.get()

        assert app.find('form', page=True), 'Failed to find any forms'
        assert app.find('input', page=True), 'Failed to find any input elements'

        assert app.find(['form input[type=text]', 'form input[type=search]'], page=True), \
            'Failed to find any text/search inputs inside forms'
        assert app.find(['form input[type=submit]', 'form button[type=submit]'], page=True), \
            'Failed to find any submit buttons inside forms'

        assert app.find(['form input.form-control[type=text]', 'form input.form-control[type=search]'], page=True), \
            'Failed to find any text/search inputs with the right CSS class'
        assert app.find(['form input.btn[type=submit]', 'form button.btn[type=submit]'], page=True), \
            'Failed to find any submit buttons with the right CSS class'

    def test_img(self, app):
        app.get()

        images = app.find('img')
        assert images, 'Failed to find any images'
        assert any([t.get_attribute('src') for t in images]), 'All images are missing source tags'
        assert any([t.size['width'] > 50 and t.size['height'] > 50 for t in images]), \
            'Image failed to load, is it inside the wwwroot directory?'

    def test_css(self, app):
        app.get()

        stylesheets = app.find('link[rel=stylesheet]', page=True)
        assert stylesheets, 'Failed to find any stylesheet links'

        urls = [s.get_attribute('href') for s in stylesheets]
        assert any(['/css/custom.css' in u for u in urls]), \
            'Failed to find link to /css/custom.css custom style sheet, only found' + str(urls)


if __name__ == "__main__":
    pytest.main()
