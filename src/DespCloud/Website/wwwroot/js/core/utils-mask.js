
(function () {

    var MaskUtils = {};

    MaskUtils.maskAll = function (contextSelector) {

        $('.number', contextSelector).mask('0#');
        $('.date', contextSelector).mask('00/00/0000', { placeholder: "__/__/____" });
        $('.time', contextSelector).mask('00:00:00', { placeholder: "__:__:__" });
        $('.date-time', contextSelector).mask('00/00/0000 00:00:00', { placeholder: "__/__/____ __:__:__" });
        $('.zipcode', contextSelector).mask('00000-000', { placeholder: "_____-___" });
        $('.phone', contextSelector).mask('90000-0000', { placeholder: "_____-____" });
        $('.phone-ddd', contextSelector).mask('(00) 90000-0000', { placeholder: "(__) _____-____" });
        $('.plate', contextSelector).mask('SSS-0000', { placeholder: "___-____" });
        $('.cpf', contextSelector).mask('000.000.000-00', { placeholder: "___.___.___-__", reverse: true });
        $('.cnpj', contextSelector).mask('00.000.000/0000-00', { placeholder: "__.___.___/____-__", reverse: true });
        $('.money', contextSelector).mask('000.000.000.000.000,00', { placeholder: "_,__", reverse: true });
        $('.money2', contextSelector).mask("#.##0,00", { placeholder: "_,__", reverse: true });
        $('.percent', contextSelector).mask('##0,00%', { placeholder: "_,__", reverse: true });
        $('.yearManufactureModel', contextSelector).mask('00/00', { placeholder: "__/__" });

       
        $('.cpf-cnpj', contextSelector).each(function (i, el) {

            var options = {
                placeholder: 'CPF ou CNPJ',
                onKeyPress: function (value, e, field, options) {
                    var masks = ['000.000.000-009', '00.000.000/0000-00'];
                    var mask = value.length > 14 ? masks[1] : masks[0];
                    $(field).mask(mask, options);
                }
            };

            $(el).mask('000.000.000-009', options);
        });
    };

    app.moduleUtils.createNamespace('app').maskUtils = MaskUtils;
})();