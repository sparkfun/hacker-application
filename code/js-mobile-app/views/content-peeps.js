/*global forge:true, module:true, require:true, iScroll:true*/
/*jslint newcap:false*/  // because iScroll is stupid and ignores convention

var app = require('../app');
var View = require('./view');
var template = require('./templates/content-peeps');
var utils = require('../lib/utilities');

module.exports = View.extend({
    tagName: 'div',
    className: 'content-peeps-view',
	initialize: function (props) {
        this.template = template;
        this.collection = props.collection;
        this.set_callbacks(); // track collection events
        return this;
	},
    set_callbacks: function () {
        // this.listenTo(this.collection, 'reset', this.reset_callback);
    },
    post_render: function () {
        $('.row-item', this.el).hammer();
        $('.content').append(this.el);
        this.scroller = new iScroll(this.className + "-scroller", {
            hScroll: false,
            hScrollbar: false,
            vScrollbar: false,
            checkDOMChanges: true
        });
    },
    hammerEvents: {
        'tap .row-item': 'display_peep_details'
    },
    get_data: function () {
        var peeps = [];
        for (var i = 0; i < this.collection.length; i++) {
            var peep = {};
            var model = this.collection.models[i];
            peep.id = model.id;
            var dn = model.attributes.display_name;
            peep.display_name = dn;
            peep.cap_letter = dn.charAt(0);
            peeps.push(peep);
        }
        var stream = {
            peeps: JSON.parse(JSON.stringify(peeps)),
            class_name: this.className
        };
        return stream;
    },
    display_peep_details: function (e) {
        app.router.navigate('#display-peep/' + e.currentTarget.id, { trigger: true });
    },
    show: function () {
        $('.content').show();
        $('.' + this.className).show();
    }
});
