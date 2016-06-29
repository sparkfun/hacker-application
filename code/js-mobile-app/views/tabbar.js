/*global forge:true, module:true, require:true*/

var app = require('../app');
var View = require('./view');
var template = require('./templates/tabbar');

module.exports = View.extend({
    tagName: 'div',
    className: 'tabbar-view',
	initialize: function () {
        var that = this;
        if (forge.is.web()) {
            this.template = template;
        } else {
            forge.tabbar.removeButtons();;
            forge.tabbar.addButton({
                text: 'Peeps',
                icon: 'img/buttons/glyphicons_071_book@2x.png',
                index: 3
            }, function (button) {
                app.state.set('tabbar-peeps', button);
                button.onPressed.addListener(function () {
                    that.tabbar_peeps();
                });
            });
        }
        return this;
	},
    post_render: function () {
        if (forge.is.web()) {
            $('#tabbar-peeps', this.el).hammer();
            $('#tabbar-buttons').append(this.el);
        }
    },
    hammerEvents: {
        'tap #tabbar-peeps': 'tabbar_peeps'
    },
    tabbar_peeps: function () {
        app.router.navigate('#peeps', { trigger: true });
    },
    show: function () {
        if (forge.is.mobile()) {
            forge.tabbar.show();
        } else {
            $('.tabbar').show();
        }
    }
});
