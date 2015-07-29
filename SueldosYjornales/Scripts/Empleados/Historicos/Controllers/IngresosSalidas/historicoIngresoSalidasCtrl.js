(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('historicoIngresoSalidasCtrl', historicoIngresoSalidasCtrl);

    historicoIngresoSalidasCtrl.$inject = ['$rootScope', 'sYjResource'];

    function historicoIngresoSalidasCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.historicoIngresoSalida = {};

        vm.nuevoParaCargar = function () {
            vm.historicoIngresoSalida = {};
            vm.historicoIngresoSalida.empleadoID = vm.empleadoID;
        };

        vm.guardar = function () {
            sYjResource.historicoIngresoSalidas.save(vm.historicoIngresoSalida)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.historicoIngresoSalida = mensaje.objetoDto;
                      //Crear un objeto date es para evitar el error al asignar la fecha
                      //http://stackoverflow.com/questions/26782917/model-is-not-a-date-object-on-input-in-angularjs
                      vm.historicoIngresoSalida.fechaIngreso = new Date(mensaje.objetoDto.fechaIngreso);
                      if (mensaje.objetoDto.fechaSalida != null) {
                          vm.historicoIngresoSalida.fechaSalida = new Date(mensaje.objetoDto.fechaSalida);
                      }
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadohistoricoIngresoSalidas', {});
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
            vm.historicoIngresoSalida.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });

        $rootScope.$on('actualizarHistoricoIngresoSalida', function (event, objValRecibido) {
            vm.historicoIngresoSalida = objValRecibido;
            vm.historicoIngresoSalida.fechaIngreso = new Date(objValRecibido.fechaIngreso);
            if (objValRecibido.fechaSalida != null) {
                vm.historicoIngresoSalida.fechaSalida = new Date(objValRecibido.fechaSalida);
            }
        });
    }
})();
