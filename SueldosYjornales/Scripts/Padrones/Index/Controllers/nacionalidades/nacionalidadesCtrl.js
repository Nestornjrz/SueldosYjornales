(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('nacionalidadesCtrl', nacionalidadesCtrl);

    nacionalidadesCtrl.$inject = ['$rootScope', 'sYjResource'];

    function nacionalidadesCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.guardar = function () {
            sYjResource.nacionalidades.save(vm.nacionalidade)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.nacionalidade = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;                     
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
            vm.nacionalidade = {};
        }
    }
})();
