(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('historicoDireccionCtrl', historicoDireccionCtrl);

    historicoDireccionCtrl.$inject = ['$rootScope', 'sYjResource'];

    function historicoDireccionCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.historicoDireccion = {};
        vm.nuevoParaCargar = function () {
            vm.historicoDireccion = {};
            vm.historicoDireccion.empleadoID = vm.empleadoID;
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
        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.historicoDireccion.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });

        $rootScope.$on('actualizarHistoricoDireccion', function (event, objValRecibido) {
            vm.historicoDireccion = objValRecibido;
        });
    }
})();
