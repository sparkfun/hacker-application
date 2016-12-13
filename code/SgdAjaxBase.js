// SgdAjaxBase.js: Provides the basic AJAX interface that is used in all
// AJAX enabled web pages

var sgd_http = sgd_createRequestObject();
var sgd_base_url = document.URL; 
var sgd_lphp = sgd_base_url.lastIndexOf('.php');
if (sgd_lphp > -1) {
    sgd_base_url = sgd_base_url.substr(0,sgd_lphp);
}
var sgd_lslash = sgd_base_url.lastIndexOf('/');
sgd_base_url = sgd_base_url.substr(0,sgd_lslash);

function sgd_createRequestObject() {
    var objAjax;
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer") {
        objAjax = new ActiveXObject("Microsoft.XMLHTTP");
    } else {
        objAjax = new XMLHttpRequest();
    }
    return objAjax;
}
