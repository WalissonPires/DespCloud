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
    function OrderServiceEditor(options) {

        // ==== private
        // ===========================================
        options = options || {};
        var self = this;
        var id = 'order-service-editor-' + new Date().getTime();
        var $context = null;
        var HTML_LOADING = '<div class="app-card-load-content" style="background-color: #fff; padding: 20px;">Carregando...</id>';
        var orderServiceApi = app.services.ServiceOrders.getInstance();
        var orderItemApi = app.services.OrderServiceItems.getInstance();
        var formValidator = null;
        var osItemUI = null;
        var $tableItems = null;

        // ===== handles
        // ===========================================
        function handleAddService(e) {

            var order = $context.find('form').serializeJson();

            if (+order.id === 0) {
                app.notify.warn('Salve a ordem de serviço antes de adicionar os items');
                return;
            }

            openItemModal({
                orderId: order.id
            });
        }

        function handleEditService(e) {

            var orderItem = app.bootstrapTableUtils.getSelected($tableItems)[0];
            if (orderItem === undefined) {
                app.notify.error('Selecione um item');
                return;
            }

            $(e.target).attr('disabled', 'disabled');

            orderItemApi.getById(orderItem.id)
                .then(function (orderItem) {

                    $(e.target).attr('disabled', null);
                    openItemModal(orderItem);
                })
                .catch(function (error) {

                    $(e.target).attr('disabled', null);
                    if (error.title === undefined)
                        app.notify.error('Ops, falha inesperada. Contacte o suporte técnico. ' + (error.message || ''));
                    else
                        app.notify.error(error.title);
                });
        }

        function handleRemoveService(e) {

            var orderItem = app.bootstrapTableUtils.getSelected($tableItems)[0];
            if (orderItem === undefined) {
                app.notify.error('Selecione um item');
                return;
            }

            orderItemApi.deleteItem(orderItem.id)
                .then(function () {

                    app.bootstrapTableUtils.removeData($tableItems, orderItem.id);
                    updateOrderTotalField();
                    app.notify.success('Item deletado com sucesso');
                })
                .catch(function (error) {

                    if (error.title === undefined)
                        app.notify.error('Ops, falha inesperada. Contacte o suporte técnico. ' + (error.message || ''));
                    else
                        app.notify.error(error.title);
                });

        }

        function handleSaveOrder(e) {

            e.preventDefault();

            if ($(e.target).valid() === false)
                return;

            var formOptions = {
                formatterValue: function (value, inputName) {
                    return value;
                }
            };

            var order = $(this).serializeJson(formOptions);

            disabledBtnActions();

            var promise = null;
            var newOrder = (+order.id) === 0;
            if (newOrder) 
                promise = orderServiceApi.createOrder(order);
            else
                promise = orderServiceApi.updateOrder(order);

            promise
                .then(function (order) {
                    
                    if (typeof options.onSave === 'function') {
                        if (options.onSave.call(self, order, newOrder) === true && options.modal)
                            app.modal.hide(id);

                        if (newOrder)
                            setNoUpdateElements();
                    }                
                
                    self.updateData(order);
                })
                .catch(function (response) {

                    if (response.title === undefined) {
                        app.notify.error('Ops, falha inesperada. Contacte o suporte técnico. ' + (response.message || ''));
                        return;
                    }

                    if (response.status === 400) {
                        formValidator.showErrors(response.errors);
                        app.notify.warn(response.title);
                    }
                    else {
                        app.notify.error(response.title);
                    }
                })
                .then(function () { enableBtnActions(); });
        }


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

            $.get('../ServiceOrders/_OrderServiceUI')
                .done(function (html) {

                    if (options.modal)
                        $context.iziModal('setContent', html);
                    else
                        $context.html(html);

                    initElements();
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

        function initElements() {
                       
            var $form = $context.find('form');
            if (options.data !== null && typeof options.data === 'object') {
                self.updateData(options.data);

                if (options.data.status !== 0) {

                    isReadOnly($form);
                }
            }
            enableFormValidation($form);

            app.ui.select.clients({
                selectSelector: $context.find('[name="clientId"]'),
                selected: options.data !== undefined && options.data !== null ? options.data.clientId : undefined
            });

            $context.find('.selectpicker').selectpicker();

            var items = options.data !== undefined && options.data !== null ? options.data.items : [];
            initServiceTable(items);

            if (options.data !== undefined && options.data.id > 0)
                setNoUpdateElements();
            else {
                $form.find('[name="status"]').attr('disabled', true).selectpicker('refresh');
            }

            bindEvents();            
        }

        function enableFormValidation($form) {

            formValidator = $form.validate({
                rules: {
                    createAt: {
                        required: true
                    },
                    "clientId": {
                        selectRequired: true
                    }
                }
            });
        }

        function initServiceTable(data) {

            var tableOptions = app.bootstrapTableUtils.mergeDefaultOptions({
                toolbar: '#' + id + ' .btn-group-manager',
                url: undefined,
                uniqueId: 'id',
                data: data,
                columns: [{
                    radio: 'state',
                    title: ''
                }, {
                    field: 'id',
                    title: 'Código',
                    sortable: true,
                    visable: false
                }, {
                    field: 'service.name',
                    title: 'Serviço',
                    sortable: true
                }, {
                    field: 'vehicle.model',
                    title: 'Veículo'
                }, {
                    field: 'total',
                    title: 'Valor Total',
                    formatter: app.bootstrapTableUtils.currency
                }]
            });

            $tableItems = $context.find('[name="tableServices"]').bootstrapTable(tableOptions);
        }

        function disabledBtnActions() {

            $context.find('.row-group-btn .btn').attr('disabled', 'disabled');
        }

        function enableBtnActions() {

            $context.find('.row-group-btn .btn').attr('disabled', null);
        }

        function isReadOnly($form) {

            $form.find('.row-data').addClass('disabled');
            $form.find('[name="save"]').attr('disabled', true);
        }

        function bindEvents() {

            $context.find('[name="addService"]').click(handleAddService);
            $context.find('[name="editService"]').click(handleEditService);
            $context.find('[name="removeService"]').click(handleRemoveService);

            $context.find('form').submit(handleSaveOrder);
        }


        function updateOrderTotalField() {

            var items = $tableItems.bootstrapTable('getData', false);
            var total = 0;
            for (var i = 0; i < items.length; i++) {
                total += items[i].total;
            }

            $context.find('form [name="total"]').val(total);
        }

        function openItemModal(data) {

            var editorOptions = {
                modal: true,
                data: data,
                clientId: parseInt($context.find('[name="clientId"]').val()),
                onSave: function (orderItem, newOrderItem) {

                    if (newOrderItem) {
                        var rowData = app.bootstrapTableUtils.getDataById($tableItems, orderItem.id);

                        if (rowData === null)
                            app.bootstrapTableUtils.insertData($tableItems, orderItem);
                    }
                    else {
                        app.bootstrapTableUtils.updateData($tableItems, orderItem, orderItem.id);
                    }

                    updateOrderTotalField();
                    app.notify.success('Item salvo com sucesso');

                    return true;
                }
            };

            osItemUI = new app.OrderServiceItemEditor(editorOptions);
        }

        function setNoUpdateElements(disabled) {

            if (disabled === false) {
                $context.find('.noupdate:not(.bootstrap-select)').attr('disabled', null);
                $context.find('.noupdate.bootstrap-select').removeClass('disabled').children('.btn ').removeClass('disabled');
            }
            else {
                $context.find('.noupdate:not(.bootstrap-select)').attr('disabled', 'disabled');
                $context.find('.noupdate.bootstrap-select').addClass('disabled').children('.btn ').addClass('disabled');
            }
        }

        function resolveAttributesValues(data) {

            if (typeof data.createAt === 'string')
                data.createAt = moment(data.createAt).format('YYYY-MM-DD');   

            if (typeof data.closedAt === 'string')
                data.closedAt = moment(data.closedAt).format('YYYY-MM-DD');   
        }

        // => init
        // ============================================
        initConstructor();

        // ===== public
        // ============================================
        this.updateData = function (data) {

            resolveAttributesValues(data);

            var $form = $context.find('form');
            app.formUtils.loadData({
                formSelector: $form,
                data: data,
                map: {
                    clientId: [null, "client.name"]                    
                }
            });
        };
    }

    window.app.OrderServiceEditor = OrderServiceEditor;
})();