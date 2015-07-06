(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('empleadoCtrl', empleadoCtrl);

    empleadoCtrl.$inject = ['$scope','$rootScope', '$modal', 'sYjResource'];

    function empleadoCtrl($scope, $rootScope, $modal, sYjResource) {
        $scope.sexos = [
            { id: 1, sexo: "Masculino" },
            { id: 2, sexo: "Femenino" }
        ];
        $scope.estadosCiviles = sYjResource.estadosCiviles.query();
        $scope.nacionalidades = sYjResource.nacionalidades.query();
        $scope.profesiones = sYjResource.profesiones.query();
    }
})();
