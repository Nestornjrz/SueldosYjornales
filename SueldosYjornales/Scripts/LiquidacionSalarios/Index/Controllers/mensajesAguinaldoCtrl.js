(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('mensajesAguinaldoCtrl', mensajesAguinaldoCtrl);

    mensajesAguinaldoCtrl.$inject = ['$scope', '$modal', '$rootScope', 'sYjResource'];

    function mensajesAguinaldoCtrl($scope, $modal, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        //#region MANEJO DEL MENU
        vm.menu = {};
        vm.menu.introduccion = true;
        vm.logsFn = function ($event) {
            if ($event != null) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            ocultar();
            vm.menu.logs.class = "active";
            vm.menu.logs.mostrar = true;
        }
        vm.detalleEmpleadoFn = function ($event) {
            if ($event != null) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            ocultar();
            vm.menu.detalleEmpleado.class = "active";
            vm.menu.detalleEmpleado.mostrar = true;
        }
        function ocultar() {
            vm.menu.introduccion = false;
            vm.menu.logs = {};
            vm.menu.detalleEmpleado = {};
        }
        vm.logsFn(null);
        //#endregion
        $rootScope.$on('actualizarLogsAguinaldo', function (event, objValRecibido) {
            vm.logsAguinaldo = objValRecibido;
            $rootScope.$broadcast('elegirTabAguinaldo', {});
        });
        $rootScope.$on('actualizarDetallesAguinaldos', function (event, objValRecibido) {
            vm.aguinaldosPorMes = objValRecibido;
            vm.detalleEmpleadoFn(null);
        });
        $rootScope.$on('formLiquidacionAguinaldo', function (event, objValRecibido) {
            vm.formLiquidacionSalario = objValRecibido;
            var json = angular.toJson(vm.formLiquidacionSalario);
            $('#jsonInputAguinaldo').val(json);//Esto le pasa al formulario para poder imprimir          
        });
    }
})();
