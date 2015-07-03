(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('navbarCtrl', navbarCtrl);

    navbarCtrl.$inject = ['$scope', '$rootScope', 'sYjResource'];

    function navbarCtrl($scope, $rootScope, sYjResource) {
        sYjResource.ubicacionSucUsuarios.get(function (respuesta) {
            $scope.ubicacionSucUsuario = respuesta.objetoDto;
        });
    }
})();
