/*global forge:true, module:true, require:true*/

var app = require('../app');
var View = require('./view');
var template = require('./templates/topbar-peeps');
var utils = require('../lib/utilities');

module.exports = View.extend({
    tagName: 'div',
    className: 'topbar-peeps-view',
	initialize: function () {
        var that = this;
        if (forge.is.web()) {
            this.template = template;
        }
        return this;
	},
    post_render: function () {
        if (forge.is.web()) {
            $('#topbar-peeps-sort', this.el).hammer();
            $('#topbar-buttons').append(this.el);
            $('.topbar-peeps-view').hide();
        }
    },
    hammerEvents: {
        'tap #topbar-peeps-sort': 'topbar_peeps_sort'
    },
    topbar_peeps_sort: function () {
        utils.alert("TODO", "Sort based on visibility/registration?");
    },
    show: function () {
        var that = this;
        if (forge.is.mobile()) {
            forge.topbar.removeButtons();
            forge.topbar.setTitle('Peeps');
            forge.topbar.addButton({
                text: 'Sort',
                style: 'done',
                index: 1
            }, function (button) {
                that.topbar_peeps_sort();
            });
            forge.topbar.show();
        } else {
            $('.topbar').show();
            $('.topbar-peeps-view').show();
        }
    }
});
