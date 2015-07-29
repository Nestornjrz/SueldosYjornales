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

        vm.comisionesFn = function () {
            ocultar();
            vm.menu.comisiones.class = "active";
            vm.menu.comisiones.mostrar = true;
        }
        vm.anticiposFn = function () {
            ocultar();
            vm.menu.anticipos.class = "active";
            vm.menu.anticipos.mostrar = true;
        }

        //////
        function ocultar() {
            vm.menu.introduccion = false;
            vm.menu.comisiones = {};
            vm.menu.anticipos = {};
        }

        //Eventos
        $timeout(function () {
            $rootScope.$broadcast('empleadoID', vm.empleadoID);
            vm.empleado = sYjResource.empleados.get({ id: vm.empleadoID });
        });
    }
})();
