/*global require:true, module:true, Backbone:true, forge:true*/

var utils = require('lib/utilities');
var Peep = require('models/peep');

var App = {
    cache: {
        // on app bootstrap
        current_user: null,
        // on mobile_sync
        peeps: null, // contacts of user
    },
    initialize: function () {
        // set the api endpoint based on release or debug mode
        if (forge.config.modules.parameters.config.debug)
        {
            this.base_url = forge.config.modules.parameters.config.dbgUrl;
            this.api_url = forge.config.modules.parameters.config.dbgUrl + 'api/';
        } else {
            this.base_url = forge.config.modules.parameters.config.rlsUrl;
            this.api_url = forge.config.modules.parameters.config.rlsUrl + 'api/';
        }

        var Router = require('router');
        this.router = new Router();

        var State = Backbone.Model.extend();
        this.state = new State();
        this.state.set('sync_job', null);
        this.state.set('is_online', false);

        var that = this;
        Backbone.listenToOnce(this.state, 'change', function () {
            that.startup();  // kickstart my heart
        });

        var CurrentUser = require('models/peep');
        this.cache.current_user = new CurrentUser();

        var Peeps = require('models/peeps');
        this.cache.peeps = new Peeps();

        // check for active tab prefs and set to default if not available
        forge.prefs.keys(function (array) {
            if ($.inArray('active_tab', array) === -1) {
                // [options] -> peeps, events, invite-peeps, profile, stream
                forge.prefs.set('active_tab', 'peeps');
            }
            if ($.inArray('active_peep_sort', array) === -1) {
                // [options] -> all, registered, visible
                forge.prefs.set('active_peep_sort', 'all');
            }
        });
        if (forge.is.mobile()) {
            $('.topbar').remove();
            $('.tabbar').remove();
            forge.topbar.hide();
            forge.tabbar.hide();
            // setup the offline/online listener events
            forge.event.connectionStateChange.addListener(function () {
                if (forge.is.connection.connected()) {
                    that.state.set('is_online', true);
                } else {
                    that.state.set('is_online', false);
                }
            });
        } else {
            $('.topbar').hide();
            $('.tabbar').hide();
            // account for content buffering based on platform (web based)
            $('.content').css("top", "44px");
            $('.content').css("bottom", "44px");
            this.state.set('is_online', true);
        }
    },
    startup: function () {
        forge.logging.log("app.startup:backbone_start()[success]");
        Backbone.history.start();  // route to index to validate authentication
    },
    kickstart: function (response) {
        // set the current user attributes and preferences on startup
        for (var key in response) {
            // convert python isostring into js isostring
            if (key === 'birth_date' || key === 'created' || key === 'edited' ) {
                response[key] = utils.python_iso_to_js_iso(response[key], {silent: true});
            }
            // set all current_user attributes
            this.cache.current_user.set(key, response[key], {silent: true});
        }
        // save all the backbone models to forge prefs now
        forge.prefs.set('current_user', app.cache.current_user);

        var that = this;
        if (forge.is.mobile()) {
            that.mobile_sync();
        } else {
            that.web_bootstrap();
        }
    },
    web_bootstrap: function () {
        var that = this;
        forge.request.ajax({
            type: 'GET',
            url: encodeURI(that.api_url + 'peep'),
            dataType: 'json',
            success: function (data, headers) {
                that.cache_peeps(data['peeps']);
                forge.prefs.get('active_tab', function (value) {
                    that.router.navigate('#' + value, { trigger: true });
                });
            },
            error: function(error) {
                utils.alert('Web contacts bootstrap failed', error);
                utils.log_error_response('app.web_bootstrap:request[error]: ', error);
            }
        });
    },
    mobile_sync: function () {
        var that = this;
        forge.contact.selectAll(['name', 'emails', 'phoneNumbers'], function (contactList) {
            // clean up the contact list (only include contacts with phone numbers)
            var sync_arr = [];
            var i = contactList.length;
            while (i--) {
                var p = contactList[i];
                if (p.phoneNumbers) {
                    sync_arr.push(p);  // array sent to server for contact sync
                };
            };
            forge.request.ajax({
                type: 'POST',
                url: encodeURI(that.api_url + 'sync'),
                dataType: 'json',
                contentType: 'json',
                data: JSON.stringify(sync_arr),
                success: function (data, headers) {
                    forge.logging.log('app.mobile_sync:post_contacts[success]');
                    that.state.set('sync_job', data['job_id']);
                    that.mobile_sync_poller();
                },
                error: function (error) {
                    utils.alert('Mobile contacts failed to sync', error);
                    utils.log_error_response('app.mobile_sync:post_contacts[error]: ', error);
                    forge.prefs.get('active_tab', function (value) {
                        that.router.navigate('#' + value, { trigger: true });
                    });
                }
            });
        }, function (content) {
            utils.alert('Mobile contacts failed to select', error);
            utils.log_error_response('app.mobile_sync:selectAll[error]: ', content);
            forge.prefs.get('active_tab', function (value) {
                that.router.navigate('#' + value, { trigger: true });
            });
        });
    },
    mobile_sync_poller: function () {
        var that = this;
        var timeout = "";
        var poller = function() {
            var job_id = that.state.get('sync_job');
            forge.request.ajax({
                type: 'GET',
                url: encodeURI(that.api_url + 'sync/' + job_id),
                dataType: 'json',
                success: function (data, headers) {
                    // [HACK] trigger.io is shit and cant differentiate between 200 and 202 HTTP status codes
                    if (!data['status']){
                        clearInterval(timeout);
                        that.cache_peeps(data['peeps']);
                        that.state.set('sync_job', null);  // reset the sync_job id to null
                        forge.prefs.get('active_tab', function (value) {
                            that.router.navigate('#' + value, { trigger: true });
                        });
                    }
                },
                error: function(error) {
                    utils.alert('Mobile contacts failed to poll', error);
                    utils.log_error_response('app.mobile_sync_poller:poller[error]: ', error);
                }
            });
        };
        timeout = setInterval(poller, 2000);  // poll the server every 2 secs for a completion result
    }
    activate_mobile_btn: function (name, cb) {
        var that = this;
        var timer = setInterval(function () {
            var btn = that.state.get(name);
            if (btn) {
                clearInterval(timer);
                cb(btn);
            }
        }, 200);
    },
    hide_view: function (el) {
        if (el !== undefined || el !== null) {
            el.hide();
            if (el.length > 0) { // check view isnt empty
                var view = this.state.get(el.attr('class'));
                if (view ===  undefined || view === null) {
                    el.remove(); // this view is regenerated each use
                } else {
                    if (el.attr('class') !== 'login-view' &&
                        el.attr('class') !== 'signup-view') {
                        view.el = el;
                        this.state.set(el.attr('class'), view);
                    }
                }
            }
        }
    },
    cache_peeps: function (peeps) {
        for (var item in peeps) {
            var p = peeps[item];
            var peep = new Peep({
                'display_name': p.display_name,
                'mobile_id': p.mobile_id,
                'visible': p.visible,
                'registered': p.registered
            });
            peep.set('id', p.id, {silent: true});
            this.cache.peeps.add(peep);
        }
    },
    save_view: function (name, view) {
        this.state.set(name, view);
        view.show();
    },
    show_view: function (name, opts) {
        var that = this;
        var view = this.state.get(name);
        if (view) {
            view.show();
        } else {
            if (name === 'login-view') {
                var Login = require('views/login');
                new Login().render(function (v) {
                    that.save_view(name, v);
                });
            } else if (name === 'signup-view') {
                var Signup = require('views/signup');
                new Signup().render(function (v) {
                    that.save_view(name, v);
                });
            } else if (name === 'tabbar-view') {
                var Tabbar = require('views/tabbar');
                new Tabbar().render(function (v) {
                    that.save_view(name, v);
                });
            } else if (name === 'topbar-peeps-view') {
                var TopbarPeeps = require('views/topbar-peeps');
                new TopbarPeeps().render(function (v) {
                    that.save_view(name, v);
                });
            } else if (name === 'content-peeps-view') {
                var ContentPeeps = require('views/content-peeps');
                new ContentPeeps({
                    collection: that.cache.peeps
                }).render(function (v) {
                    that.save_view(name, v);
                });
            }
        }
    }
};

module.exports = App;
