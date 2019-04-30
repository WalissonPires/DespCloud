(function () {

    // ===== const
    // ====================================
    var BASE_URI = app.services.WebApi.getBaseUri() + 'Clients';
    var instance = null;

    function ClientsApi() {

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

        this.createClient = function (client) {

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'POST',
                body: JSON.stringify(client)
            });

            promise = app.utils.fetch.parseResponse(promise);

            return promise;
        };

        this.updateClient = function (client) {

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'PUT',
                body: JSON.stringify(client)
            });

            promise = app.utils.fetch.parseResponse(promise);

            return promise;
        };

        this.deleteClient = function (id) {

            var url = BASE_URI + '/' + id;

            var promise = app.utils.fetch.build(url, {
                method: 'DELETE'
            });

            promise = app.utils.fetch.parseResponse(promise);

            return promise;
        };
    }

    ClientsApi.getSearchUrl = function () {

        return BASE_URI;
    };

    ClientsApi.getInstance = function () {

        if (instance === null)
            instance = new ClientsApi();

        return instance;
    };

    app.moduleUtils.createNamespace('app.services').Clients = ClientsApi;
})();