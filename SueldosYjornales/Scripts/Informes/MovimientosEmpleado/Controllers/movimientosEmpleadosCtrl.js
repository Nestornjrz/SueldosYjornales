(function () {
    'use strict';
    angular
      .module('sueldosYjornalesApp')
      .controller('movimientosEmpleadosCtrl', movimientosEmpleadosCtrl);

    movimientosEmpleadosCtrl.$inject = ['$scope', '$rootScope', 'sYjResource'];

    function movimientosEmpleadosCtrl($scope, $rootScope, sYjResource) {
        var vm = this;
        ///Listado de movimientos
        vm.movimientos = sYjResource
            .movEmpleadosDets
            .query({
                "fechaDesde": new Date(2016, 0, 1),
                "fechaHasta": new Date(2016, 11, 30),
                "empleadoID": 8
            });


        //LIstado de empleados
        vm.empleados = sYjResource
            .empleados.query();

        //Se actualiza el listado de empledos segun el empleado
        //Seleccionado
        vm.recuperarMovimientosEmpleado = function (empleadoID) {
            ///Listado de movimientos x mes
            vm.movimientosXmes = sYjResource
               .movEmpleadosDetsXmes
               .query({
                   "fechaDesde": new Date(2015, 0, 1),
                   "fechaHasta": new Date(2016, 11, 30),
                   "empleadoID": empleadoID
               });

            vm.presSimpleConCuo = sYjResource.presSimpleConCuotas
               .query({ "empleadoID": empleadoID });
        }
    }
})();