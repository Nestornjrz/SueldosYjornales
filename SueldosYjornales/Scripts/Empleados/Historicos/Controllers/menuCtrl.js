(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$rootScope', '$timeout'];

    function menuCtrl($rootScope, $timeout) {
        var vm = this;
        vm.menu = {};
        vm.menu.introduccion = true;

        vm.historicoDireccionesFn = function () {
            ocultar();
            vm.menu.historicoDirecciones.class = "active";
            vm.menu.historicoDirecciones.mostrar = true;
        }

        //////
        function ocultar() {
            vm.menu.introduccion = false;
            vm.menu.historicoDirecciones = {};
        }

        //Eventos
        $timeout(function () {
            $rootScope.$broadcast('empleadoID', vm.empleadoID);
        });
    }
})();
