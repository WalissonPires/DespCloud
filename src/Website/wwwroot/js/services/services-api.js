(function () {

    // ===== const
    // ====================================
    var BASE_URI = app.services.WebApi.getBaseUri() + 'Services';
    var instance = null;

    function ServicesApi() {

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

            var promise = app.utils.fetch.build(BASE_URI);

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };

        this.createService = function (service) {

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'POST',
                body: JSON.stringify(service)
            });

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };

        this.updateService = function (service) {

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'PUT',                
                body: JSON.stringify(service)
            });

            promise = app.utils.fetch.parseResponse(promise);

            return promise;
        };

        this.deleteService = function (id) {

            var url = BASE_URI + '/' + id;

            var promise = app.utils.fetch.build(url, {
                method: 'DELETE'
            });

            promise = app.utils.fetch.parseResponse(promise);

            return promise;
        };
    }

    ServicesApi.getSearchUrl = function () {

        return BASE_URI;
    };

    ServicesApi.getInstance = function () {

        if (instance === null)
            instance = new ServicesApi();

        return instance;
    };

    app.moduleUtils.createNamespace('app.services').Services = ServicesApi;
})();