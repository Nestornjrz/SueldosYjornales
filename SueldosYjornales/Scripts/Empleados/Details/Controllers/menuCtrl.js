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
     

        //////
        function ocultar() {
            vm.menu.introduccion = false;
            vm.menu.comisiones = {};
        }

        //Eventos
        $timeout(function () {
            $rootScope.$broadcast('empleadoID', vm.empleadoID);
            vm.empleado = sYjResource.empleados.get({ id: vm.empleadoID });
        });
    }
})();
