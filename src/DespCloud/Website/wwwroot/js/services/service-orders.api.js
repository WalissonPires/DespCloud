(function () {
   
    // ===== const
    // ====================================
    var BASE_URI = app.services.WebApi.getBaseUri() + 'ServiceOrders';
    var BASE_URL_REPORT = app.services.WebApi.getBaseUri() + 'Reports';
    var instance = null;

    function ServiceOrdersApi() {

        var self = this;

        // ===== public
        // ==============================      
        this.getById = function (id) {

            var url = BASE_URI + '/' + id;
            var promise = app.utils.fetch.build(url);

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };


        this.getAll = function (filterParams) {

            var promise = app.utils.fetch.build(BASE_URI, {
                queryParams: filterParams
            });

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };

        this.createOrder = function (orderService) {          

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'POST',
                body: JSON.stringify(orderService)
            });

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };

        this.updateOrder = function (orderService) {

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'PUT',
                body: JSON.stringify(orderService)
            });

            promise = app.utils.fetch.parseResponse(promise);

            return promise;
        };

        this.printReceipt = function (id) {

            var url = BASE_URL_REPORT + '/receipt?orderid=' + id;
            window.open(url, '_blank');
        };
    }

    ServiceOrdersApi.getSearchUrl = function () {

        return BASE_URI;
    };

    ServiceOrdersApi.getInstance = function () {

        if (instance === null)
            instance = new ServiceOrdersApi();

        return instance;
    };

    app.moduleUtils.createNamespace('app.services').ServiceOrders = ServiceOrdersApi;
})();