
(function () {

    var checkIsObjeto = function (obj) {

        return obj !== null && typeof obj === 'object';// && !(obj instanceOf Date);
    };

    var visitarObjeto = function (objeto, caminho, visitante) {

        if (objeto === null || objeto === undefined)
            return;

        if (checkIsObjeto(objeto) === false)
            return;

        caminho = (typeof caminho === 'string') ? caminho : 'raiz';
        for (var propriedade in objeto) {

            var valor = objeto[propriedade];
            var isObjeto = checkIsObjeto(valor);
            var parametros = { objeto, propriedade, valor, caminho, isObjeto };
            var parar = visitante(parametros);

            if (!parar && isObjeto) {
                var novoCaminho = caminho.length === 0 ? propriedade : caminho + '.' + propriedade;
                parar = visitarObjeto(valor, novoCaminho, visitante);
            }

            if (parar) return false;
        }

    };

    var getValor = function (objeto, campo) {

        if (objeto === null || objeto === undefined)
            return null;

        var valor = objeto;
        $.each(campo.split('.'), function (i, c) {

            valor = valor[c];
        });

        if (valor === objeto)
            return null;
        else
            return valor;
    };

    var formUtils = {};

    formUtils.send = function (formSelector, events) {

        var $form = $(formSelector);
        events = events || {};

        $.ajax({
            url: $form.attr('action'),
            method: $form.attr('method') || 'POST',
            data: $form.serialize()
        })
            .done(function (responseData) { typeof events.done === 'function' && events.done.call($form, responseData); })
            .fail(function (xhr) {

                if (xhr.status === 422) {

                    try {
                        var serverValidation = JSON.parse(xhr.responseText);

                        formUtils.showModelStateMessages(formSelector, serverValidation.details);
                        alert(serverValidation.message || 'Falha ao realizar operação');
                    }
                    catch (e) {

                        console.error('Exception', e);
                        console.error('Response', xhr);
                        alert('Falha ao interpretar resposta do servidor');
                    }

                } else {

                    typeof events.fail === 'function' && events.fail($form, xhr);
                }
            })
            .always(function () { typeof events.always === 'function' && events.always.call($form); });
    };

    formUtils.showModelStateMessages = function (formSelector, errors) {

        var $validator = $(formSelector).validate();
        $validator.showErrors(errors);
    };
    
    /**
      * 
      * @param {any} param
         {
             formSelector: '[name="formPropostaItem"]',
             raiz: '',                                               // Prefixo inicial para as propriedades
             incluir: [ 'qtd' ],                                     // Lista de propriedades
             excluir: [ 'itemId' ],                                  // Lista de propriedades
             mapa: {                                                 // Mapear nome das propriedades para nome dos input's
                 qtd: 'quantidade',                                  // Mapeamento simples:  Propriedade: 'Nome do input'		            
                 itemId: [ 'itemRefId', 'produto.descricao' ],       // Mapeamento e select vinculo:   Propriedade: [ 'Nome do input', 'Propriedade com o texto do select' ] 
                 vendId: [ null, 'produto.descricao' ]               // Select vinculo somente:   Propriedade: [ null, 'Propriedade com o texto do select' ]
             },
             dados: {
                 un: 'UN',
                 qtd: 500,
                 itemId: 'GUID',
                 vendId: 'GUID,
                 produto: {
                     id: 'GUID',
                     descricao: 'MEU PRODUTO'
                 },
                 vendedor: {
                     id: 'GUID',
                     nome: 'Vendedor'
                 }
             }
         }
      */
    formUtils.loadData = function (param) {

        if (arguments.length !== 1)
            throw '[form.carregarDados] Número de argumentos inválido';

        function getElNome(prop, semRaiz) {

            var elNome = prop;
            var propMap = config.map[prop];
            if (typeof propMap === 'string')
                elNome = propMap;
            else if (Array.isArray(propMap) && typeof propMap[0] === 'string')
                elNome = propMap[0];

            return config.root.length > 0 && semRaiz !== true ? config.root + '.' + elNome : elNome;
        }

        var config = {
            formSeletor: '',
            data: {},
            root: '',
            map: {},
            include: undefined,
            exclude: undefined
        };

        Object.assign(config, param);


        if (Array.isArray(config.include)) {

            var mapaIncluir = [];
            config.include.forEach(function (item) {
                mapaIncluir.push(getElNome(item));
            });

            config.include = mapaIncluir;
        }

        if (Array.isArray(config.exclude)) {

            var mapaExcluir = [];
            config.exclude.forEach(function (item) {
                mapaExcluir.push(getElNome(item));
            });

            config.exclude = mapaExcluir;
        }

        var mapaElementos = {};
        $(config.formSelector).find('[name]').each(function (i, el) {

            var nome = el.getAttribute('name');

            if (Array.isArray(config.include) && config.include.indexOf(nome) === -1)
                return;

            if (Array.isArray(config.exclude) && config.exclude.indexOf(nome) >= 0)
                return;

            if (mapaElementos[nome] === undefined)
                mapaElementos[nome] = [];

            mapaElementos[nome].push(el);
        });

        //visitante:  parametros = { objeto, propriedade, valor, caminho, isObjeto };
        visitarObjeto(config.data, '', function (meta) {

            if (meta.isObjeto)
                return;

            if (Array.isArray(meta.valor))
                return;

            var valor = meta.valor;

            if (valor instanceof Date)
                valor = valor.toLocaleDateString();

            if (typeof valor === 'function')
                valor = valor.call(meta.objeto);

            var prop = meta.caminho.length > 0 ? meta.caminho + '.' + meta.propriedade : meta.propriedade;
            var elNome = getElNome(prop, true);

            var elementos = mapaElementos[elNome];
            if (elementos !== undefined) {

                elementos.forEach(function (el) {

                    if (el.tagName === 'SELECT') {

                        var campoMap = config.map[prop];
                        if (Array.isArray(campoMap) && campoMap.length === 2) {

                            var campoTexto = campoMap[1];
                            var texto = campoTexto !== undefined ? getValor(config.data, campoTexto) : valor;

                            if ($(el).find(`option[value="${valor}"]`).length === 0)
                                $(el).append(`<option value="${valor}">${texto}</option>`);
                        }
                    }

                    $(el).val(valor);
                });
            }
        });
    };


    app.moduleUtils.createNamespace('app').formUtils = formUtils;
})();
