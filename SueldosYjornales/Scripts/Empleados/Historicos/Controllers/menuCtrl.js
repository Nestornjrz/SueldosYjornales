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

        vm.historicoDireccionesFn = function () {
            ocultar();
            vm.menu.historicoDirecciones.class = "active";
            vm.menu.historicoDirecciones.mostrar = true;
        }
        vm.salariosFn = function () {
            ocultar();
            vm.menu.salarios.class = "active";
            vm.menu.salarios.mostrar = true;
        }
        vm.ingresoSalidasFn = function () {
            ocultar();
            vm.menu.ingresoSalidas.class = "active";
            vm.menu.ingresoSalidas.mostrar = true;
        }
        vm.horariosFn = function () {
            ocultar();
            vm.menu.horarios.class = "active";
            vm.menu.horarios.mostrar = true;
        }

        //////
        function ocultar() {
            vm.menu.introduccion = false;
            vm.menu.historicoDirecciones = {};
            vm.menu.salarios = {};
            vm.menu.ingresoSalidas = {};
            vm.menu.horarios = {};
        }

        //Eventos
        $timeout(function () {
            $rootScope.$broadcast('empleadoID', vm.empleadoID);
            vm.empleado = sYjResource.empleados.get({ id: vm.empleadoID });
        });
    }
})();
