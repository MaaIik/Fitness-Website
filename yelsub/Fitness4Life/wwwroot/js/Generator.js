$(document).ready(function(){
    
    let Answer1 = 0
    let Answer2 = 0
    let Answer3 = 0
    
    
    /*    Makes sure only one answer can be checked for each question  */
    $('.answer1').click(function() {
        $('.answer1').not(this).prop('checked', false);
        console.log(this.id);
        Answer1 = this.value
    });

    $('.answer2').click(function() {
        $('.answer2').not(this).prop('checked', false);
        console.log(this.id);
        Answer2 = this.value
    });

    $('.answer3').click(function() {
        $('.answer3').not(this).prop('checked', false);
        console.log(this.id);
        Answer3 = this.value
        $('#submit').show();
    });

    
    
    
  /*  if ( $('.answer3').is(':checked') ) {
        $('#submit').show();
    }
    else {
        $('#submit').hide();
    }
    */
    $('#submit').click(function() {
        console.log(Answer1, Answer2, Answer3)
        $("#content").load("Home/Generate", $('#GeneratorForm').serialize());
    });

  
    
    
    
   /* $("#submit").toggle(
        $("#Choice3_2").prop("checked") // For checked attribute it returns true/false;
        // Return value changes with checkbox state
    );*/
    
    
    
/*    if($("#Choice3_1").prop('checked', true))
        //$(".button").show();  // checked
        console.log("Choice 3_1 checked");
    else if($("#Choice3_2").prop('checked', true))
        console.log("Choice 3_2 checked")
        //$(".button").show();// checked
    else
        console.log("None checked");
        //$(".button").hide();  // unchecked
    */
});

    //
    // $('input[type=checkbox]').prop('checked')(function () {
    //     $(this).innerText("checked")
    // });
    //
    //
    //
    
/*    $('input[type="checkbox"]').click(function() {
        if($(this).is(":checked")) {
            alert("Checkbox is checked.");
        }
        else if($(this).is(":not(:checked)")) {
            alert("Checkbox is unchecked.");
        }
    });*/
    
    
    





// $(document).ready(function(){
//
//     $("#Choice1_1").on("click", function () {
//         var $checkbox = $(this).find(':checkbox');
//         $checkbox.prop('checked', !$checkbox[0].checked);  
//         function uncheck() {
//         let inputs = document.getElementById('Choice1_2');
//         inputs.checked = false;
//     }
//     });
//    
//    
//     var $checkbox1 = $('#Choice1_1');
//     var $checkbox2 = $('#Choice1_2');
//     var $checkbox3 = $('#Choice1_3');
//     $("#checkbox1").on('click', function() {
//         if ($($checkbox1).is(':checked')) {
//             $($checkbox1).prop('checked', true);
//             $($checkbox2).removeAttr('checked', false); // Unchecks it
//             $($checkbox3).removeAttr('checked', false);
//         } else {
//             $($checkbox1).prop('checked', false);
//         }
//     });
// });

