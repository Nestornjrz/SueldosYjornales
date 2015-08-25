(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoEmpleadosCtrl', listadoEmpleadosCtrl);

    listadoEmpleadosCtrl.$inject = ['$scope', '$rootScope', '$modal', 'sYjResource'];

    function listadoEmpleadosCtrl($scope, $rootScope, $modal, sYjResource) {
        $scope.empleados = sYjResource.empleadosSegunUbicacionSucursal.query();

        $scope.empleadosSeleccionados = [];
        $scope.cantSeleccionados = 0;

        $scope.seleccionarTodo = function (event) {
            var cantidad = 0;
            $scope.empleadosSeleccionados = [];
            _.each($scope.empleados, function (empleado, key) {
                if (event.target.checked) {
                    cantidad++;
                    $scope.empleadosSeleccionados.push(empleado.empleadoID);
                }
                empleado.seleccionadoSn = (event.target.checked) ? true : false;
            });
            $scope.cantSeleccionados = cantidad;
        };
        $scope.seleccionIndividual = function () {
            $scope.cantSeleccionados = 0;
            var cantidad = 0;
            $scope.empleadosSeleccionados = [];
            _.each($scope.empleados, function (empleado, key) {
                cantidad++;
                if (empleado.seleccionadoSn) {
                    $scope.cantSeleccionados++;
                    $scope.empleadosSeleccionados.push(empleado.empleadoID);
                }
            });
            if (cantidad == $scope.cantSeleccionados) {
                $scope.varSeleccionarTodo = true;
            } else {
                $scope.varSeleccionarTodo = false;
            }
        };

        //Se genera la liquidacion de salarios
        $scope.guardar = function () {
            sYjResource.liquidacionSalarios.save($scope.empleadosSeleccionados)
           .$promise.then(
            function (mensaje) {
                if (!mensaje.error) {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                } else {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                }
            },
          function (mensaje) {
              $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
          });
        }
    }
})();