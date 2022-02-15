#!/usr/bin/python3

import os
import time

import pytest

from time import localtime, strftime

test_post_edit = ['Linux for dummies', 'Learning Linux step by step in this blog post',
                  'Some useful information about Linux']
test_post_add = ['C# for dummies', 'Learning C# step by step in this blog post', 'Some useful information about C#']
test_post_add_2 = ['C++ for dummies', 'Learning C++ step by step in this blog post',
                   'Some useful information about C++']

test_user = ['darth@vader.com', 'Anakin42', 'UMK6VtLD1!']
test_user_2 = ['luke@skywalker.eu', 'Anakin69', 'MWK7tLM1?']


class BlogUI:
    def __init__(self, app):
        app.get('Blog')
        ui = app.find(['.Nickname', '.Title', '.Summary', '.Content', '.Time'])

        self.nickname = app.one(ui, 'class', 'Nickname')
        self.title = app.one(ui, 'class', 'Title')
        self.summary = app.one(ui, 'class', 'Summary')
        self.content = app.one(ui, 'class', 'Content')
        self.time = app.one(ui, 'class', 'Time')

        # Array of tuples with name and element, useful for loops later
        self.blog_post = [
            ('Nickname', self.nickname),
            ('Title', self.title),
            ('Summary', self.summary),
            ('Content', self.content),
            ('Time', self.time)
        ]

        assert all([self.nickname, self.title, self.summary, self.content, self.time]),\
            'Failed to find one or more UI elements'

        time = strftime("%d.%m.%Y", localtime())
        assert time in self.time.text, "Failed to find timestamp of posts, expected to find Time class with: "\
                                       + time + " Found: " + self.time.text


class AddUI:
    def __init__(self, app):
        app.get('Blog/Add')
        ui = app.find(['#Title', '#Summary', '#Content', '#Submit'])

        self.title = app.one(ui, 'id', 'Title')
        self.summary = app.one(ui, 'id', 'Summary')
        self.content = app.one(ui, 'id', 'Content')
        self.submit = app.one(ui, 'id', 'Submit')

        assert all([self.title, self.summary, self.content, self.submit]), 'Failed to find one or more UI elements'


class AuthUI:
    def __init__(self, app):
        app.get('Identity/Account/Register')
        ui = app.find(['#Input_Email', '#Input_Nickname', '#Input_Password', '#Input_ConfirmPassword', '.btn'])

        self.email = app.one(ui, 'id', 'Input_Email')
        self.nickname = app.one(ui, 'id', 'Input_Nickname')
        self.password = app.one(ui, 'id', 'Input_Password')
        self.confirm_password = app.one(ui, 'id', 'Input_ConfirmPassword')
        self.submit = app.one(ui, 'type', 'submit')

        assert all([self.email, self.nickname, self.password, self.confirm_password, self.submit]),\
            'Failed to find one or more UI elements'


class EditUI:
    def __init__(self, app, post_id):
        app.get('Blog/Edit/%s' % post_id)
        ui = app.find(['#Title', '#Summary', '#Content', '#Submit'])

        self.title = app.one(ui, 'id', 'Title')
        self.summary = app.one(ui, 'id', 'Summary')
        self.content = app.one(ui, 'id', 'Content')
        self.submit = app.one(ui, 'id', 'Submit')

        # Array of tuples with name and element, useful for loops later
        self.form = [
            ('Title', self.title),
            ('Summary', self.summary),
            ('Content', self.content)
        ]

        assert all([self.title, self.summary, self.content, self.submit]), 'Failed to find one or more UI elements'


class LoginUI:
    def __init__(self, app):
        app.get('Identity/Account/Login')
        ui = app.find(['#Input_Email', '#Input_Password', '.btn'])

        self.email = app.one(ui, 'id', 'Input_Email')
        self.password = app.one(ui, 'id', 'Input_Password')
        self.submit = app.one(ui, 'type', 'submit')

        assert all([self.email, self.password, self.submit]), 'Failed to find one or more UI elements'


