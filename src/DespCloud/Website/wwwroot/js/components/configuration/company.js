(function () {

    function CompanyConfigManager(options) {

        // ==== private 
        // =====================================
        var self = this;
        var $context = $(options.rootElement);
        var formValidator = null;
        var companyApi = app.services.Companies.getInstance();

        var companyObj = null;

        function initConstructor() {

            initElements();

            getCompanyData()
                .then(function () {

                    app.ui.select.addressUI($context);
                });            
        }

        function initElements() {

            var $form = $context.find('form');

            enableFormValidation($form);

            app.maskUtils.maskAll($context);            

            bindEvents();
        }

        function enableFormValidation($form) {

            formValidator = $form.validate({
                rules: {
                    name: {
                        required: true,
                        minlength: 3,
                        maxlength: 100
                    },
                    email: {
                        email: true,
                        minlength: 3,
                        maxlength: 60
                    },
                    cpfCnpj: {
                        minlength: 14,
                        maxlength: 18
                    },
                    phone: {
                        minlength: 14,
                        maxlength: 15
                    },
                    'address.number': {
                        //required: true,
                        minlength: 1,
                        maxlength: 30
                    },
                    'address.street': {
                        //required: true,
                        minlength: 2,
                        maxlength: 200
                    },
                    'address.districtId': {
                        //required: true
                    },
                    'address.cityId': {
                        //required: true
                    },
                    'address.countyId': {
                        //required: true
                    }
                }
            });
        }

        function bindEvents() {

            $context.find('form').submit(handleSaveCompany);
            $context.find('[name="setLogo"]').click(handleSetLogo);
            $context.find('[name="fileLogo"]').change(handleFileLogoSelected);
        }

        function getCompanyData() {

            return companyApi.getUserCompany()
                .then(function (company) {

                    companyObj = company;

                    app.formUtils.loadData({
                        formSelector: $context.find('form'),
                        data: company,
                        map: {
                            'address.countyId'  : [null, 'address.countyName'],
                            'address.cityId'    : [null, 'address.cityName'],
                            'address.districtId': [null, 'address.districtName']
                        }
                    });

                    $context.find('.masked').each(function (i, el) {

                        $(el).val($(el).masked());
                    });

                    loadCompanyLogo(company.id);
                })
                .catch(function (response) {

                    if (response.title === undefined) {
                        app.notify.error('Ops. Falha inesperada');
                        return;
                    }

                    if (response.status === 400 && formValidator !== null) {
                        formValidator.showErrors(response.errors);
                        app.notify.warn(response.title);
                    }
                    else {
                        app.notify.error(response.title);
                    }
                });
        }

        function loadCompanyLogo(companyId) {

            var imgElement = document.getElementById('config-companyLogo');

            var url = app.services.Companies.getBaseUri() + '/' + companyId + '/logo';

            fetch(url)
                .then(function (response) {
                    return response.blob();
                })
                .then(function (myBlob) {
                    var objectURL = URL.createObjectURL(myBlob);
                    imgElement.src = objectURL;
                });
        }

        function checkLogoDimensions(file, done) {

            var img = new Image();

            img.src = window.URL.createObjectURL(file);

            img.onload = function () {
                var width = img.naturalWidth,
                    height = img.naturalHeight;

                window.URL.revokeObjectURL(img.src);

                if (width > 100 || height > 100) {
                    done.call(self, 'A imagem deve ter a resolução máxima de 100x100');
                }
                else {
                    done.call(self, '');
                }
            };
        }

        // ==== handles 
        // =====================================
        function handleSaveCompany(e) {

            e.preventDefault();
            var formElement = e.target;

            try {
                if ($(formElement).valid() === false)
                    return;

                var formOptions = {
                    formatterValue: function (value, inputName) {

                        var $element = $context.find('[name="' + inputName + '"].masked');
                        if ($element.length > 0)
                            value = $element.cleanVal();

                        if (value === '')
                            return undefined;

                        return value;
                    }
                };

                var data = $(formElement).serializeJson(formOptions);

                $context.find('[type="submit"]').attr('disabled', 'disabled');

                companyApi.updateCompany(data)
                    .then(function (data) {

                        //app.formUtils.loadData({ formSelector: formElement, data: data });

                        app.notify.success('Dados salvo com sucesso');
                    })
                    .catch(function (response) {

                        if (response.title === undefined) {
                            app.notify.error('Ops. Falha inesperada');
                            return;
                        }

                        if (response.status === 400 && formValidator !== null) {
                            formValidator.showErrors(response.errors);
                            app.notify.warn(response.title);
                        }
                        else {
                            app.notify.error(response.title);
                        }
                    })
                    .then(function () { $context.find('[type="submit"]').attr('disabled', null); });
            }
            catch (excpt) {

                $context.find('[type="submit"]').attr('disabled', null);
                app.notify.error('Ops, falha inesperada. Contacte o suporte técnico.');
                console.log(excpt);
            }            
        }   

        function handleSetLogo(e) {

            $context.find('[name="fileLogo"]').click();
        }

        function handleFileLogoSelected(e) {

            if (e.target.files.length === 0)
                return;

            var file = e.target.files[0];

            if (file.size > 1024 * 1024 * 2) {

                app.notify.warn('O arquivo excede o tamanho máximo de 2MB');
                return;
            }            

            checkLogoDimensions(file, function (msg) {

                if (msg.length > 0) {

                    app.notify.warn(msg);
                    return;
                }

                $context.find('[name="setLogo"]').attr('disabled', 'disabled');

                companyApi.uploadLogo(file)
                    .then(function () {

                        loadCompanyLogo(companyObj.id);
                        app.notify.success('Logo alterada com sucesso');
                    })
                    .catch(function (response) {

                        if (response.title === undefined) {
                            app.notify.error('Ops. Falha inesperada');
                            return;
                        }

                        if (response.status === 400 && formValidator !== null) {
                            formValidator.showErrors(response.errors);
                            app.notify.warn(response.title);
                        }
                        else {
                            app.notify.error(response.title);
                        }
                    })
                    .then(function () { $context.find('[name="setLogo"]').attr('disabled', null); });
            });            
        }

        // ==== init 
        // =====================================
        initConstructor();
    }

    window.app.CompanyConfigManager = CompanyConfigManager;

})();