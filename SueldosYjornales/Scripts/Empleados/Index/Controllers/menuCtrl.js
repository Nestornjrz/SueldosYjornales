(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$scope', '$rootScope', '$uibModal', 'sYjResource'];

    function menuCtrl($scope, $rootScope, $uibModal, sYjResource) {
        $scope.mostrarFormCargaEmpleado = false;

        $scope.mostrarFormCargarEmpleado = function () {
            $('#div_formCargarEmpleado').removeClass('col-md-1');
            $('#div_formCargarEmpleado').addClass('col-md-5');

            $('#div_ListadoEmpleados').removeClass('col-md-11');
            $('#div_ListadoEmpleados').addClass('col-md-6');

            $scope.mostrarFormCargaEmpleado = true;
        }
        $scope.ocultarFormCargarEmpleado = function () {
            $('#div_formCargarEmpleado').removeClass('col-md-5');
            $('#div_formCargarEmpleado').addClass('col-md-1');

            $('#div_ListadoEmpleados').removeClass('col-md-6');
            $('#div_ListadoEmpleados').addClass('col-md-11');

            $scope.mostrarFormCargaEmpleado = false;
        }
        //Captura de evento
        $rootScope.$on('actualizarEmpleado', function (event,objValRecibido) {
            $scope.mostrarFormCargarEmpleado();
        });
    }
})();
