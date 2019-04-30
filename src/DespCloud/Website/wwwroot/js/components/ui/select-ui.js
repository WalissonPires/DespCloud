(function () {   

    function getSelectData(options) {

        var $select = $(options.selectSelector);

        var promise = new Promise(function (resolve, reject) {

            options._getData(options._getDataParams)
                .then(function (serverData) {

                    data = serverData.map(options._dataMap);

                    var item;
                    var selected = options.selected !== undefined ? options.selected : '';

                    data.unshift({ id: '#new', text: options._newOptionText });
                    data.unshift({ id: '', text: 'Selecionar...' });

                    $select.empty();
                    for (var i = 0; i < data.length; i++) {
                        item = data[i];
                        $select.append(new Option(item.text, item.id, selected === item.id, selected === item.id));
                    }

                    if ($select.hasClass('ui-selectpicker')) {
                        $select.removeClass('ui-selectpicker').addClass('selectpicker');
                        $select.selectpicker();
                    }

                    $select.change(function (e) {

                        if ($select.val() === '#new') {

                            $select.selectpicker('val', '');

                            options._callEditor.call(null, $select);
                        }
                    });

                    resolve(serverData);

                }).catch(function (error) {

                    app.notify.error('Falha ao carregar dados');
                    reject(error);
                });
        });

        return promise;
    }

    function getSelectDataSimple(options) {

        var $select = $(options.selectSelector);
        
        var promise = options._getData.call();
        promise = app.utils.fetch.parseResponse(promise);

        promise
            .then(function (serverData) {

                var data = serverData.map(options._dataMap);                
                var selected = $select.val();
                $select.empty();
                var item, op;
                for (var i = 0; i < data.length; i++) {
                    item = data[i];
                    op = new Option(item.text, item.id, item.id == selected, item.id == selected);
                    if (item.data !== undefined) {
                        for (var key in item.data)
                            $(op).data(key, item.data[key]);
                    }
                    $select.append(op);
                }

                if ($select.hasClass('ui-selectpicker')) {
                    $select.removeClass('ui-selectpicker').addClass('selectpicker');
                    $select.selectpicker();
                }
                else if ($select.is('.selectpicker'))
                    $select.selectpicker('refresh'); 

                return serverData;
            })
            .catch(function (error) {

                $select.empty();
            });

        return promise;
    }

    function SelectComponent() {

        this.clients = function (options) {

            var $select = $(options.selectSelector);

            var promise = new Promise(function (resolve, reject) {

                app.services.Clients.getInstance()
                    .getAll()
                    .then(function (clients) {

                        var client;
                        var selected = options.selected !== undefined ? options.selected : '';                        

                        clients.unshift({ id: '#new', name: 'CADASTRAR CLIENTE' });
                        clients.unshift({ id: '', name: 'Selecionar...' });

                        for (var i = 0; i < clients.length; i++) {
                            client = clients[i];
                            $select.append(new Option(client.name, client.id, selected === client.id, selected === client.id));
                        }

                        if ($select.hasClass('ui-selectpicker')) {
                            $select.removeClass('ui-selectpicker').addClass('selectpicker');
                            $select.selectpicker();
                        }

                        $select.change(function (e) { 

                            if ($select.val() === '#new') {
                               
                                $select.selectpicker('val', '');

                                clientEditor = new app.ClientEditor({
                                    modal: true,
                                    onSave: function (client) {

                                        $select.append(new Option(client.name, client.id, true, true));
                                        $select.selectpicker('refresh');

                                        if (typeof options.onNewData === 'function')
                                            options.onNewData.call($select, client);

                                        return true;
                                    }
                                });
                            }
                        });


                        resolve(clients);

                    }).catch(function (error) {

                        app.notify.error('Falha ao carregar clientes');
                        reject(error);
                    });
            });            

            return promise;
        };

        this.services = function (options) {

            options._getData = app.services.Services.getInstance().getAll;
            options._newOptionText = 'CADASTRAR SERVIÇO';
            options._dataMap = function (item) { return { id: item.id, text: item.name }; };
            options._callEditor = function ($select) {
                
                serviceEditor = new app.ServiceEditor({
                    modal: true,
                    onSave: function (service) {

                        $select.append(new Option(service.name, service.id, true, true));
                        $select.selectpicker('refresh');

                        if (typeof options.onNewData === 'function')
                            options.onNewData.call($select, service);

                        return true;
                    }
                });
            };

            return getSelectData(options);
        };

        this.vehicles = function (options) {

            options._getData = app.services.Vehicles.getInstance().getAll;
            options._getDataParams = { clientId: options.clientId };
            options._newOptionText = 'CADASTRAR VEÍCULO';
            options._dataMap = function (item) { return { id: item.id, text: item.model }; };
            options._callEditor = function ($select) {
              
                vehicleEditor = new app.VehicleEditor({
                    modal: true,
                    data: {
                        clientId: options.clientId
                    },
                    onSave: function (vehicle) {

                        $select.append(new Option(vehicle.model, vehicle.id, true, true));
                        $select.selectpicker('refresh');

                        if (typeof options.onNewData === 'function')
                            options.onNewData.call($select, vehicle);

                        return true;
                    }
                });
            };

            return getSelectData(options);
        };


        this.countys = function (options) {

            options._getData = function () {

                var url = app.services.WebApi.getBaseUri() + 'address/countys';
                var promise = app.utils.fetch.build(url, {
                    method: 'GET'
                });

                return promise;
            };

            options._dataMap = function (x) { return { id: x.id, text: x.name, data: { initials: x.initials } }; };

            return getSelectDataSimple(options);            
        };

        this.citys = function (options) {

            options._getData = function () {

                var url = app.services.WebApi.getBaseUri() + 'address/county/' + options.countyId + '/citys';
                var promise = app.utils.fetch.build(url, {
                    method: 'GET'
                });

                return promise;
            };

            options._dataMap = function (x) { return { id: x.id, text: x.name }; };

            return getSelectDataSimple(options);
        };

        this.districts = function (options) {

            options._getData = function () {

                var url = app.services.WebApi.getBaseUri() + 'address/city/' + options.cityId + '/districts';
                var promise = app.utils.fetch.build(url, {
                    method: 'GET'
                });

                return promise;
            };

            options._dataMap = function (x) { return { id: x.id, text: x.name }; };

            return getSelectDataSimple(options);
        };


        this.addressUI = function (context) {

            context = context || document;

            var $selectCounty = $('select[data-county]', context);
            var $selectCity = $('select[data-city]', context);
            var $selectDistrict = $('select[data-district]', context);

            var $inputZipcode = $('[data-zipcode]', context);
            var $inputStreet = $('[data-street]', context);
            var $inputCountyName = $('input[data-countyName]', context);
            var $inputCountyInitials = $('input[data-countyInitials]', context);
            var $inputCityName = $('input[data-cityName]', context);
            var $inputDstrictName = $('input[data-districtName]', context);

            var $btnSearchZipcode = $('[data-search-zipcode]', context);

            $btnSearchZipcode.click(function (e) {
               
                var zipcode = $inputZipcode.cleanVal();

                if (zipcode === '') {
                    $inputZipcode.focus();
                    app.notify.warn('Informe o CEP que deseja pesquisar');
                    return;
                }

                var $loadContainer = $inputZipcode.closest('.address-content');
                var url = app.services.WebApi.getBaseUri() + 'address/zipcode/' + zipcode;
                var promise = app.utils.fetch.build(url, {
                    method: 'GET'
                });
               
                promise = app.utils.fetch.parseResponse(promise);

                $loadContainer.length > 0 && $loadContainer.addClass('loading');
                promise
                    .then(function (address) {

                        var op;

                        $inputStreet.val(address.street);

                        if ($selectCounty.find('option[value="' + address.countyId + '"]').length === 0) {
                            op = new Option(address.countyName, address.countyId, true, true);
                            op.data('initials', address.countyInitials);
                            $selectCounty.append(op);
                        }
                        $selectCounty.val(address.countyId);
                        $selectCounty.selectpicker('refresh');
                        $inputCountyName.val(address.countyName);
                        $inputCountyInitials.val(address.countyInitials);

                        $selectCity.empty();                        
                        op = new Option(address.cityName, address.cityId, true, true);
                        $selectCity.append(op);                        
                        //$selectCity.val(address.cityId);
                        $selectCity.selectpicker('refresh');
                        $inputCityName.val(address.cityName);

                        $selectDistrict.empty();
                        op = new Option(address.districtName, address.districtId, true, true);
                        $selectDistrict.append(op);
                        //$selectDistrict.val(address.districtId);
                        $selectDistrict.selectpicker('refresh');
                        $inputDstrictName.val(address.districtName);

                        $loadContainer.length > 0 && $loadContainer.removeClass('loading');                       
                    })
                    .catch(function (error) {

                        if (error.title === undefined)
                            app.notify.error('Ops, falha inesperada. Contacte o suporte técnico. ' + (error.message || ''));
                        else
                            app.notify.error(error.title);

                        $loadContainer.length > 0 && $loadContainer.removeClass('loading');
                    });
            });

            $selectCounty.change(function (e) {

                var countyId = $(e.target).val();
                var $option = $(e.target).find(':selected');
                $inputCountyName.val($option.text());
                $inputCountyInitials.val($option.data('initials'));

                app.ui.select.citys({
                    countyId: countyId,
                    selectSelector: $selectCity
                })
                    .then(function () {

                        $inputCityName.val($(e.target).find(':selected').text());
                        $selectCity.change();
                    });

            });

            $selectCity.change(function (e) {

                var cityId = $(e.target).val();
                $inputCityName.val($(e.target).find(':selected').text());

                app.ui.select.districts({
                    cityId: cityId,
                    selectSelector: $selectDistrict
                })
                    .then(function () {

                        $inputDstrictName.val($(e.target).find(':selected').text());
                    });
            });

            $selectDistrict.change(function (e) {

                $inputDstrictName.val($(e.target).find(':selected').text());
            });

            var promiseCountys = app.ui.select.countys({
                selectSelector: $selectCounty
            });

            var promiseCitys = app.ui.select.citys({
                countyId: $selectCounty.val(),
                selectSelector: $selectCity
            });

            var promiseDistricts = app.ui.select.districts({
                cityId: $selectCity.val(),
                selectSelector: $selectDistrict
            });

            var promise = Promise.all([promiseCountys, promiseCitys, promiseDistricts]);

            promise.then(function () {

                var $option = $selectCounty.find(':selected');
                $inputCountyName.val($option.text());
                $inputCountyInitials.val($option.data('initials'));

                $inputCityName.val($selectCity.find(':selected').text());

                $inputDstrictName.val($selectDistrict.find(':selected').text());

            });

            return promise;
        };
    }

    app.moduleUtils.createNamespace('app.ui').select = new SelectComponent();
})();