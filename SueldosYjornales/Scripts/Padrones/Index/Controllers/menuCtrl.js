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

        vm.empresasFn = function () {
            ocultar();
            vm.menu.empresas.class = "active";
            vm.menu.empresas.mostrar = true;
        }
        vm.sucursalesFn = function () {
            ocultar();
            vm.menu.sucursales.class = "active";
            vm.menu.sucursales.mostrar = true;
            $rootScope.$broadcast('actualizarEmpresas', {});
        }
        vm.nacionalidadesFn = function () {
            ocultar();
            vm.menu.nacionalidades.class = "active";
            vm.menu.nacionalidades.mostrar = true;
        }
        vm.cargosFn = function () {
            ocultar();
            vm.menu.cargos.class = "active";
            vm.menu.cargos.mostrar = true;
        }
        vm.profesionesFn = function () {
            ocultar();
            vm.menu.profesiones.class = "active";
            vm.menu.profesiones.mostrar = true;
        }
      
        //////
        function ocultar() {
            vm.menu.introduccion = false;
            vm.menu.empresas = {};
            vm.menu.sucursales = {};
            vm.menu.nacionalidades = {};
            vm.menu.cargos = {};
            vm.menu.profesiones = {};
        }
    }
})();
