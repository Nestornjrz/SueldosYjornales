(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('encabezadoCtrl', encabezadoCtrl);

    encabezadoCtrl.$inject = ['$rootScope', '$timeout', '$scope', 'sYjResource'];

    function encabezadoCtrl($rootScope, $timeout, $scope, sYjResource) {
        var vm = this;       

        //Eventos
        $timeout(function () {
            $rootScope.$broadcast('empleadoID', vm.empleadoID);
            vm.empleado = sYjResource.empleados.get({ id: vm.empleadoID });            
        });
    }
})();
