(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('mensajesCtrl', mensajesCtrl);

    mensajesCtrl.$inject = ['$scope', '$rootScope', 'sYjResource'];

    function mensajesCtrl($scope, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
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

        vm.getClassEmpleadoDet = function (detalle) {
            if (detalle.liquidacionConcepto.liquidacionConceptoID == 5) {
                return "resaltado";
            }
        }


        $rootScope.$on('actualizarLogs', function (event, objValRecibido) {
            vm.logs = objValRecibido;
            vm.logsFn(null);
        });

        $rootScope.$on('actualizarDetalles', function (event, objValRecibido) {
            vm.movimientos = objValRecibido;
            vm.detalleEmpleadoFn(null);
        });
    }
})();
