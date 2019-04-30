(function () {

    // ===== const
    // ====================================
    var BASE_URI = app.services.WebApi.getBaseUri() + 'OrderServiceItems';
    var instance = null;

    function OrderServiceItemsApi() {

        var self = this;

        // ===== public
        // ==============================        
        this.getAll = function (filterParams) {

            var promise = app.utils.fetch.build(BASE_URI, {
                queryParams: filterParams
            });

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };

        this.getById = function (id) {

            var url = BASE_URI + '/' + id;
            var promise = app.utils.fetch.build(url);
            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };


        this.createItem = function (item) {

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'POST',
                body: JSON.stringify(item)
            });

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };

        this.updateItem = function (item) {
            
            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'PUT',
                body: JSON.stringify(item)
            });

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };

        this.deleteItem = function (id) {

            var url = BASE_URI + '/' + id;
            var promise = app.utils.fetch.build(url, {
                method: 'DELETE'
            });

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };
    }

    OrderServiceItemsApi.getSearchUrl = function () {

        return BASE_URI;
    };

    OrderServiceItemsApi.getInstance = function () {

        if (instance === null)
            instance = new OrderServiceItemsApi();

        return instance;
    };

    app.moduleUtils.createNamespace('app.services').OrderServiceItems = OrderServiceItemsApi;
})();