/*global require:true, module:true, Backbone:true, _:true*/

require('lib/view_helper');

module.exports = Backbone.View.extend({
    initialize: function () {
        this.render = _.bind(this.render, this);
        return this;
    },
    template: function () {},
    get_data: function () {},
    render: function (callback) {
        this.$el.html(this.template(this.get_data()));
        this.post_render();
        callback(this);
    },
    post_render: function () {},
    show: function () {}
});
