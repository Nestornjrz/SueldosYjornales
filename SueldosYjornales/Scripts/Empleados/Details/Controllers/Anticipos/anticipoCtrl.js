(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('anticipoCtrl', anticipoCtrl);

    anticipoCtrl.$inject = ['$rootScope', 'sYjResource'];

    function anticipoCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.anticipo = {};
        vm.nuevoParaCargar = function () {
            vm.anticipo = {};
            vm.anticipo.empleadoID = vm.empleadoID;
        };

        vm.guardar = function () {
            sYjResource.anticipos.save(vm.anticipo)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.anticipo = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      vm.anticipo.fechaAnticipo = new Date(mensaje.objetoDto.fechaAnticipo);
                      $rootScope.$broadcast('actualizarListadoAnticipos', {});
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
            vm.anticipo.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });

        $rootScope.$on('actualizarAnticipo', function (event, objValRecibido) {
            vm.anticipo = objValRecibido;
            vm.anticipo.fechaAnticipo = new Date(objValRecibido.fechaAnticipo);
        });
    }
})();
