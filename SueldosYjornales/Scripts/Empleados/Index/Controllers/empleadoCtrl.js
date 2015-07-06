(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('empleadoCtrl', empleadoCtrl);

    empleadoCtrl.$inject = ['$scope', '$rootScope', '$modal', 'sYjResource'];

    function empleadoCtrl($scope, $rootScope, $modal, sYjResource) {
        $scope.sexos = [
            { sexoID: 1, nombreSexo: "Masculino" },
            { sexoID: 2, nombreSexo: "Femenino" }
        ];
        $scope.estadosCiviles = sYjResource.estadosCiviles.query();
        $scope.nacionalidades = sYjResource.nacionalidades.query();
        $scope.profesiones = sYjResource.profesiones.query();
        $scope.empleado = {};
        $scope.nuevoParaCargar = function () {
            $scope.empleado = {};
        };
        $scope.guardar = function () {
            sYjResource.empleados.save($scope.empleado)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      $scope.empleado = mensaje.objetoDto;
                      //Crear un objeto date es para evitar el error al asignar la fecha
                      //http://stackoverflow.com/questions/26782917/model-is-not-a-date-object-on-input-in-angularjs
                      $scope.empleado.fechaNacimiento = new Date(mensaje.objetoDto.fechaNacimiento);

                      $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadoEmpleados', {});
                  } else {
                      $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                  }
              },
            function (mensaje) {
                $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            }
          );
        }

        //Captura del evento
        $rootScope.$on('actualizarEmpleado', function (event, objValRecibido) {
            $scope.empleado = objValRecibido;
            $scope.empleado.fechaNacimiento = new Date(objValRecibido.fechaNacimiento);
            refrescarCampoSelect("empleado", $scope.sexos, "sexo", "sexoID");
            refrescarCampoSelect("empleado", $scope.estadosCiviles, "estadoCivile", "estadoCivilID");
            refrescarCampoSelect("empleado", $scope.nacionalidades, "nacionalidade", "nacionalidadID");
            refrescarCampoSelect("empleado", $scope.profesiones, "profesione", "profesionID");
        });

        //Funciones
        function refrescarCampoSelect(objetoPrincipal, array, nombreObjeto, campoID) {
            if (array != null) {
                for (var i = 0; i < array.length; i++) {
                    if ($scope[objetoPrincipal][nombreObjeto] == null) { break; }
                    if ($scope[objetoPrincipal][nombreObjeto][campoID] == null) { break; }
                    if (array[i][campoID] == $scope[objetoPrincipal][nombreObjeto][campoID]) {
                        $scope[objetoPrincipal][nombreObjeto] = array[i];
                        break;
                    }
                };
            }
        }
    }
})();
