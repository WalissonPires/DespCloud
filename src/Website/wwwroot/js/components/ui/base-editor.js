(function () {

    /**     
     * @param {any} options
       {            
            rootElement: DOMElement,
            modal: boolean,
            data: object,
            onSave: function(osData) {} 
       }
     *
     * @param {any} settings 
        {
            name: string,
            owner: Object,
            pageUrl: string, ajax
            initElements: function() { }
        }
     */
    function BaseEditor(settings, options) {

        // ==== private
        // ===========================================
        options = options || {};
        settings = settings || {};
        settings.name = settings.name || 'base';
        settings.owner = settings.owner || this;

        var self = this;
        var id = settings.name + '-editor-' + new Date().getTime();
        var $context = null;
        var HTML_LOADING = '<div class="app-card-load-content" style="background-color: #fff; padding: 20px;">Carregando...</id>';
        var clientApi = app.services.Clients.getInstance();
        var formValidator = null;


        function openModal() {

            app.modal.show({
                id: id,
                contentHtml: HTML_LOADING,
                onOpened: function () {

                    $context.find('[name="exit"]').attr('data-izimodal-close', '');
                }
            });

            $context = $('#' + id);

            loadHtml();
        }

        function openInline() {

            $context = $('<div id="' + id + '" class=""></div>');
            $context.html(HTML_LOADING);
            $context.appendTo(options.rootElement);

            loadHtml();
        }

        function loadHtml() {

            var promise;

            if (typeof settings.pageUrl === 'string')
                promise = $.get(settings.pageUrl);
            else if (typeof settings.pageUrl === 'function') {
                promise = settings.pageUrl.call(settings.owner);
                if (promise !== null || typeof promise.done !== 'function')
                    throw new Error('Expected ajax promise');
            }
                
            promise.done(function (html) {

                if (options.modal)
                    $context.iziModal('setContent', html);
                else
                    $context.html(html);

                settings.initElements && settings.initElements.call(settings.owner);
            })
            .fail(function (xhr) {

                app.notify.error('Falha ao carregar página');
            });
        }

        function initConstructor() {

            if (options.modal === true)
                openModal();
            else
                openInline();
        }

        // ===== public
        // ============================================
        this.getContext = function () {
            return $context;
        };

        this.init = function () {

            initConstructor();
        };

        this.disabledBtnActions = function () {

            $context.find('.row-group-btn .btn').attr('disabled', 'disabled');
        };

        this.enableBtnActions = function () {

            $context.find('.row-group-btn .btn').attr('disabled', null);
        };

        this.submitForm = function (formElement, getSendData) {

            try {
                if ($(formElement).valid() === false)
                    return;

                var formOptions = {
                    formatterValue: function (value, inputName) {

                        var $element = $context.find('[name="' + inputName + '"].masked');
                        if ($element.length > 0)
                            value = $element.cleanVal();

                        if (value === '')
                            return undefined;                    
                        
                        return value;
                    }
                };

                var data = $(formElement).serializeJson(formOptions);

                var sendDataResult = getSendData.call(settings.owner, data);

                if (sendDataResult === undefined)
                    throw new Error('Retorno do metodo não definido');

                if (sendDataResult.sendData === undefined)
                    throw new Error('Método de envio não configurado');

                self.disabledBtnActions();
            
                sendDataResult.sendData.call(settings.owner, data)
                    .then(function (data) {

                        app.formUtils.loadData({ formSelector: formElement, data: data });

                        if (typeof options.onSave === 'function') {
                            if (options.onSave.call(settings.owner, data, sendDataResult.newData) === true && options.modal)
                                app.modal.hide(id);
                        }

                    })
                    .catch(function (response) {

                        if (response.title === undefined) {
                            app.notify.error('Ops. Falha inesperada');
                            return;
                        }

                        if (response.status === 400 && formValidator !== null) {
                            formValidator.showErrors(response.errors);
                            app.notify.warn(response.title);
                        }
                        else {
                            app.notify.error(response.title);
                        }
                    })
                    .then(function () { self.enableBtnActions(); });
            }
            catch (e) {

                self.enableBtnActions(); 
                app.notify.error('Ops, falha inesperada. Contacte o suporte técnico.');
                console.log(e);                
            }
        };

        this.setFormValidator = function (validator) {

            formValidator = validator;
        };
    }

    window.app.BaseEditor = BaseEditor;

})();