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

        $scope.eliminar = function (empleado) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacionEmpleado.html',
                controller: function ($scope, $modalInstance) {
                    $scope.empleado = empleado;
                    $scope.objeto = {};
                    $scope.objeto.id = empleado.empleadoID;
                    $scope.objeto.mensaje = "Se eliminara el empleado numero ";
                    $scope.ok = function () {
                        sYjResource.empleados.delete({ id: empleado.empleadoID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  $scope.empleados = sYjResource.empleados.query();
                              });
                        //$rootScope.$broadcast('actualizarTodos', {});
                    };
                    $scope.cancel = function () {
                        $modalInstance.dismiss('cancel');
                    };
                },
                size: 'sm'
            });
            modalInstance.result.then(function (selectedItem) {

            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
                $scope.empleados = sYjResource.empleados.query();
            });
        }
        $rootScope.$on('actualizarListadoEmpleados', function (event,objValRecibido) {
            $scope.empleados = sYjResource.empleados.query();
        });
    }
})();