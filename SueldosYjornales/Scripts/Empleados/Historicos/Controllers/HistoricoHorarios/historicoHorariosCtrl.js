﻿(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('historicoHorariosCtrl', historicoHorariosCtrl);

    historicoHorariosCtrl.$inject = ['$rootScope', 'sYjResource'];

    function historicoHorariosCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;

        vm.historicoHorario = {};

        vm.guardar = function () {
            sYjResource.historicoHorarios.save(vm.historicoHorario)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.historicoHorarios = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadoHistoricoHorarios', {});
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
            vm.historicoHorario.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });
    }
})();