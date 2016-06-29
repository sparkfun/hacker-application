/*global module:true, require:true, Backbone:true*/

var app = require('../app');
var utils = require('../lib/utilities');

module.exports = Backbone.Model.extend({
    urlRoot: encodeURI(app.api_url + 'peep'),
    synced: false,
    synced_time: null,
    parse: function (response) {
        response.birth_date = utils.python_iso_to_js_iso(response.birth_date);
        response.created = utils.python_iso_to_js_iso(response.created);
        response.edited = utils.python_iso_to_js_iso(response.edited);
        return response;
    },
    is_synced: function () {
        return this.synced;
    },
    last_sync: function () {
        return this.synced_time;
    }
});
