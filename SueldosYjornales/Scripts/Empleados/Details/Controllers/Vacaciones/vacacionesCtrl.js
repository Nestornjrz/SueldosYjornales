(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('vacacionesCtrl', vacacionesCtrl);

    vacacionesCtrl.$inject = ['$rootScope', 'sYjResource'];

    function vacacionesCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.vacacione = {};
        vm.nuevoParaCargar = function () {
            vm.vacacione = {};
            vm.vacacione.empleadoID = vm.empleadoID;
        };

        vm.guardar = function () {
           sYjResource.vacaciones.save(vm.vacacione)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.vacacione = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      vm.vacacione.fechaSalida = new Date(mensaje.objetoDto.fechaSalida);
                      $rootScope.$broadcast('actualizarListadoVacaciones', {});
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
            vm.vacacione.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });

        $rootScope.$on('actualizarVacacione', function (event, objValRecibido) {
            vm.vacacione = objValRecibido;
            vm.vacacione.fechaSalida = new Date(objValRecibido.fechaSalida);
            //vm.comisione.timezone = vm.comisione.fechaComision.getTimezoneOffset();
            //vm.comisione.toLocaleDateString = vm.comisione.fechaComision.toLocaleDateString();
            //vm.comisione.toUTCString = vm.comisione.fechaComision.toUTCString();
            //vm.comisione.fechaComision = new Date(objValRecibido.fechaComision.slice(0, 10));
        });
    }
})();
