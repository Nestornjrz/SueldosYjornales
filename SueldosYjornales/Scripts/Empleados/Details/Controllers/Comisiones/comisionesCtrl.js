(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('comisionesCtrl', comisionesCtrl);

    comisionesCtrl.$inject = ['$rootScope', 'sYjResource'];

    function comisionesCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.comisione = {};
        vm.nuevoParaCargar = function () {
            vm.comisione = {};
            vm.comisione.empleadoID = vm.empleadoID;
        };

        vm.guardar = function () {
           sYjResource.comisiones.save(vm.comisione)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.comisione = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      vm.comisione.fechaComision = new Date(mensaje.objetoDto.fechaComision);
                      $rootScope.$broadcast('actualizarListadoComisiones', {});
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
            vm.comisione.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });

        $rootScope.$on('actualizarComisione', function (event, objValRecibido) {
            vm.comisione = objValRecibido;
            vm.comisione.fechaComision = new Date(objValRecibido.fechaComision);
            //vm.comisione.timezone = vm.comisione.fechaComision.getTimezoneOffset();
            //vm.comisione.toLocaleDateString = vm.comisione.fechaComision.toLocaleDateString();
            //vm.comisione.toUTCString = vm.comisione.fechaComision.toUTCString();
            //vm.comisione.fechaComision = new Date(objValRecibido.fechaComision.slice(0, 10));
        });
    }
})();
