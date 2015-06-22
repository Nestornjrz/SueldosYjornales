(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('nacionalidadesCtrl', nacionalidadesCtrl);

    nacionalidadesCtrl.$inject = ['$rootScope', 'sYjResource'];

    function nacionalidadesCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.nacionalidade = {};
        vm.nacionalidade.cargarSn = true;

        vm.guardar = function () {
            sYjResource.nacionalidades.save(vm.nacionalidade)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.nacionalidade = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      vm.nacionalidade.cargarSn = false;
                      $rootScope.$broadcast('actualizarListadoNacionalidad', {});
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
            vm.nacionalidade.cargarSn = true;
        }
        //Captura de eventos
        $rootScope.$on('actualizarNacionalidade', function (event, objetoRecibido) {
            vm.nacionalidade = objetoRecibido;
            vm.nacionalidade.cargarSn = false;
        })
    }
})();
