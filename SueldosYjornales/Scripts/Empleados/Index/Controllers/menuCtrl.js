(function () {
    'use strict';

    angular
        .module('app')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$location']; 

    function menuCtrl($location) {
        /* jshint validthis:true */
        var vm = this;      
        vm.menu = {};

        vm.empresasFn = function () {
            ocultar();
            vm.menu.empresas.class = "active";
            vm.menu.empresas.mostrar = true;
        }

        //////
        function ocultar() {          
            vm.menu.empresas = {};       
        }

    }
})();
