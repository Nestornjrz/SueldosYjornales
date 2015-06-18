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
                      //$rootScope.$broadcast('actualizarListadoSucursales', {});
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
            vm.sucursal = {};
        }

    }
})();
