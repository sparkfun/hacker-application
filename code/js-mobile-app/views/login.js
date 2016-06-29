/*global forge:true, module:true, require:true*/

var app = require('../app');
var View = require('./view');
var mobile_template = require('./templates/login-mobile');
var web_template = require('./templates/login-web');
var utils = require('../lib/utilities');

module.exports = View.extend({
    tagName: 'div',
    className: 'login-view',
	initialize: function () {
        if (forge.is.web()) {
            this.template = web_template;
        } else {
            this.template = mobile_template;
        }
        return this;
	},
    post_render: function () {
        $('#submit-login', this.el).hammer();
        $('#show-signup-modal', this.el).hammer();
        $('.content').append(this.el);
    },
    hammerEvents: {
        'tap #show-signup-modal': 'show_signup_modal',
        'tap #submit-login': 'submit_login',
        'keyup :input': 'key_pressed'
    },
    key_pressed: function (e) {
        if (e.keyCode === 13) {  // validate <enter> key
            if ($('#signup-modal').not(":visible")) {
                this.submit_login();
            }
        }
    },
    show_signup_modal: function () {
        app.show_view('signup-view');
    },
    submit_login: function () {
        $('#form-login').validate({
            rules: {
                email: {email: true, required: true},
                password: {required: true, minlength: 8}
            },
            messages: {
                email: "Please enter a valid email",
                password: "Please enter a password"
            },
            tooltip_options: {
                email: {placement: 'top'},
                password: {placement: 'top'}
            }
        });
        if ($('#form-login').valid()) {
            forge.request.ajax({
                type: 'POST',
                url: app.base_url + 'login',
                data: { email: $('#login-email').val(),
                        password: $('#login-password').val(),
                        remember_me: $('input[name=login-remember-me]:checked').length
                },
                dataType: 'json',
                headers: {},
                success: function (data) {
                    forge.logging.log('views.login.submit_login:post_login[success]');
                    $('#login-password').val(""),
                    utils.show_loading();
                    app.kickstart(data);
                },
                error: function (error) {
                    $('#login-password').val(""),
                    utils.alert('Authentication failed', 'Please verify login information and try again');
                    forge.logging.log('views.login.submit_login:post_login[error] ' + JSON.stringify(error));
                }
            });
        }
    },
    show: function () {
        forge.logging.log('views.login.submit_login.show()');
        if (forge.is.mobile()) {
            forge.topbar.hide();
            forge.tabbar.hide();
        } else {
            $('.topbar').hide();
            $('.tabbar').hide();
        }
        $('.content').show();
        $('.login-view').show();
    }
});
