(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$rootScope', '$timeout', 'sYjResource'];

    function menuCtrl($rootScope, $timeout, sYjResource) {
        var vm = this;
        vm.menu = {};
        vm.menu.introduccion = true;
        $('#detallesEmpleado').addClass('active');
        $('#detallesEmpleado a').attr('href', '#');


        vm.comisionesFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.comisiones.class = "active";
            vm.menu.comisiones.mostrar = true;
        }
        vm.anticiposFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.anticipos.class = "active";
            vm.menu.anticipos.mostrar = true;
        }
        vm.prestamosSimplesFn = function ($event) {
            if ($event != null) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            ocultar();
            vm.menu.prestamosSimples.class = "active";
            vm.menu.prestamosSimples.mostrar = true;
        }

        //vm.prestamosSimplesFn();

        //////
        function ocultar() {
            vm.menu.introduccion = false;
            vm.menu.comisiones = {};
            vm.menu.anticipos = {};
            vm.menu.prestamosSimples = {};
        }

        //Eventos
        $timeout(function () {
            $rootScope.$broadcast('empleadoID', vm.empleadoID);
            vm.empleado = sYjResource.empleados.get({ id: vm.empleadoID });
        });
    }
})();
