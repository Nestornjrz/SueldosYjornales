(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('cargosCtrl', cargosCtrl);

    cargosCtrl.$inject = ['$rootScope', 'sYjResource'];

    function cargosCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.cargo = {};
        vm.nuevoParaCargar = function () {
            vm.cargo = {};
        };

        vm.guardar = function () {
            sYjResource.cargos.save(vm.cargo)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.cargo = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadoCargos', {});
                  } else {
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                  }
              },
            function (mensaje) {
                vm.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            }
          );
        }
    }
})();
