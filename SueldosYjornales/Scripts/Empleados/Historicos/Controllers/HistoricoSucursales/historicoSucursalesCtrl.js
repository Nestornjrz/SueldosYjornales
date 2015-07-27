(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('historicoSucursalesCtrl', historicoSucursalesCtrl);

    historicoSucursalesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function historicoSucursalesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.historicoSucursale = {};
        vm.sucursales = sYjResource.sucursales.query();

        vm.nuevoParaCargar = function () {
            vm.historicoSucursale = {};
            vm.historicoSucursale.empleadoID = vm.empleadoID;
        };

        vm.guardar = function () {
            sYjResource.historicoSucursales.save(vm.historicoSucursale)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.historicoSucursale = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadoHistoricoSucursales', {});
                      refrescarCampoSelect("historicoSucursale", vm.sucursales, "sucursal", "sucursalID");
                  } else {
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                  }
              },
            function (mensaje) {
                vm.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            }
          );
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.historicoSucursale.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });

        $rootScope.$on('actualizarhistoricoSucursale', function (event, objValRecibido) {
            vm.historicoSucursale = objValRecibido;           
            refrescarCampoSelect("historicoSucursale", vm.sucursales, "sucursal", "sucursalID");
        });
        //Funciones
        function refrescarCampoSelect(objetoPrincipal, array, nombreObjeto, campoID) {
            if (array != null) {
                for (var i = 0; i < array.length; i++) {
                    if (vm[objetoPrincipal][nombreObjeto] == null) { break; }
                    if (vm[objetoPrincipal][nombreObjeto][campoID] == null) { break; }
                    if (array[i][campoID] == vm[objetoPrincipal][nombreObjeto][campoID]) {
                        vm[objetoPrincipal][nombreObjeto] = array[i];
                        break;
                    }
                };
            }
        }

    }
})();
