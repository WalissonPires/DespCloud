(function () {

    function FetchUtils() {

        this.parseResponse = function(promiseResponse) {

            var responseSave = null;

            var promise = promiseResponse.then(function (response) {

                responseSave = response;

                var contentType = response.headers.get("content-type");
                if (contentType === null)
                    return {};
                else if (contentType && contentType.indexOf("json") !== -1) { // application/json; application/problem+json;
                    return response.json();
                }
                else
                    throw { status: 0, title: 'Unexpected response type: ' + contentType + '. Expected type: application/json' };
            })
                .then(function (jsonData) {

                    var responseJson = { status: responseSave.status, data: jsonData };

                    var responseError = {
                        status: responseJson.status,
                        title: undefined,
                        errors: undefined,
                        data: undefined
                    };

                    try {

                        if (Math.trunc(responseJson.status / 100) === 2)
                            return responseJson.data;
                        else {

                            if (responseJson.status === 500) {

                                responseError.title = 'Error inesperado. Contacte o suporte técnico: ' + responseJson.data.title;
                            }
                            else if (responseJson.status === 400) {

                                responseError.title = 'Um ou mais erros de validação ocorreram. Verifique os dados e tente novamente.';

                                var props = Object.keys(responseJson.data.errors);
                                responseError.errors = {};

                                for (var i = 0; i < props.length; i++) {

                                    responseError.errors[props[i]] = responseJson.data.errors[props[i]][0];
                                }
                            }
                            else {
                                responseError.title = 'Falha ao interpretar resposta do servidor. Caso o problema persistir contacte o suporte técnico';
                                responseError.data = responseJson.data;
                            }
                        }
                    }
                    catch (e) {

                        responseError.title = 'Ocorreu um erro inesperado ao ler a resposta do servidor.  Caso o problema persistir contacte o suporte técnico';
                        responseError.data = e;
                    }

                    throw responseError;
                })
                .catch(function (error) {

                    if (responseSave !== null && responseSave.status === 405)
                        error = { status: 0, title: 'Operação não pode ser executada pelo servidor' };
                    else if (error.title === undefined)                         
                        error = { status: 0, title: 'Falha ao comunicar com o servidor' };                    

                    throw error;
                });

            return promise;
        };

        this.build = function (url, options) {

            // === base
            // ===============================================
            options = options || {};
            options.method = options.method || 'GET';

            if (options.queryParams !== undefined && options.queryParams !== null) {

                var strParams = '';
                for (var key in options.queryParams) {

                    value = options.queryParams[key];
                    if (Array.isArray(value)) {

                        for (var i = 0; i < value.length; i++) {
                            if (strParams.length === 0)
                                strParams += key + '=' + encodeURIComponent(value[i]);
                            else
                                strParams += '&' + key + '=' + encodeURIComponent(value[i]);
                        }
                    }
                    else {
                        if (strParams.length === 0)
                            strParams += key + '=' + encodeURIComponent(value);
                        else
                            strParams += '&' + key + '=' + encodeURIComponent(value);
                    }
                }

                if (strParams.length > 0)
                    url = url + '?' + strParams;
            }

            // === headers
            // ================================================
            options.headers = options.headers || {};
            options.headers['Accept'] = options.headers['Accept'] || 'application/json';

            if ((options.method === 'POST' || options.method === 'PUT') && options.headers['Content-Type'] === undefined)
                options.headers['Content-Type'] = 'application/json';


            return fetch(url, options);
        };
       
    }
 
    app.moduleUtils.createNamespace('app.utils').fetch = new FetchUtils();
})();