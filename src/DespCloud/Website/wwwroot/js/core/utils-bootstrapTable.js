(function () {

    var defaultOptions = {
        striped: true,
        rememberOrder: true,
        pagination: true,
        search: true,
        showColumns: true,
        showRefresh: true,
        showToggle: true,
        clickToSelect: true,
        toolbar: undefined,
        url: undefined,
        columns: undefined,
        queryParams: function (params) {

            return {
                limit: params.limit,
                page: params.offset,
                search: params.search,
                sort: params.sort,
                order: params.order
            };
        }
    };

    var bootstrapTableUtils = {};


    bootstrapTableUtils.dateFormat = function (value, row, index) {

        var date = new Date(value);

        if (isNaN(date))
            return value;

        return `${(date.getDate() + '').padStart('0', 2)}/${((date.getMonth() + 1) + '').padStart('0', 2)}/${date.getFullYear() + ''}`;
    };

    bootstrapTableUtils.addressFormat = function (value, row, index) {

        if (row.address !== null && row.address !== undefined) {

            var ad = row.address;

            return `${ad.street}, ${ad.number}, ${ad.districtName} - ${ad.cityName}/${ad.countyInitials}`;
        }

        return null;
    };

    bootstrapTableUtils.idFormat = function (value, row, index) {

        return (value + '').padStart(6, '0');
    };    

    bootstrapTableUtils.currency = function (value, row, index) {

        return 'R$ ' + value.toFixed(2);
    };    


    bootstrapTableUtils.mergeDefaultOptions = function (customOptions) {

        var newOptions = {};

        Object.assign(newOptions, defaultOptions, customOptions);

        return newOptions;
    };

    bootstrapTableUtils.getDataById = function ($table, id) {

        return $table.bootstrapTable('getRowByUniqueId', id);
    };

    bootstrapTableUtils.getSelected = function ($table) {

        return $table.bootstrapTable('getAllSelections');
    };

    bootstrapTableUtils.insertData = function ($table, data, index) {

        $table.bootstrapTable('insertRow', { index: index || 0, row: data });
    };

    bootstrapTableUtils.updateData = function ($table, data, id) {

        $table.bootstrapTable('updateByUniqueId', { id: id, row: data });
    };

    bootstrapTableUtils.removeData = function ($table, id) {

        $table.bootstrapTable('removeByUniqueId', id);
    };
    

    app.bootstrapTableUtils = bootstrapTableUtils;
})();
