(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('historicoDireccionCtrl', historicoDireccionCtrl);

    historicoDireccionCtrl.$inject = ['$location']; 

    function historicoDireccionCtrl($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.historicoDireccion = {};
        vm.nuevoParaCargar = function () {
            vm.historicoDireccion = {};
        };

        vm.guardar = function () {
            sYjResource.historicoDirecciones.save(vm.historicoDireccion)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.historicoDireccion = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadohistoricoDirecciones', {});
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
