(function () {

    /**     
     * @param {any} options
       {
            rootElement: DOMElement,
            modal: boolean,
            data: object,
            onSave: function(osData) {} 
       }
     */
    function ServiceEditor(options) {

        // ==== private
        // ===========================================
        var settings = {};
        var editor = null;
        var self = this;
        var $context = null;
        var serviceApi = app.services.Services.getInstance();

        function initConstructor() {

            settings.owner = self;
            settings.initElements = initElements;
            settings.pageUrl = '../Services/_ServiceUI';

            editor = new app.BaseEditor(settings, options);
            editor.init();

            $context = editor.getContext();
        }

        function initElements() {

            var $form = $context.find('form');
            options.data !== null && typeof options.data === 'object' && app.formUtils.loadData({ formSelector: $form, data: options.data });
            enableFormValidation($form);

            app.maskUtils.maskAll($context);

            bindEvents();
        }

        function enableFormValidation($form) {

            var formValidator = $form.validate({
                rules: {
                    name: {
                        required: true,
                        minlength: 3,
                        maxlength: 100
                    },
                    honorary: {
                        required: true,
                        min: 0,
                        number: true
                    },
                    rate: {
                        required: true,
                        min: 0,
                        number: true
                    },
                    plateCard: {
                        required: true,
                        min: 0,
                        number: true
                    },
                    other: {
                        required: true,
                        min: 0,
                        number: true
                    }
                }
            });

            editor.setFormValidator(formValidator);
        }

        function bindEvents() {

            $context.find('form').submit(handleSaveService);
        }


        // ==== handles
        // ===========================================
        function handleSaveService(e) {

            e.preventDefault();

            editor.submitForm(e.target, function (data) {

                var result = {
                    newData: +data.id === 0
                };

                if (result.newData)
                    result.sendData = serviceApi.createService;
                else
                    result.sendData = serviceApi.updateService;

                return result;
            });
        }


        // => init
        // ============================================
        initConstructor();
    }

    window.app.ServiceEditor = ServiceEditor;
})();