class LogoutUI:
    def __init__(self, app):
        app.get('Identity/Account/Manage')
        self.logout = app.driver.find_elements_by_xpath("//*[text()[contains(., 'Logout')]]")[0]
        assert self.logout, 'failed to find log out button'


def signup(app, email, nickname, password):
    ui = AuthUI(app)

    ui.email.send_keys(email)
    ui.nickname.send_keys(nickname)
    ui.password.send_keys(password)
    ui.confirm_password.send_keys(password)

    with app.wait():
        ui.submit.click()
        pass

    confirm = app.find('#confirm-link')
    assert len(confirm) == 1, 'Failed to find register confirm link, something went wrong during signup.'

    with app.wait():
        confirm[0].click()
        pass

    login(app, email, password)


def login(app, email, password):
    ui = LoginUI(app)

    ui.email.send_keys(email)
    ui.password.send_keys(password)

    with app.wait():
        ui.submit.click()


def add_post(app, title, summary, content):
    ui = AddUI(app)

    ui.title.send_keys(title)
    ui.summary.send_keys(summary)
    ui.content.send_keys(content)

    with app.wait():
        ui.submit.click()


def edit_post(app, id, title, summary, content):
    ui = EditUI(app, id)

    ui.title.clear()
    ui.summary.clear()
    ui.content.clear()

    ui.title.send_keys(title)
    ui.summary.send_keys(summary)
    ui.content.send_keys(content)

    with app.wait():
        ui.submit.click()


