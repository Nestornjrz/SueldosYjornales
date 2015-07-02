(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('usuariosCtrl', usuariosCtrl);

    usuariosCtrl.$inject = ['$rootScope', 'sYjResource'];

    function usuariosCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.usuario = {};
        vm.nuevoParaCargar = function () {
            vm.usuario = {};
        };
        vm.guardar = function () {
            sYjResource.usuarios.save(vm.usuario)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.usuario = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadoUsuarios', {});
                  } else {
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                  }
              },
               function (mensaje) {
                   vm.mensajeDelServidor = mensaje.data.mensajeDelProceso;
               }
           );
        }
        //Eventos de usuario
        $rootScope.$on('actualizarUsuario', function (event, objValRecibido) {
            vm.usuario = objValRecibido;
        });
    }
})();
