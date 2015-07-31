(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('liquidacionSalarioCtrl', liquidacionSalarioCtrl);

    liquidacionSalarioCtrl.$inject = ['$rootScope', 'sYjResource'];

    function liquidacionSalarioCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.empresas = sYjResource.empresas.query();

        vm.meses = [
            { "mesID": 1, "nombreMes": "Enero" },
            { "mesID": 2, "nombreMes": "Febrero" },
            { "mesID": 3, "nombreMes": "Marzo" },
            { "mesID": 4, "nombreMes": "Abril" },
            { "mesID": 5, "nombreMes": "Mayo" },
            { "mesID": 6, "nombreMes": "Junio" },
            { "mesID": 7, "nombreMes": "Julio" },
            { "mesID": 8, "nombreMes": "Agosto" },
            { "mesID": 9, "nombreMes": "Setiembre" },
            { "mesID": 10, "nombreMes": "Octubre" },
            { "mesID": 11, "nombreMes": "Noviembre" },
            { "mesID": 12, "nombreMes": "Diciembre" }
        ];

        var fechaActual = new Date();
        var yearActual = fechaActual.getFullYear();
        var mesActual = fechaActual.getMonth();//desde 0 - 11

        vm.years = [];
        for (var i = -1; i < 5; i++) {
            vm.years.push(yearActual - i);
        }

        vm.liquidacionSalario = {};
        vm.liquidacionSalario.mes = _.findWhere(vm.meses, { "mesID": mesActual + 1  });
        vm.liquidacionSalario.year = _.find(vm.years, function (num) {
            return num == yearActual;
        });

        vm.nuevoParaCargar = function () {
            vm.liquidacionSalario = {};
        }
    }
})();
