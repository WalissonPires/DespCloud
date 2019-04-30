(function () {

    function ServiceOrdersManager(options) {

        // ==== private 
        // =====================================
        var $context = $(options.rootElement);
        var $tableOS = null;
        var osEditor = null;
        var orderServiceApi = app.services.ServiceOrders.getInstance();

        function initConstructor() {

            bindEvents();

            initTableOrdersService();
        }

        function initTableOrdersService() {

            var tableOptions = app.bootstrapTableUtils.mergeDefaultOptions({                
                toolbar: '#appTabServiceOrders .btn-group-manager',
                url: app.services.ServiceOrders.getSearchUrl(),
                uniqueId: 'id',
                columns: [{
                    radio: 'state',
                    title: ''
                }, {
                    field: 'id',
                    title: 'Protocolo',
                    sortable: true,
                    formatter: app.bootstrapTableUtils.idFormat
                }, {
                    field: 'client.name',
                    title: 'Cliente',
                    sortable: true
                }, {
                    field: 'createAt',
                    title: 'Criada em',
                    formatter: app.bootstrapTableUtils.dateFormat
                }, {
                    field: 'status',
                    title: 'Status',
                    sortable: true,
                    formatter: function (value) { return app.entities.OrderStatus[value]; }
                }, {
                    field: 'total',
                    title: 'Valor total',
                    formatter: app.bootstrapTableUtils.currency
                }]
            });

            $tableOS = $('[name="tableServiceOrders"]').bootstrapTable(tableOptions);
        }

        function bindEvents() {

            $context.find('[name="newOS"]').click(handleNewOS);
            $context.find('[name="editOS"]').click(handleEditOS);
            $context.find('[name="printReceipt"]').click(handlePrintReceipt);
        }

        function openOSModal(data) {

            resolveAttributesValues(data);

            var editorOptions = {
                modal: true,
                data: data,
                onSave: function (order, newOrder) {

                    if (newOrder) {
                        var rowData = app.bootstrapTableUtils.getDataById($tableOS, order.id);

                        if (rowData === null)
                            app.bootstrapTableUtils.insertData($tableOS, order);
                    }
                    else {
                        app.bootstrapTableUtils.updateData($tableOS, order, order.id);
                    }

                    app.notify.success('OS salva com sucesso');

                    return false;
                }
            };
            osEditor = new app.OrderServiceEditor(editorOptions);   
        }

        function resolveAttributesValues(data) {
            
        }

        // ==== handle
        // =====================================
        function handleNewOS(e) {

            openOSModal({
                id: 0,
                createAt: moment().format('YYYY-MM-DD'),
                total: 0,
                status: app.entities.OrderStatus.OPENED,
                client: {
                    id: ''
                }
            });  
        }

        function handleEditOS(e) {

            var order = app.bootstrapTableUtils.getSelected($tableOS)[0];

            if (order === undefined) {

                app.notify.warn('Selecione uma ordem de serviço');
                return;
            }

            $(e.target).attr('disabled', 'disabled');
            orderServiceApi.getById(order.id)
                .then(function (order) {

                    $(e.target).attr('disabled', null);
                    openOSModal(order);
                })
                .catch(function (error) {

                    $(e.target).attr('disabled', null);
                    if (error.title === undefined)
                        app.notify.error('Ops, falha inesperada. Contacte o suporte técnico. ' + (error.message || ''));
                    else
                        app.notify.error(error.title);
                });
        }

        function handlePrintReceipt(e) {

            var order = app.bootstrapTableUtils.getSelected($tableOS)[0];

            if (order === undefined) {

                app.notify.warn('Selecione uma ordem de serviço');
                return;
            }

            if (order.status !== app.entities.OrderStatus.CLOSED) {
                app.notify.warn('Finalize a OS para realizar a impressão do recibo');
                return;
            }

            orderServiceApi.printReceipt(order.id);
        }

        // ==== init 
        // =====================================
        initConstructor();
    }

    window.app.ServiceOrdersManager = ServiceOrdersManager;
})();