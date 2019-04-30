(function () {

    // ===== const
    // ====================================
    var BASE_URI = app.services.WebApi.getBaseUri() + 'Companies';
    var instance = null;

    function CompaniesApi() {

        var self = this;

        // ===== public
        // ==============================      
        this.getUserCompany = function () {

            var url = BASE_URI + '/this';
            var promise = app.utils.fetch.build(url);

            promise = app.utils.fetch.parseResponse(promise);
            return promise;
        };        

        this.updateCompany = function (company) {

            var promise = app.utils.fetch.build(BASE_URI, {
                method: 'PUT',
                body: JSON.stringify(company)
            });

            promise = app.utils.fetch.parseResponse(promise);

            return promise;
        };

        this.uploadLogo = function (fileLogo) {

            var formData = new FormData();
            formData.append('logo', fileLogo);

            var url = BASE_URI + '/this/logo';
            var promise = fetch(url, {
                method: 'POST',
                body: formData//fileLogo
            });

            promise = app.utils.fetch.parseResponse(promise);

            return promise;
        };
    }

    CompaniesApi.getSearchUrl = function () {

        return BASE_URI;
    };

    CompaniesApi.getBaseUri = function () {

        return BASE_URI;
    };

    CompaniesApi.getInstance = function () {

        if (instance === null)
            instance = new CompaniesApi();

        return instance;
    };

    app.moduleUtils.createNamespace('app.services').Companies = CompaniesApi;
})();