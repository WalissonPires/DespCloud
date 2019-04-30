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
    function ClientEditor(options) {

        // ==== private
        // ===========================================
        var settings = {};
        var editor = null;
        var self = this;
        var $context = null;
        var clientApi = app.services.Clients.getInstance();

        // ==== handles
        // ===========================================
        function handleSaveClient(e) {

            e.preventDefault();

            editor.submitForm(e.target, function (data) {

                var result = {
                    newData: +data.id === 0
                };

                if (result.newData)
                    result.sendData = clientApi.createClient;
                else
                    result.sendData = clientApi.updateClient;

                return result;
            });            
        }


        function initElements() {

            var $form = $context.find('form');
            options.data !== null && typeof options.data === 'object' && loadDataToForm($form, options.data);
            enableFormValidation($form);

            app.maskUtils.maskAll($context);

            app.ui.select.addressUI($context);

            bindEvents();
        }

        function initConstructor() {

            settings.owner = self;
            settings.initElements = initElements;
            settings.pageUrl = '../Clients/_ClientUI';

            editor = new app.BaseEditor(settings, options);
            editor.init();

            $context = editor.getContext();
        }

        function bindEvents() {

            $context.find('form').submit(handleSaveClient);
        }

        function enableFormValidation($form) {

            var formValidator = $form.validate({
                rules: {
                    name: {
                        required: true,
                        minlength: 3,
                        maxlength: 100
                    },
                    email: {
                        email: true,
                        minlength: 3,
                        maxlength: 60
                    },
                    cpfCnpj: {
                        minlength: 14,
                        maxlength: 18
                    },
                    phone: {
                        minlength: 14,
                        maxlength: 15
                    },
                    contactName: {
                        minlength: 3,
                        maxlength: 60
                    },
                    rgIe: {
                        minlength: 3,
                        maxlength: 20
                    },
                    org: {
                        minlength: 2,
                        maxlength: 20
                    },
                    'address.number': {
                        //required: true,
                        minlength: 1,
                        maxlength: 30
                    },
                    'address.street': {
                        //required: true,
                        minlength: 2,
                        maxlength: 200
                    },
                    'address.districtId': {
                        //required: true
                    },
                    'address.cityId': {
                        //required: true
                    },
                    'address.countyId': {
                        //required: true
                    }
                }
            });

            editor.setFormValidator(formValidator);
        }

        function loadDataToForm($form, data) {

            app.formUtils.loadData({
                formSelector: $form,
                data: data,
                map: {
                    'address.countyId'  : [null, 'address.countyName'],
                    'address.cityId'    : [null, 'address.cityName'],
                    'address.districtId': [null, 'address.districtName']
                }
            });
        }


        // => init
        // ============================================
        initConstructor();
    }

    window.app.ClientEditor = ClientEditor;

})();