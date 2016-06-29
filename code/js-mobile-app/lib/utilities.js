/*global require:true, module:true, Backbone:true, forge:true, Spinner:true*/

var Utils = {
    loading_spinner: null,
    show_loading: function () {
        $('.content').children(':visible').hide();
        $('body').append('<div id="spinner"><br /><br /><h1> Loading....</h1></div>');
        var tgt = document.getElementById('spinner');
        if (this.loading_spinner) {
            this.loading_spinner.spin(tgt);
        } else {
            this.loading_spinner = new Spinner().spin(tgt);
        }
    },
    hide_loading: function () {
        if (this.loading_spinner) {
            this.loading_spinner.stop();
            $('#spinner').remove();
        }
    },
    python_iso_to_js_iso: function (iso_str) {
        // convert python isostring to js isostring (account for Z offset)
        if (iso_str) {
            return new Date(iso_str).toISOString();
        }
        return null;
    },
    alert: function (title, msg) {
        if (forge.is.mobile()) {
            forge.notification.alert(title, msg, function () {
            }, function (err) {
                forge.logging.log(err);
            });
        } else {
            window.alert(msg);
        }
    },
    toggle_to_bool: function (status) {
        if (status === 'ON') {
            return true;
        } else {
            return false;
        }
    },
    merge_date_time: function (date, timestr) {
        var time = timestr.match(/(\d+)(?::(\d\d))?\s*(p?)/i);
        if (!time) {
            return date;
        }
        var hours = parseInt(time[1], 10);
        if (hours === 12 && !time[3]) {
            hours = 0;
        }
        else {
            hours += (hours < 12 && time[3]) ? 12 : 0;
        }
        date.setHours(hours);
        date.setMinutes(parseInt(time[2], 10) || 0);
        date.setSeconds(0, 0);
        return date;
    },
    log_error_response: function (msg, response) {
        forge.logging.log(msg + '[' + response.status + ']{"'
            + 'error":"' + response.err + '", '
            + '"status_code":"' + response.statusCode + '", '
            + '"type":"' + response.type + '", '
            + '"message":"' + response.message + '" }');
    }
};

module.exports = Utils;
