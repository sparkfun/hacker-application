// JQuery functions and events for the signup page.

// This was an ask from QA for testing purposes.
$(document).ready(function() {
    currentUrl = window.location.href;
    params = get_query_params();
    // If sending is disabled replace always display the parmeter
    if (typeof params['post_to_client'] === 'undefined' && $('#post_to_client').val() === '0') {
        //console.log('doing replace')
        location.replace('?post_to_client=false');
    }
});

// initialize the signup input
$('.sign-up-input').focus(function() {
    // set the input value to empty when clicked.
    if ($(this).prop('value') === $(this).prop("defaultValue")) {
        $(this).prop('value', '');
    }
    $(this).prev('.error').hide();
    $(this).css('border', '1px solid #dddddd');
});

// initialize the signup border
$('.sign-up-input').keypress(function() {
    $(this).prev('.error').hide();
    $(this).css('border', '1px solid #dddddd');
});

// setup the signup form
$('#sign-up-form').submit(function() {
    // On submit gather all the input elements into an object array.
    data = {};
    $('.sign-up-input').each(function() {
        name = $(this).prop('name');
        //console.log('name: ',name, ' property value: ', $(this).prop('value'));
        //console.log('name: ',name,' default value: ', $(this).prop('defaultValue'));
        if ($(this).prop('value') === $(this).prop("defaultValue")) {
            data[name] = '';
        } else {
            data[name] = $(this).prop('value').trim();
        }
    });
    
    // handle the dropdown
    if ($('#interest').prop('value') === '0') {
        data[$('#interest').prop('name')] = '';
    }
    
    // handle the hidden source
    data[$('#source').prop('name')] = $('#source').prop('value');
    
    if (typeof i_immtmid != 'undefined' && typeof i_session_id != 'undefined') {
        // Add the ivrfy variables
        data['i_session_id'] = i_session_id;
        data['tracking_master_id'] = i_immtmid;
    }
    
    // Check that the email fields are matching.
    if (email_check()) {
        sign_up(data);
    } else {
        console.log('email check failed');
    }
    return false;
});

// return the query params from the current url.
function get_query_params() {
    var query_params = [], hash;
    var q = document.URL.split('?')[1];
    if (typeof q !== 'undefined') {
        q = q.split('&');
        for (var i = 0; i < q.length; i++) {
            hash = q[i].split('=');
            //vars.push(hash[1]);
            query_params[hash[0]] = hash[1];
        }
    }
    return query_params;
}

// Do some basic validation of form fields.
function email_check() {
    email = $('#email').prop('value').trim();
    if ($('#email').prop('value') === $('#email').prop("defaultValue") || typeof email === 'undefined' && email === '' && email === ' ') {
        $('#email').css('border', '1px solid red');
        $('#email').prev('.error').show();
        return false;
    }
    email_confirm = $('#email-confirm').prop('value').trim();
    if ($('#email-confirm').prop('value') === $('#email-confirm').prop("defaultValue") || typeof email_confirm === 'undefined' && email_confirm === '' && email_confirm === ' ') {
        $('#email-confirm').css('border', '1px solid red');
        $('#email-confirm').prev('.error').show();
        return false;
    }
    if (email !== email_confirm) {
        $('#email').css('border', '1px solid red');
        $('#email-confirm').css('border', '1px solid red');

        $('#email-confirm').prev('.error').text('The email fields do not match. Please try again.');
        $('#email-confirm').prev('.error').show();
        
        return false;
    }
    return true;
}

// Send signup data back to the application via ajax.
function sign_up(data) {
    $.ajax({
        url: '/signup',
        type: 'PUT',
        cache: false,
        dataType: 'json',
        data: JSON.stringify(data),
        async: false,
        success: function(jsonData) {
            if (jsonData.success === true) {
                window.location = '/map';
            } else {
                //console.log('failure: ',jsonData.errors);
                error_field = jsonData.errors['field'];
                error_message = jsonData.errors['message'];
                $('#'+error_field).css('border', '1px solid red');
                $('#'+error_field).prev('.error').text(error_message);
                $('#'+error_field).prev('.error').show();
            }
        }
    });
}
