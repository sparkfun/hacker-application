/*
 This is the init function from the jQuery plugin controlling the HTML of the signature_creation.php

 Although incomplete, this is how we were running the main functionality of the page, so offer live
 updates and previews for creating a signature. This way, the user could customize their name, and 
 select a font for the signature to be drawn in, all without page reloads.
*/

init: function () {
    //init vars
    var that = this;

    that.config = $.extend({}, that.defaults, that.options);

    that.preview = that.esig_wrapper.find(that.config.previewWrapper);

    that.icons = that.esig_wrapper.find(that.config.iconsWrapper);

    that.overlay = that.esig_wrapper.find(that.config.overlay);

    if (that.preview.find('img').length == 0) {
        that.updatePreview();
    }

    var userNameInput = $(that.config.userName),
        userName = "";

    userNameInput.on('focus', function () {
        userName = that.getName()
    });

    userNameInput.on('blur', function () {
        if (userName != that.getName()) {
            that.updatePreview(that.config.onUpdate);
        }
    });

    var updateButton = $(that.config.updateButton);

    updateButton.on('click', function (e) {
        e.preventDefault();
        that.updatePreview(that.config.onUpdate);
    });

    var submitButton = $(that.config.submitButton);

    submitButton.on('click', function (e) {
        e.preventDefault();
        that.submitSignature(that.config.preSubmit, that.config.onSubmit);
    });

    that.icons.on('click', 'img', function () {
        var id = $(this).data('fontid');
        that.preview.find('img').hide();
        that.preview.find("img[data-fontid='" + id + "']").show();
    });

    that.icons.find("img[data-fontid=0]").trigger('click');

    return that;
},

updatePreview: function (preUpdate, postUpdate) {
    var that = this;

    typeof preUpdate === 'function' && preUpdate();

    var userName = $.trim(that.getName());

    if (userName != "") {
        var fontId = that.preview.find('img:visible').data('fontid');
        that.toggleOverlay('show');
        $.ajax({
            url: that.config.previewUrl,
            type: 'POST',
            dataType: 'json',
            data: {
                'userName': userName
            },
            success: function (response) {
                that.preview.empty();
                that.icons.empty();

                for (var i in response) {
                    that.preview.append('<img style="display: none;" data-fontid="' + i + '" src="data:image/png;charset=utf-8;base64,' + response[i] + '" />');
                    that.icons.append('<img data-fontid="' + i + '" src="data:image/png;charset=utf-8;base64,' + response[i] + '" />');
                }

                fontId = typeof fontId == 'undefined' ? 0 : fontId;

                that.icons.find("img[data-fontid='" + fontId + "']").trigger('click');

                typeof postUpdate === 'function' && postUpdate(response);

                that.toggleOverlay('hide');
            }
        });
    }
},