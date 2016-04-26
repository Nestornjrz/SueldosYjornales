(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('ControlVisoresCtrl', ControlVisoresCtrl);

    ControlVisoresCtrl.$inject = ['$scope', '$uibModal', '$rootScope', 'sYjResource'];

    function ControlVisoresCtrl($scope, $uibModal, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.menu = {};
        vm.liquidacionFn = function ($event) {
            if ($event != null) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            ocultar();
            vm.menu.liquidacion.class = "active";
            vm.menu.liquidacion.mostrar = true;
        }
        vm.aguinaldoFn = function ($event) {
            if ($event != null) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            ocultar();
            vm.menu.aguinaldo.class = "active";
            vm.menu.aguinaldo.mostrar = true;
        }
        function ocultar() {
            //vm.menu.introduccion = false;
            vm.menu.liquidacion = {};
            vm.menu.aguinaldo = {};
        }
        //Se llama al tab por defecto
        vm.liquidacionFn(null);
        //Eventos
        $rootScope.$on('elegirTabAguinaldo', function (event, objValRecibido) {
            vm.aguinaldoFn(null);
        });
        $rootScope.$on('elegirTabSueldo', function (event, objValRecibido) {
            vm.liquidacionFn(null);
        });
    }
})();
