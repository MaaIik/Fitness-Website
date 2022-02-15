// Forces the browser to parse your JavaScript strictly. Gives more warnings / errors.
'use strict';

// Sets a function that is run when the document is ready
$(document).ready(function () {
  // Hook up the button click handler using the button's id.
  // This is recommended as it keeps script stuff out of your HTML.
  $('#btnUpdate').click(buttonExternalJQuery);

  // Set the initial value of the text input
  $('#textInput').val('Initial value');

  // Click the button to set the initial text output
  $('#btnUpdate').click();

});

function buttonExternalJQuery() {
  $('#textOutput').text($('#textInput').val());
}
