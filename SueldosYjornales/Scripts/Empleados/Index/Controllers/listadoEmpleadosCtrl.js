(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoEmpleadosCtrl', listadoEmpleadosCtrl);

    listadoEmpleadosCtrl.$inject = ['$scope','$rootScope', '$modal', 'sYjResource'];

    function listadoEmpleadosCtrl($scope, $rootScope, $modal, sYjResource) {
        $scope.empleados = sYjResource.empleados.query();

        $scope.actualizar = function (empleado) {
            $rootScope.$broadcast('actualizarEmpleado', empleado);
        }
    }
})();