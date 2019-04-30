(function () {

    var BASE_URI = `http://${window.location.hostname}:5011/api/`;

    var WebApi = {
        getBaseUri: function () { return BASE_URI; }
    };

    app.moduleUtils.createNamespace('app.services').WebApi = WebApi;

})();