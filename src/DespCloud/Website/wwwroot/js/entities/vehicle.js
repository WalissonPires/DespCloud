(function () {

    var VehicleType = {
        MOTORCYCLE: 0,
        CAR: 1,
        
        0: 'Moto',
        1: 'Carro'
    };

    var entities = app.moduleUtils.createNamespace('app.entities');
    entities.VehicleType = VehicleType;

})();