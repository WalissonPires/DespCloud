(function () {

    var moduleUtils = {};

    moduleUtils.createNamespace = function (namespace) {

        var names = namespace.split('.');
        var root = window;

        for (var i = 0, len = names.length; i < len; i++) {

            if (root[names[i]] === undefined)
                root[names[i]] = {};

            root = root[names[i]];
        }

        return root;
    };

    window.app = window.app || {};
    window.app.moduleUtils = moduleUtils;

})();