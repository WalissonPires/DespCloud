(function () {

    function VehiclesManager(options) {

        // ==== private 
        // =====================================
        var $context = $(options.rootElement);
        var $tableVehicles = null;
        var vehicleEditor = null;
        var vehicleApi = app.services.Vehicles.getInstance();

        function initConstructor() {

            bindEvents();

            initTableVehicles();
        }

        function initTableVehicles() {

            var tableOptions = app.bootstrapTableUtils.mergeDefaultOptions({
                toolbar: '#appTabVehicles .btn-group-manager',
                url: app.services.Vehicles.getSearchUrl(),
                uniqueId: 'id',
                columns: [{
                    radio: 'state',
                    title: ''
                }, {
                    field: 'id',
                    title: 'Código',
                    sortable: true,
                    formatter: function (value) { return (value + '').padStart(6, '0'); }
                }, {
                    field: 'clientId',
                    title: 'Cód. Cliente',
                    sortable: true,
                    formatter: function (value) { return (value + '').padStart(6, '0'); }
                }, {
                    field: 'client.name',
                    title: 'Cliente',
                    sortable: true
                }, {
                    field: 'plate',
                    title: 'Placa',
                    sortable: true
                }, {
                    field: 'manufacturer',
                    title: 'Fabricante',
                    sortable: true
                }, {
                    field: 'model',
                    title: 'Modelo',
                    sortable: true
                }, {
                    field: 'color',
                    title: 'Cor',
                    sortable: true
                }, {
                    field: 'type',
                    title: 'Tipo',
                    sortable: true,
                    formatter: function (value) { return app.entities.VehicleType[value]; }
                }, {
                    field: 'modalYear',
                    title: 'Ano',
                    sortable: true,
                        formatter: function (value, data) { return data.yearManufacture + '/' + data.modelYear; }
                }, {
                    field: 'cityName',
                    title: 'Cidade',
                    sortable: true
                }, {
                    field: 'countyInitials',
                    title: 'UF',
                    sortable: true
                }, {
                    field: 'chassis',
                    title: 'Chassi',
                    sortable: true
                }, {
                    field: 'renavam',
                    title: 'Renavam',
                    sortable: true
                }]
            });

            $tableVehicles = $('[name="tableVehicles"]').bootstrapTable(tableOptions);
        }

        function bindEvents() {

            $context.find('[name="newVehicle"]').click(handleNewVehicle);
            $context.find('[name="editVehicle"]').click(handleEditVehicle);
            $context.find('[name="deleteVehicle"]').click(handleDeleteVehicle);
        }

        function openVehicleModal(data) {

            var editorOptions = {
                modal: true,
                data: data,
                onSave: function (vehicle, newVehicle) {

                    if (newVehicle) {
                        var rowData = app.bootstrapTableUtils.getDataById($tableVehicles, vehicle.id);

                        if (rowData === null)
                            app.bootstrapTableUtils.insertData($tableVehicles, vehicle);
                    }
                    else {
                        app.bootstrapTableUtils.updateData($tableVehicles, vehicle, vehicle.id);
                    }

                    app.notify.success('Veículo salvo com sucesso');

                    return true;
                }
            };

            vehicleEditor = new app.VehicleEditor(editorOptions);
        }


        // ==== handle
        // =====================================
        function handleNewVehicle(e) {

            openVehicleModal({
                id: 0
            });
        }

        function handleEditVehicle(e) {

            var vehicle = app.bootstrapTableUtils.getSelected($tableVehicles)[0];

            if (vehicle === undefined) {

                app.notify.warn('Selecione um veículo');
                return;
            }

            $(e.target).attr('disabled', 'disabled');
            vehicleApi.getById(vehicle.id)
                .then(function (client) {

                    $(e.target).attr('disabled', null);
                    openVehicleModal(client);
                })
                .catch(function (error) {

                    $(e.target).attr('disabled', null);
                    if (error.title === undefined)
                        app.notify.error('Ops, falha inesperada. Contacte o suporte técnico. ' + (error.message || ''));
                    else
                        app.notify.error(error.title);
                });
        }

        function handleDeleteVehicle(e) {

            var vehicle = app.bootstrapTableUtils.getSelected($tableVehicles)[0];

            if (vehicle === undefined) {

                app.notify.warn('Selecione um veículo');
                return;
            }

            $(e.target).attr('disabled', 'disabled');
            vehicleApi.deleteClient(vehicle.id)
                .then(function () {

                    $(e.target).attr('disabled', null);                    
                    app.bootstrapTableUtils.removeData($tableVehicles, vehicle.id);
                    app.notify.success('Veículo excluido');
                })
                .catch(function (error) {

                    $(e.target).attr('disabled', null);
                    if (error.title === undefined)
                        app.notify.error('Ops, falha inesperada. Contacte o suporte técnico. ' + (error.message || ''));
                    else
                        app.notify.error(error.title);
                });
        }

        // ==== init 
        // =====================================
        initConstructor();
    }

    window.app.VehiclesManager = VehiclesManager;
})();