(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$rootScope', '$timeout', 'sYjResource'];

    function menuCtrl($rootScope, $timeout, sYjResource) {
        var vm = this;
        $('#resumenEmpleado').addClass('active');


        //Eventos
        $timeout(function () {
            $rootScope.$broadcast('empleadoID', vm.empleadoID);
            vm.empleado = sYjResource.empleados.get({ id: vm.empleadoID });

            vm.direccion = sYjResource.historicoDireccionesDireccionActual.get({ "empleadoID": vm.empleadoID });
            vm.horario = sYjResource.historicoHorariosUltimoHorario.get({ "empleadoID": vm.empleadoID });
            vm.ingresoSalida = sYjResource.historicoIngresoSalidasUltimoIngreso.get({ "empleadoID": vm.empleadoID });
            vm.salarioYcargo = sYjResource.historicoSalariosSalarioActual.get({ "empleadoID": vm.empleadoID });
            vm.sucursal = sYjResource.historicoSucursalesUltimoSucursales.get({ "empleadoID": vm.empleadoID });
            vm.telefono = sYjResource.historicoTelefonosUltimoTelefono.get({ "empleadoID": vm.empleadoID });
            vm.comisiones = sYjResource.comisionesUltimo2meses.query({ "empleadoID": vm.empleadoID });
            vm.anticipos = sYjResource.anticiposUltimo2Meses.query({ "empleadoID": vm.empleadoID });
        });
    }
})();
