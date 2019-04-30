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
    function OrderServiceItemEditor(options) {

        // ==== private
        // ===========================================
        var settings = {};
        var editor = null;
        var self = this;
        var $context = null;
        var osItemsApi = app.services.OrderServiceItems.getInstance();

        // ==== repositories
        // ==========================================
        var servicesData = [];

        // ==== handles
        // ===========================================
        function handleSaveOrderDetail(e) {

            e.preventDefault();
            
            editor.submitForm(e.target, function (orderItem) {

                var result = {
                    newData: +orderItem.id === 0
                };

                if (result.newData)
                    result.sendData = osItemsApi.createItem;
                else
                    result.sendData = osItemsApi.updateItem;

                return result;
            });
        }

        function handleServiceChange(e) {

            var serviceId = parseInt($(e.target).val());

            if (!(serviceId > 0))
                return;

            var service = servicesData.find(function (x) { return x.id === serviceId; });

            if (service === undefined) {
                app.notify.error('Falha ao obter dados do serviço');
                return;
            }

            app.formUtils.loadData({
                formSelector: $context.find('form'),
                data: {
                    "service.honorary": service.honorary,
                    "service.rate": service.rate,
                    "service.plateCard": service.plateCard,
                    "service.other": service.other
                }
            });            
        }

        function handleSelectNewService(service) {

        }

        function initElements() {          

            var $form = $context.find('form');
            if (options.data !== null && typeof options.data === 'object') {
                app.formUtils.loadData({
                    formSelector: $form,
                    data: options.data,
                    map: {
                        vehicleId: [null, 'vehicle.model'],
                        serviceId: [null, 'service.name']
                    }
                });
            }
            enableFormValidation($form);

            app.maskUtils.maskAll($context);

            app.ui.select.services({
                selectSelector: $context.find('[name="serviceId"]'),
                selected: options.data !== undefined && options.data !== null ? options.data.serviceId : undefined,
                onNewData: function (service) { servicesData.push(service); $(this).change(); }
            }).then(function (data) { servicesData = data; });

            app.ui.select.vehicles({
                selectSelector: $context.find('[name="vehicleId"]'),
                selected: options.data !== undefined && options.data !== null ? options.data.vehicleId : undefined,
                clientId: options.clientId
            });

            $context.find('.selectpicker').selectpicker();

            $context.find('[name="serviceId"]').change(handleServiceChange);

            bindEvents();
        }

        function initConstructor() {

            settings.owner = self;
            settings.initElements = initElements;
            settings.pageUrl = '../ServiceOrders/_OrderServiceItemUI';

            editor = new app.BaseEditor(settings, options);
            editor.init();

            $context = editor.getContext();
        }

        function bindEvents() {

            $context.find('form').submit(handleSaveOrderDetail);
        }

        function enableFormValidation($form) {

            var formValidator = $form.validate({
                rules: {
                    serviceId: {
                        selectRequired: true
                    },
                    vehicleId: {
                        selectRequired: true                        
                    }
                }
            });

            editor.setFormValidator(formValidator);
        }



        // => init
        // ============================================
        initConstructor();
    }

    window.app.OrderServiceItemEditor = OrderServiceItemEditor;

})();