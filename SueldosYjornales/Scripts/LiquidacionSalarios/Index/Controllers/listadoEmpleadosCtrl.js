(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoEmpleadosCtrl', listadoEmpleadosCtrl);

    listadoEmpleadosCtrl.$inject = ['$scope', '$rootScope', '$modal', 'sYjResource'];

    function listadoEmpleadosCtrl($scope, $rootScope, $modal, sYjResource) {
        $scope.empleados = sYjResource.empleadosSegunUbicacionSucursal.query();

        $scope.liqui = {};
        $scope.liqui.empleadosSeleccionados = [];
        $scope.cantSeleccionados = 0;

        $scope.seleccionarTodo = function (event) {
            var cantidad = 0;
            $scope.liqui.empleadosSeleccionados = [];
            _.each($scope.empleados, function (empleado, key) {
                if (event.target.checked) {
                    cantidad++;
                    $scope.liqui.empleadosSeleccionados.push(empleado.empleadoID);
                }
                empleado.seleccionadoSn = (event.target.checked) ? true : false;
            });
            $scope.cantSeleccionados = cantidad;
        };
        $scope.seleccionIndividual = function () {
            $scope.cantSeleccionados = 0;
            var cantidad = 0;
            $scope.liqui.empleadosSeleccionados = [];
            _.each($scope.empleados, function (empleado, key) {
                cantidad++;
                if (empleado.seleccionadoSn) {
                    $scope.cantSeleccionados++;
                    $scope.liqui.empleadosSeleccionados.push(empleado.empleadoID);
                }
            });
            if (cantidad == $scope.cantSeleccionados) {
                $scope.varSeleccionarTodo = true;
            } else {
                $scope.varSeleccionarTodo = false;
            }
        };
        //Datos del formulario para mes y año
        $scope.meses = [
          { "mesID": 1, "nombreMes": "Enero" },
          { "mesID": 2, "nombreMes": "Febrero" },
          { "mesID": 3, "nombreMes": "Marzo" },
          { "mesID": 4, "nombreMes": "Abril" },
          { "mesID": 5, "nombreMes": "Mayo" },
          { "mesID": 6, "nombreMes": "Junio" },
          { "mesID": 7, "nombreMes": "Julio" },
          { "mesID": 8, "nombreMes": "Agosto" },
          { "mesID": 9, "nombreMes": "Setiembre" },
          { "mesID": 10, "nombreMes": "Octubre" },
          { "mesID": 11, "nombreMes": "Noviembre" },
          { "mesID": 12, "nombreMes": "Diciembre" }
        ];

        var fechaActual = new Date();
        var yearActual = fechaActual.getFullYear();
        var mesActual = fechaActual.getMonth();//desde 0 - 11

        $scope.years = [];
        for (var i = -1; i < 5; i++) {
            $scope.years.push(yearActual - i);
        }

    
        $scope.liqui.mes = _.findWhere($scope.meses, { "mesID": mesActual + 1 });
        $scope.liqui.year = _.find($scope.years, function (num) {
            return num == yearActual;
        });

        //Se genera la liquidacion de salarios
        $scope.guardar = function () {
            sYjResource.liquidacionSalarios.save($scope.liqui)
           .$promise.then(
            function (mensaje) {
                $scope.mensaje = mensaje;
                if (!mensaje.error) {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                } else {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                }
                $rootScope.$broadcast('actualizarMensajes',$scope.mensaje);
            },
          function (mensaje) {
              $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
          });
        }
    }
})();