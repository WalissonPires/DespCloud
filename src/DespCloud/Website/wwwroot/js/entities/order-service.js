(function () {

    var OrderStatus = {
        OPENED   : 0,        
        CLOSED   : 1,
        CANCELED : 2,

        0 : 'Aberta',
        1 : 'Fechada',
        2 : 'Cancelada'
    };

    var entities = app.moduleUtils.createNamespace('app.entities');
    entities.OrderStatus = OrderStatus;

})();