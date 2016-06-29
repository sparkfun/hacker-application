/*global require:true, forge:true, FastClick:true*/

if (forge.config.modules.parameters.config.debug)
{
    forge.enableDebug();
}

var app = require('app');

$(function () {
    // resize the navbars to proper size based on window width (applies only to web-based version)
    $(window).bind('resize', function () {
        var w = $(window).width();
        if (w <= 767) {
            $('#tabbar-buttons').width(w);
            $('#topbar-buttons').width(w);
        } else if (w >= 768 && w <= 979) {
            $('#tabbar-buttons').width(724);
            $('#topbar-buttons').width(724);
        } else if (w >= 980 && w <= 1199) {
            $('#tabbar-buttons').width(940);
            $('#topbar-buttons').width(940);
        } else if (w >= 1200) {
            $('#tabbar-buttons').width(1170);
            $('#topbar-buttons').width(1170);
        }
    });
    // polyfill to remove click delays on browsers with touch ui
    window.addEventListener('load', function () {
        FastClick.attach(document.body);
    }, false);
    // initialize the application
    app.initialize();
});
