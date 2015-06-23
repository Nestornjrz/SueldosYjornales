(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('sucursalesCtrl', sucursalesCtrl);

    sucursalesCtrl.$inject = ['$rootScope', 'sYjResource'];

    function sucursalesCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.empresas = sYjResource.empresas.query();

        vm.guardar = function () {
            sYjResource.sucursales.save(vm.sucursale)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.sucursale = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      refrescarCampoSelect("sucursale", vm.empresas, "empresa", "empresaID");
                      $rootScope.$broadcast('actualizarListadoSucursales', {});
                  } else {
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                  }
              },
            function (mensaje) {
                vm.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            }
          );
        }

        vm.nuevoParaCargar = function () {
            vm.sucursale = {};
        }

        //Eventos
        $rootScope.$on("actualizarSucursale", function (event, datoRecibido) {
            vm.sucursale = datoRecibido;
            refrescarCampoSelect("sucursale", vm.empresas, "empresa", "empresaID");
        });
        $rootScope.$on("actualizarEmpresas", function (event, datoRecibido) {
            vm.empresas = sYjResource.empresas.query();
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
