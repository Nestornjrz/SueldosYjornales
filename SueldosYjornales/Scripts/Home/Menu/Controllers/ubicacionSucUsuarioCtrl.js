(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('ubicacionSucUsuarioCtrl', ubicacionSucUsuarioCtrl);

    ubicacionSucUsuarioCtrl.$inject = ['$scope', '$rootScope', 'sYjResource'];

    function ubicacionSucUsuarioCtrl($scope, $rootScope, sYjResource) {
        $scope.ubicacionSucUsuario = {};
        $scope.ubicacionSucUsuario.empresa = {};
        $scope.obviarBuscarSucursalesAlCambiarEmpresa = true;
        sYjResource.empresas.query(function (resp) {
            $scope.empresas = resp;
            sYjResource.ubicacionSucUsuarios.get(function (respuesta) {
                //$scope.mensaje = respuesta; 
                $scope.obviarBuscarSucursalesAlCambiarEmpresa = true;
                $scope.ubicacionSucUsuario = respuesta.objetoDto;
                $scope.obviarBuscarSucursalesAlCambiarEmpresa = true;
                $scope.ubicacionSucUsuario.empresa = respuesta.objetoDto.sucursale.empresa;
                refrescarCampoSelect("ubicacionSucUsuario", $scope.empresas, "empresa", "empresaID");
                sYjResource.sucursalesSegunEmpresaID.query(
                   { 'empresaID': $scope.ubicacionSucUsuario.empresa.empresaID },
                   function (respuesta) {
                       $scope.sucursales = respuesta;
                       refrescarCampoSelect("ubicacionSucUsuario", $scope.sucursales, "sucursale", "sucursalID");
                   });                
            });
            
        });

        $scope.$watch('ubicacionSucUsuario.empresa', function (newValue, oldValue) {
            if ($scope.obviarBuscarSucursalesAlCambiarEmpresa) {
                $scope.obviarBuscarSucursalesAlCambiarEmpresa = false;
                return;
            }
            if (newValue !== oldValue) {
                sYjResource.sucursalesSegunEmpresaID.query(
                   { 'empresaID': $scope.ubicacionSucUsuario.empresa.empresaID },
                   function (respuesta) {
                       $scope.sucursales = respuesta;
                   });
            }
        });
        $scope.guardar = function () {
            sYjResource.ubicacionSucUsuarios.save($scope.ubicacionSucUsuario)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      //vm.nacionalidade = mensaje.objetoDto;
                      $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                  } else {
                      $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                  }
              },
            function (mensaje) {
                $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            }
          );
        }

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
