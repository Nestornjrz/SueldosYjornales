(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoEmpleadosCtrl', listadoEmpleadosCtrl);

    listadoEmpleadosCtrl.$inject = ['$scope', '$rootScope', '$modal', 'sYjResource'];

    function listadoEmpleadosCtrl($scope, $rootScope, $modal, sYjResource) {
        //#region SE MANEJA EL TAB 
        $scope.menu = {};
        ocultar();
        $scope.activosFn = function ($event) {
            if ($event != null) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            ocultar();
            $scope.menu.activos.class = "active";
            $scope.menu.activos.mostrar = true;
        }
        $scope.inactivosFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            $scope.menu.inactivos.class = "active";
            $scope.menu.inactivos.mostrar = true;
        }
        function ocultar() {
            $scope.menu.activos = {};
            $scope.menu.inactivos = {};
        }
        $scope.activosFn();
        //#endregion

        $scope.empleados = sYjResource.empleadosSegunUbicacionSucursal.query();
        $scope.empleadosInactivos = sYjResource.empleadosInactivosSegunUbicacionSucursal.query();

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
        $rootScope.$on('actualizarListadoEmpleados', function (event, objValRecibido) {
            $scope.empleados = sYjResource.empleadosSegunUbicacionSucursal.query();
        });
    }
})();