(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuMovEmpleado', menuMovEmpleado);

    menuMovEmpleado.$inject = ['$rootScope'];

    function menuMovEmpleado($rootScope) {
        var vm = this;
        vm.menu = {};      

        vm.movimientosFn = function () {
            ocultar();
            vm.menu.movimientos.class = "active";
            vm.menu.movimientos.mostrar = true;
        }
        vm.prestamosFn = function () {
            ocultar();
            vm.menu.prestamos.class = "active";
            vm.menu.prestamos.mostrar = true;            
        }
       
        //////
        function ocultar() {           
            vm.menu.movimientos = {};
            vm.menu.prestamos = {};
        }
        //Se llama a la funcion
        vm.movimientosFn();
    }
})();
