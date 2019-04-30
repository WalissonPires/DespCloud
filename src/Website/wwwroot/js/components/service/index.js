(function () {

    function ServicesManager(options) {

        // ==== private 
        // =====================================
        var $context = $(options.rootElement);
        var $tableServices = null;
        var serviceEditor = null;
        var serviceApi = app.services.Services.getInstance();

        function initConstructor() {

            bindEvents();

            initTableServices();
        }

        function initTableServices() {

            var tableOptions = app.bootstrapTableUtils.mergeDefaultOptions({
                toolbar: '#appTabServices .btn-group-manager',
                url: app.services.Services.getSearchUrl(),
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
                    field: 'name',
                    title: 'Nome',
                    sortable: true
                }, {
                    field: 'honorary',
                    title: 'Honorário'
                }, {
                    field: 'rate',
                    title: 'Taxa'
                }, {
                    field: 'plateCard',
                    title: 'Placa/Targeta'
                }, {
                    field: 'other',
                    title: 'Outros'
                }]
            });

            $tableServices = $('[name="tableServices"]').bootstrapTable(tableOptions);
        }

        function bindEvents() {

            $context.find('[name="newService"]').click(handleNewService);
            $context.find('[name="editService"]').click(handleEditService);
            $context.find('[name="deleteService"]').click(handleDeleteService);
        }

        function openServiceModal(data) {

            var editorOptions = {
                modal: true,
                data: data,
                onSave: function (service, newService) {

                    if (newService) {
                        var rowData = app.bootstrapTableUtils.getDataById($tableServices, service.id);

                        if (rowData === null)
                            app.bootstrapTableUtils.insertData($tableServices, service);
                    }
                    else {
                        app.bootstrapTableUtils.updateData($tableServices, service, service.id);
                    }

                    app.notify.success('Serviço salvo com sucesso');

                    return true;
                }
            };

            serviceEditor = new app.ServiceEditor(editorOptions);
        }


        // ==== handle
        // =====================================
        function handleNewService(e) {

            openServiceModal({
                id: 0
            });
        }

        function handleEditService(e) {

            var service = app.bootstrapTableUtils.getSelected($tableServices)[0];

            if (service === undefined) {

                app.notify.warn('Selecione um serviço');
                return;
            }

            $(e.target).attr('disabled', 'disabled');
            serviceApi.getById(service.id)
                .then(function (service) {

                    $(e.target).attr('disabled', null);
                    openServiceModal(service);
                })
                .catch(function (error) {

                    $(e.target).attr('disabled', null);
                    if (error.title === undefined)
                        app.notify.error('Ops, falha inesperada. Contacte o suporte técnico. ' + (error.message || ''));
                    else
                        app.notify.error(error.title);
                });
        }

        function handleDeleteService(e) {

            var service = app.bootstrapTableUtils.getSelected($tableServices)[0];

            if (service === undefined) {

                app.notify.warn('Selecione um serviço');
                return;
            }

            $(e.target).attr('disabled', 'disabled');
            serviceApi.deleteService(service.id)
                .then(function () {

                    $(e.target).attr('disabled', null);                    
                    app.bootstrapTableUtils.removeData($tableServices, service.id);
                    app.notify.success('Serviço excluido');
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

    window.app.ServicesManager = ServicesManager;
})();