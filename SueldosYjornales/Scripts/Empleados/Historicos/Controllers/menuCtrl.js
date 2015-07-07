(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$rootScope'];

    function menuCtrl($rootScope) {
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
    }
})();
