(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('historicoSalariosCtrl', historicoSalariosCtrl);

    historicoSalariosCtrl.$inject = ['$rootScope', 'sYjResource'];

    function historicoSalariosCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.historicoSalario = {};
        
        vm.guardar = function () {
            sYjResource.historicoSalarios.save(vm.historicoSalario)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.historicoSalario = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadoHistoricoSalario', {});
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
            vm.historicoSalario.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });
    }
})();
