#!/usr/bin/python3

import pytest

import random
import operator


class Calculator:
    def __init__(self, app):
        app.get('Calculator')

        number_ids = ['zero', 'one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine']
        operator_ids = ['plus', 'minus', 'multiply', 'divide', 'equals', 'clear', 'reset']
        all_ids = number_ids + operator_ids + ['display']

        buttons = app.find(['#' + id_ for id_ in all_ids])
        assert len(buttons) == len(all_ids), 'One or more of the buttons are missing. Check ids and try again.'

        self.numbers = []
        for n in number_ids:
            self.numbers.append(app.one(buttons, 'id', n))

        self.plus = self.minus = self.multiply = self.divide = self.equals = self.clear = self.reset = None
        for o in operator_ids:
            setattr(self, o, app.one(buttons, 'id', o))

        self.display = app.one(buttons, 'id', 'display')

    def calculate(self, method, op):
        num1 = random.randint(0, 1000)
        num2 = random.randint(0, 1000)

        for d in str(num1):
            self.numbers[int(d)].click()

        for btn in method.split():
            getattr(self, btn).click()

        for d in str(num2):
            self.numbers[int(d)].click()

        self.equals.click()

        return op(num1, num2)

    def value(self):
        return self.display.get_attribute('value') if self.display.get_attribute('value') else self.display.text


@pytest.fixture()
def calc(app):
    return Calculator(app)


class TestAssignment2:
    def test_numeric_keys(self, calc):
        assert calc.value() in ['', '0'], 'Display did not start blank'

        for n in reversed(calc.numbers):
            n.click()

        assert calc.value() == '9876543210', 'Display does not reflect the buttons that were clicked'

    def test_plus(self, calc):
        result = calc.calculate('plus', operator.add)
        assert calc.value() == str(result), 'Failed to add numbers (+)'

    def test_minus(self, calc):
        result = calc.calculate('minus', operator.sub)
        assert calc.value() == str(result), 'Failed to subtract numbers (-)'

    def test_multiply(self, calc):
        result = calc.calculate('multiply', operator.mul)
        assert calc.value() == str(result), 'Failed to multiply numbers (*)'

    def test_divide(self, calc):
        result = calc.calculate('divide', operator.truediv)
        try:
            float(calc.value())
        except ValueError:
            pytest.fail(f'Failed to parse calculator display value as float: { calc.value() }')

        assert round(float(calc.value()), 2) == round(result, 2), 'Failed to divide numbers (/)'

    def test_reset(self, calc):
        assert calc.value() in ['', '0'], 'Display did not start blank'

        calc.numbers[1].click()
        assert calc.value() == '1', 'Display does not show the clicked number'

        calc.reset.click()
        assert calc.value() in ['', '0'], 'Display did not reset on reset'

    def test_clear(self, calc):
        calc.numbers[1].click()
        assert calc.value() == '1', 'Display does not show the clicked number'

        calc.plus.click()
        calc.numbers[2].click()

        calc.clear.click()

        calc.numbers[5].click()
        calc.equals.click()

        assert calc.value() == '6', 'Display did not clear current entry on clear'

    def test_user_errors(self, calc):
        result = calc.calculate('multiply multiply', operator.mul)
        assert calc.value() == str(result), 'Failed to multiply numbers (*) when pressing multiply twice'
        calc.reset.click()

        result = calc.calculate('plus plus', operator.add)
        assert calc.value() == str(result), 'Failed to add numbers (+) when pressing plus twice'
        calc.reset.click()

        result = calc.calculate('minus minus', operator.sub)
        assert calc.value() == str(result), 'Failed to subtract numbers (-) when pressing subtract twice'
        calc.reset.click()

        result = calc.calculate('divide divide', operator.truediv)

        try:
            float(calc.value())
        except ValueError:
            pytest.fail(f'Failed to parse calculator display value as float: {calc.value()}')

        assert round(float(calc.value()), 2) == round(result, 2),\
            'Failed to divide numbers (/) when pressing divide twice'
        calc.reset.click()

    def test_next_calculation(self, calc):
        result = calc.calculate('plus', operator.add)
        calc.plus.click()

        rand = random.randint(1, 9)
        calc.numbers[rand].click()

        calc.equals.click()

        result = operator.add(result, rand)
        assert calc.value() == str(result), 'Failed to use previous result in the next calculation'

    def test_css_file(self, app):
        app.get('Calculator')

        css_files = [link.get_attribute('href') for link in app.find('link', page=True)]
        assert any('/css/calculator.css' in file for file in css_files),\
            'Failed to find CSS link to /css/calculator.css.'

    def test_js_file(self, app):
        app.get('Calculator')

        js_files = [script.get_attribute('src') for script in app.find('script', page=True)]
        assert any('/js/calculator.js' in script for script in js_files),\
            'Failed to find JavaScript reference to ~/jss/calculator.js.'


if __name__ == "__main__":
    pytest.main()
