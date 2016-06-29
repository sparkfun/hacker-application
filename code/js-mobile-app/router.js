/*global require:true, module:true, forge:true, Backbone:true*/

var app = require('app');
var utils = require('lib/utilities');

module.exports = Backbone.Router.extend({
    routes: {
        '': 'index',
        'login': 'login',
        'signup': 'signup',
        'logout': 'logout',
        'peeps': 'peeps',
        '*path': 'index' // default view (route to the active tab)
    },
    index: function () {
        // validate user login
        forge.request.ajax({
            type: 'GET',
            url: encodeURI(app.base_url + 'login'),
            dataType: 'json',
            success: function (data, headers) {
                utils.show_loading();
                app.kickstart(data);
            },
            error: function (error) {
                // user is not authenticated, route them back to login
                app.router.navigate('#login', { trigger: true });
            }
        });
    },
    login: function () {
        app.show_view('login-view');
    },
    signup: function () {
        app.show_view('signup-view');
    },
    logout: function () {
        forge.request.ajax({
            type: 'GET',
            url: encodeURI(app.base_url + 'logout'),
            dataType: 'json',
            success: function (data) {
                // TODO: reset all views and collections here
                app.hide_view($('.content').children(':visible'));
                app.router.navigate('#login', { trigger: true });
            },
            error: function (error) {
                forge.logging.log('router.logout:get_logout[error] ' + JSON.stringify(error));
            }
        });
    },
    peeps: function () {
        app.show_view('tabbar-view');
        app.hide_view($('#topbar-buttons').children(':visible'));
        app.show_view('topbar-peeps-view');
        if (forge.is.mobile()) {
            app.activate_mobile_btn('tabbar-peeps', function (btn) {
                btn.setActive();
            });
        } else {
            $('#tabbar-peeps').button("toggle");
        }
        app.hide_view($('.content').children(':visible'));
        app.show_view('content-peeps-view');
        forge.prefs.set('active_tab', 'peeps');
        utils.hide_loading();
    }
});
