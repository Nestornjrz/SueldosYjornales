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
    }
})();
