
(function () {

    var defaultConfig = {
        title: '',
        subtitle: '',
        headerColor: '#88A0B9',
        background: null,
        theme: '',  // light
        icon: null,
        iconText: null,
        iconColor: '',
        rtl: false,
        width: 800,
        top: null,
        bottom: null,
        borderBottom: true,
        padding: 0,
        radius: 1,
        zindex: 999999,
        iframe: false,
        iframeHeight: 400,
        iframeURL: null,
        focusInput: true,
        group: '',
        loop: false,
        arrowKeys: true,
        navigateCaption: true,
        navigateArrows: true, // Boolean, 'closeToModal', 'closeScreenEdge'
        history: false,
        restoreDefaultContent: false,
        autoOpen: 0, // Boolean, Number
        bodyOverflow: false,
        fullscreen: true,
        openFullscreen: false,
        closeOnEscape: false,
        closeButton: true,
        appendTo: 'body', // or false
        appendToOverlay: 'body', // or false
        overlay: true,
        overlayClose: false,
        overlayColor: 'rgba(0, 0, 0, 0.4)',
        timeout: false,
        timeoutProgressbar: false,
        pauseOnHover: false,
        timeoutProgressbarColor: 'rgba(255,255,255,0.5)',
        transitionIn: 'fadeIn',
        transitionOut: 'fadeOut',
        transitionInOverlay: 'fadeIn',
        transitionOutOverlay: 'fadeOut',
        onFullscreen: function () { },
        onResize: function () { },
        onOpening: function () { },
        onOpened: function () { },
        onClosing: function () {
            var op = this;
            setTimeout(function () { $('#' + op.id).remove(); }, 1000);
        },
        onClosed: function () { },
        afterRender: function () { }
    };

    function clone() {

        var copy = {};

        for (var j = 0; j < arguments.length; j++) {

            var props = Object.keys(arguments[j]);
            for (var i = 0; i < props.length; i++) {
                copy[props[i]] = arguments[j][props[i]];
            }
        }

        return copy;
    }


    function ModalApp() { }


    /**
     * param { id: string, contentHtml }
     */
    ModalApp.show = function (param) {

        $("#" + param.id).remove();

        var $modal = $('<div id="' + param.id + '" class="izi-modal-app"></div>');
        $modal.append(param.contentHtml);
        $modal.appendTo($('body'));

        var options = clone(defaultConfig, param);

        options.zindex += 10 * $('body .iziModal').length;

        $("#" + param.id).iziModal(options);
        $("#" + param.id).iziModal('open');
    };

    ModalApp.hide = function (modalId) {

        $("#" + modalId).iziModal('close');

    };


    app.modal = ModalApp;
})();
