(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('ubicacionSucUsuarioCtrl', ubicacionSucUsuarioCtrl);

    ubicacionSucUsuarioCtrl.$inject = ['$scope','$rootScope', 'sYjResource'];

    function ubicacionSucUsuarioCtrl($scope, $rootScope, sYjResource) {
        $scope.ubicacionSucUsuario = {};
        $scope.ubicacionSucUsuario.empresa = {};
        $scope.empresas = sYjResource.empresas.query();

        $scope.$watch('ubicacionSucUsuario.empresa', function (newValue, oldValue) {
            if (newValue !== oldValue) {
                sYjResource.sucursalesSegunEmpresaID.query(
                    { 'empresaID': $scope.ubicacionSucUsuario.empresa.empresaID },
                    function (respuesta) {
                        $scope.sucursales = respuesta;
                    });
            }
        });
    }
})();
