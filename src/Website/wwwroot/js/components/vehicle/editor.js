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
    function VehicleEditor(options) {

        // ==== private
        // ===========================================
        var settings = {};
        var editor = null;
        var self = this;
        var $context = null;
        var vehicleApi = app.services.Vehicles.getInstance();

        function initConstructor() {

            settings.owner = self;
            settings.initElements = initElements;
            settings.pageUrl = '../Vehicles/_VehicleUI';

            editor = new app.BaseEditor(settings, options);
            editor.init();

            $context = editor.getContext();
        }

        function initElements() {

            var $form = $context.find('form');
            options.data !== null && typeof options.data === 'object' && loadDataToForm($form, options.data);
            enableFormValidation($form);

            createClientsSelectionIfNeeded($form);

            app.maskUtils.maskAll($context);

            $context.find('.selectpicker').selectpicker();

            app.ui.select.addressUI($context);

            bindEvents();
        }

        function enableFormValidation($form) {

            var formValidator = $form.validate({
                rules: {
                    model: {
                        required: true,
                        minlength: 3,
                        maxlength: 60
                    },
                    manufacturer: {
                        minlength: 3,
                        maxlength: 60
                    },
                    yearManufactureModel: {
                        minlength: 5,
                        maxlength: 5
                    },
                    type: {
                        required: true                       
                    },
                    plate: {
                        required: true,
                        minlength: 8,
                        maxlength: 8
                    },
                    chassi: {
                        minlength: 3,
                        maxlength: 17
                    },
                    renavam: {
                        minlength: 2,
                        maxlength: 11
                    },
                    color: {
                        minlength: 2,
                        maxlength: 20
                    },
                    countyId: {
                        required: true
                    },
                    cityId: {
                        required: true
                    }
                }
            });

            editor.setFormValidator(formValidator);
        }

        function bindEvents() {

            $context.find('form').submit(handleSaveVehicle);
            $context.find('[name="yearManufactureModel"]').change(handleYearManufactureModelChange);
        }

        function createClientsSelectionIfNeeded($form) {

            var $clientIdElement = $form.find('[name="clientId"]');
            var clientId = parseInt($clientIdElement.val());
            if (isNaN(clientId) || clientId > 0)
                return;

            var html = `
            <div class="row clearfix row-data">
	            <div class="col-sm-6">
		            <label>Cliente</label>
		            <div class="form-group">
			            <select name="clientId" data-live-search="true" class="form-control ui-selectpicker noupdate"></select>
		            </div>
	            </div>	
            </div>
            `;

            $clientIdElement.remove();
            $form.prepend(html);

            app.ui.select.clients({
                selectSelector: $context.find('[name="clientId"]'),
                selected: undefined
            });
        }

        function loadDataToForm($form, data) {
             
            if (data.yearManufacture > 0 && data.modelYear > 0)
                data.yearManufactureModel = data.yearManufacture + '/' + data.modelYear;

            app.formUtils.loadData({
                formSelector: $form,
                data: data,
                map: {
                    'countyId': [null, 'countyName'],
                    'cityId': [null, 'cityName']
                }
            });
        }

        // ==== handles
        // ===========================================
        function handleSaveVehicle(e) {

            e.preventDefault();

            editor.submitForm(e.target, function (data) {

                var result = {
                    newData: +data.id === 0
                };

                if (result.newData)
                    result.sendData = vehicleApi.createVehicle;
                else
                    result.sendData = vehicleApi.updateVehicle;

                return result;
            });
        }

        function handleYearManufactureModelChange(e) {

            var years = e.target.value.split('/');

            $context.find('[name="yearManufacture"]').val(years[0] || null);
            $context.find('[name="modelYear"]').val(years[1] || null);
        }


        // => init
        // ============================================
        initConstructor();
    }

    window.app.VehicleEditor = VehicleEditor;
})();