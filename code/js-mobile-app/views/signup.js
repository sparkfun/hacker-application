/*global forge:true, module:true, require:true*/

var app = require('../app');
var View = require('./view');
var template = require('./templates/signup');
var utils = require('../lib/utilities');

module.exports = View.extend({
    tagName: 'div',
    className: 'signup-view',
	initialize: function () {
        forge.logging.log("views.signup.init()");
        this.template = template;
        return this;
    },
    post_render: function () {
        $('#submit-signup', this.el).hammer();
        if (forge.is.web()) {  // initialize the webbased datepicker
            $('#signup-birthdate', this.el).datepicker({
                format: 'mm/dd/yyyy',
                startView: 1,  // start with years selection
                autoclose: true
            });
        }
        // set the max height of the modal dialog based on screen height
        $('#signup-modal', this.el).on('show', function () {
            // TODO: tweak this to handle keyboard issues
            var h = $(window).height() - 170;  // account for header & footer
            $(this).find('.modal-body').css({
                'max-height': h
            });
        });
        $('body').append(this.el);
    },
    hammerEvents: {
        'tap #submit-signup': 'submit_signup',
        'keyup :input': 'key_pressed'
    },
    key_pressed: function (e) {
        if (e.keyCode === 13) {  // validate <enter> key
            if ($('#signup-modal').is(":visible")) {
                this.submit_signup();
            }
        }
    },
    get_data: function () {
        var context;
        if (forge.is.mobile()) {
            context = { mobile: true };
        } else {
            context = { mobile: false };
        }
        return context;
    },
    submit_signup: function () {
        $('#form-signup').validate({
            rules: {
                firstname: {required: true},
                lastname: {required: true},
                email: {email: true, required: true},
                password: {required: true, minlength: 8},
                passwordverify: {required: true, minlength: 8, equalTo: "#signup-password"},
                birthdate: {date: true, required: true},
                gender: {required: true}
            },
            messages: {
                firstname: "Please enter your first name",
                lastname: "Please enter your last name",
                email: "Please enter a valid email",
                password: "Password must be at least 8 characters",
                passwordverify: "Passwords do not match",
                birthdate: "Please enter your birthdate",
                gender: "Please select your gender"
            }
        });
        if ($('#form-signup').valid()) {
            var view = this;
            var birth_date;  // get the birthdate from the correct input event
            if (forge.is.mobile()) {
                birth_date = new Date($('#signup-birthdate').val()).toISOString();
            } else {
                birth_date = $('#signup-birthdate').datepicker('getDate').toISOString();
            }
            forge.request.ajax({
                type: 'POST',
                url: encodeURI(app.base_url + 'signup'),
                data: { first_name: $('#signup-first-name').val(),
                    last_name: $('#signup-last-name').val(),
                    email: $('#signup-email').val(),
                    password: $('#signup-password').val(),
                    birth_date: birth_date,
                    gender: $('input[name=gender]:checked').val(),
                    remember_me: $('input[name=login-remember-me]:checked').length
                },
                dataType: 'json',
                headers: {},
                success: function (data) {
                    forge.logging.log("views.signup.submit_signup:post_signup[success]");
                    $('#signup-modal').modal('hide');
                    utils.show_loading();
                    app.kickstart(data);
                },
                error: function (error) {
                    utils.log_error_response('app.signup[error]: ', error);
                    utils.alert("Signup Failed", JSON.stringify(error));
                }
            });
        }
    },
    clean: function () {
        $('#signup-first-name').val("");
        $('#signup-first-name').tooltip('hide');
        $('#signup-last-name').val("");
        $('#signup-last-name').tooltip('hide');
        $('#signup-email').val("");
        $('#signup-email').tooltip('hide');
        $('#signup-password').val("");
        $('#signup-password').tooltip('hide');
        $('#signup-password-verify').val("");
        $('#signup-password-verify').tooltip('hide');
        $('input[name=gender]:checked').prop('checked', false);
        $('input[name=gender]').tooltip('hide');
        $('#signup-birthdate-value').val("");
        $('#signup-birthdate-value').tooltip('hide');
    },
    show: function () {
        forge.logging.log("views.signup.show()");
        var view = this;
        if (forge.is.mobile()) {
            forge.topbar.hide();
            forge.tabbar.hide();
        } else {
            $('.topbar').hide();
            $('.tabbar').hide();
        }
        view.clean();  // make sure the view is clean
        $('#signup-modal').modal('show');
    }
});
