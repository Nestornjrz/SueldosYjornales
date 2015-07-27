(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('historicoTelefonosCtrl', historicoTelefonosCtrl);

    historicoTelefonosCtrl.$inject = ['$rootScope', 'sYjResource'];

    function historicoTelefonosCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.historicoTelefono = {};
        vm.nuevoParaCargar = function () {
            vm.historicoTelefono = {};
            vm.historicoTelefono.empleadoID = vm.empleadoID;
        };

        vm.guardar = function () {
            sYjResource.historicoTelefonos.save(vm.historicoTelefono)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.historicoTelefono = mensaje.objetoDto;                     
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadoHistoricoTelefonos', {});
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
            vm.historicoTelefono.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });

        $rootScope.$on('actualizarhistoricoTelefono', function (event, objValRecibido) {
            vm.historicoTelefono = objValRecibido;
        });        
    }
})();