class TestAssignment4:
    # Checks if unauthenticated users have access to the add and edit pages
    def test_no_auth_access(self, app):
        driver = app.get('Blog/Add')

        assert 'Blog/Add' not in driver.current_url, "Unauthenticated users has access to add-page. " \
                                                     "Users should be redirected."

        driver = app.get('Blog/Edit')

        assert 'Blog/Edit' not in driver.current_url, "Unauthenticated users has access to edit-page." \
                                                      " Users should be redirected."

    # Checks is a user can sign up and if it is authenticated
    def test_check_signup(self, app):
        signup(app, test_user[0], test_user[1], test_user[2])

        all_cookies = app.driver.get_cookies()
        identity_cookie = ".AspNetCore.Identity.Application"

        assert any(cookie['name'] == identity_cookie for cookie in all_cookies), 'Failed to login to page'

    # Checks if a user can add blog posts
    def test_create_post(self, app):
        signup(app, test_user[0], test_user[1], test_user[2])

        add_post(app, test_post_add[0], test_post_add[1], test_post_add[2])

        ui = BlogUI(app)

        for field, data in zip(ui.blog_post, test_post_add):
            field_name, field_element = field

            assert data in app.driver.page_source, 'Failed to find %s in the blog start page after adding a post'\
                                                   % field_name

    # Checks if a user can edit a blog post
    def test_edit_post(self, app):
        signup(app, test_user[0], test_user[1], test_user[2])
        add_post(app, test_post_add[0], test_post_add[1], test_post_add[2])

        ui = EditUI(app, 1)

        assert ui.title.get_attribute("value") == test_post_add[0],\
            "The title of the post does not correspond to the original post"
        assert ui.summary.get_attribute("value") == test_post_add[1],\
            "The summary of the post does not correspond to the original post"
        assert ui.content.get_attribute("value") == test_post_add[2],\
            "The content of the post does not correspond to the original post"

        edit_post(app, 1, test_post_edit[0], test_post_edit[1], test_post_edit[2])

        ui = EditUI(app, 1)

        assert ui.title.get_attribute("value") == test_post_edit[0],\
            "The title of the post does not correspond to the new value added by the edit"
        assert ui.summary.get_attribute("value") == test_post_edit[1],\
            "The summary of the post does not correspond to the new value added by the edit"
        assert ui.content.get_attribute("value") == test_post_edit[2],\
            "The content of the post does not correspond to the new value added by the edit"

    # Checks that posts are ordered in newest first order
    def test_post_ordering_newest_first(self, app):
        signup(app, test_user[0], test_user[1], test_user[2])
        add_post(app, test_post_add[0], test_post_add[1], test_post_add[2])
        time.sleep(1)
        add_post(app, test_post_add_2[0], test_post_add_2[1], test_post_add_2[2])

        titles = app.find('.Title')

        assert titles[0].text == test_post_add_2[0], "Failed to find posts in correct order. "\
                                                     "First title was %s " % titles[1].text

        assert titles[1].text == test_post_add[0], "Failed to find posts in correct order. Second title was %s "\
                                                   % titles[1].text

    # Checks if a anyone can see all blog posts
    def test_see_other_users_posts(self, app):
        signup(app, test_user[0], test_user[1], test_user[2])
        add_post(app, test_post_add[0], test_post_add[1], test_post_add[2])

        ui = LogoutUI(app)
        ui.logout.click()

        # Test that the posts can be seen when logged out
        app.get('Blog')
        ui = app.find(['.Nickname', '.Title', '.Summary', '.Content'])

        nicknames = app.all(ui, 'class', 'Nickname')
        titles = app.all(ui, 'class', 'Title')
        summaries = app.all(ui, 'class', 'Summary')
        contents = app.all(ui, 'class', 'Content')

        assert nicknames[0].text == test_user[1], "Failed to find posts of other users when logged out. "
        assert titles[0].text == test_post_add[0], "Failed to find posts of other users when logged out. "
        assert summaries[0].text == test_post_add[1], "Failed to find posts of other users when logged out. "
        assert contents[0].text == test_post_add[2], "Failed to find posts of other users when logged out. "

        signup(app, test_user_2[0], test_user_2[1], test_user_2[2])
        add_post(app, test_post_add_2[0], test_post_add_2[1], test_post_add_2[2])

        # Test that the posts can be seen when logged in as another user
        app.get('Blog')
        ui = app.find(['.Nickname', '.Title', '.Summary', '.Content'])

        nicknames = app.all(ui, 'class', 'Nickname')
        titles = app.all(ui, 'class', 'Title')
        summaries = app.all(ui, 'class', 'Summary')
        contents = app.all(ui, 'class', 'Content')

        assert nicknames[1].text == test_user[1],\
            "Failed to find posts of other users when logged in as another user. "
        assert titles[1].text == test_post_add[0],\
            "Failed to find posts of other users when logged in as another user. "
        assert summaries[1].text == test_post_add[1],\
            "Failed to find posts of other users when logged in as another user. "
        assert contents[1].text == test_post_add[2],\
            "Failed to find posts of other users when logged in as another user. "

    # Checks if a user can edit another users blog post
    def test_edit_other_users_posts(self, app):
        signup(app, test_user[0], test_user[1], test_user[2])
        add_post(app, test_post_add[0], test_post_add[1], test_post_add[2])

        ui = LogoutUI(app)
        ui.logout.click()

        signup(app, test_user_2[0], test_user_2[1], test_user_2[2])

        with app.wait():
            driver = app.get('Blog/Edit/1')

        assert 'Blog/Edit' not in driver.current_url, "Users has access to edit-page of another users post." \
                                                      " Users should be redirected."

    def test_check_for_partial_view(self, app):
        assignment = f'assignment_{ self.__class__.__name__[-1] }'
        main_path = os.path.join('solutions', assignment, 'Views/Blog/Index.cshtml')
        assert os.path.isfile(main_path), 'Failed to find view: %s' % main_path

        partial_path = os.path.join('solutions', assignment, 'Views/Shared/_PostPartial.cshtml')
        assert os.path.isfile(partial_path), 'Failed to find partial view: %s' % partial_path

        assert '_PostPartial' in app.read_file(main_path), 'Failed to find partial view usage in Guestbook Index view'


if __name__ == "__main__":
    pytest.main()
