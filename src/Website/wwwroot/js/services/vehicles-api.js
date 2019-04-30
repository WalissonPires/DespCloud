(function () {

    // ===== const
    // ====================================
    var BASE_URI = app.services.WebApi.getBaseUri() + 'Vehicles';
    var instance = null;

    function VehiclesApi() {

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

        this.createVehicle = function (vehicle) {

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'POST',
                body: JSON.stringify(vehicle)
            });

            promise = app.utils.fetch.parseResponse(promise);

            return promise;
        };

        this.updateVehicle = function (vehicle) {

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'PUT',                
                body: JSON.stringify(vehicle)
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

    VehiclesApi.getSearchUrl = function () {

        return BASE_URI;
    };

    VehiclesApi.getInstance = function () {

        if (instance === null)
            instance = new VehiclesApi();

        return instance;
    };

    app.moduleUtils.createNamespace('app.services').Vehicles = VehiclesApi;
})();