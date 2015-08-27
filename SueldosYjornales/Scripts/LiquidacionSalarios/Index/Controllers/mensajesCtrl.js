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


        $rootScope.$on('actualizarMensajes', function (event, objValRecibido) {
            vm.mensaje = objValRecibido;
            vm.logsFn(null);
        });
    }
})();
