/*global module:true, require:true, Backbone:true*/

var app = require('../app');
var model = require('./peep');

module.exports = Backbone.Collection.extend({
    model: model,
    url: encodeURI(app.api_url + 'peep'),
    parse: function (response) {
        return response.peeps;
    }
});
