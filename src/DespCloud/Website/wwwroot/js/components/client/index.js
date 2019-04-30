(function () {

    function ClientsManager(options) {

        // ==== private 
        // =====================================
        var $context = $(options.rootElement);
        var $tableClients = null;
        var clientEditor = null;
        var clientApi = app.services.Clients.getInstance();

        function initConstructor() {

            bindEvents();

            initTableClients();
        }

        function initTableClients() {

            var tableOptions = app.bootstrapTableUtils.mergeDefaultOptions({
                toolbar: '#appTabClients .btn-group-manager',
                url: app.services.Clients.getSearchUrl(),
                uniqueId: 'id',
                columns: [{
                    radio: 'state',
                    title: ''
                }, {
                    field: 'id',
                    title: 'Código',
                    sortable: true,
                    formatter: app.bootstrapTableUtils.idFormat
                }, {
                    field: 'name',
                    title: 'Nome',
                    sortable: true
                }, {
                    field: 'phone',
                    title: 'Telefone',
                    formatter: null
                }, {
                    field: 'contactName',
                    title: 'Contato',
                    visible: false,
                    sortable: true
                }, {
                    field: 'email',
                    title: 'Email',
                    sortable: true
                }, {
                    field: 'cpfCnpj',
                    title: 'CPF/CNPJ',
                    visible: false,
                    formatter: null
                }, {
                    field: 'rgIE',
                    title: 'RG/IE',
                    visible: false,
                    sortable: true,
                    formatter: null
                }, {
                    field: 'org',
                    title: 'Orgão Emissor',
                    visible: false,
                }, {
                    field: 'address',
                    title: 'Endereço',
                    sortable: true,
                    formatter: app.bootstrapTableUtils.addressFormat
                }]
            });

            $tableClients = $('[name="tableClients"]').bootstrapTable(tableOptions);
        }

        function bindEvents() {

            $context.find('[name="newClient"]').click(handleNewClient);
            $context.find('[name="editClient"]').click(handleEditClient);
            $context.find('[name="deleteClient"]').click(handleDeleteClient);
        }

        function openClientModal(data) {

            var editorOptions = {
                modal: true,
                data: data,
                onSave: function (client, newClient) {

                    if (newClient) {
                        var rowData = app.bootstrapTableUtils.getDataById($tableClients, client.id);

                        if (rowData === null)
                            app.bootstrapTableUtils.insertData($tableClients, client);
                    }
                    else {
                        app.bootstrapTableUtils.updateData($tableClients, client, client.id);
                    }

                    app.notify.success('Cliente salvo com sucesso');

                    return true;
                }
            };

            clientEditor = new app.ClientEditor(editorOptions);
        }


        // ==== handle
        // =====================================
        function handleNewClient(e) {

            openClientModal({
                id: 0
            });
        }

        function handleEditClient(e) {

            var client = app.bootstrapTableUtils.getSelected($tableClients)[0];

            if (client === undefined) {

                app.notify.warn('Selecione um cliente');
                return;
            }

            $(e.target).attr('disabled', 'disabled');
            clientApi.getById(client.id)
                .then(function (client) {

                    $(e.target).attr('disabled', null);
                    openClientModal(client);
                })
                .catch(function (error) {

                    $(e.target).attr('disabled', null);
                    if (error.title === undefined)
                        app.notify.error('Ops, falha inesperada. Contacte o suporte técnico. ' + (error.message || ''));
                    else
                        app.notify.error(error.title);
                });
        }

        function handleDeleteClient(e) {

            var client = app.bootstrapTableUtils.getSelected($tableClients)[0];

            if (client === undefined) {

                app.notify.warn('Selecione um cliente');
                return;
            }

            $(e.target).attr('disabled', 'disabled');
            clientApi.deleteClient(client.id)
                .then(function () {

                    $(e.target).attr('disabled', null);                    
                    app.bootstrapTableUtils.removeData($tableClients, client.id);
                    app.notify.success('Cliente excluido');
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

    window.app.ClientsManager = ClientsManager;
})();