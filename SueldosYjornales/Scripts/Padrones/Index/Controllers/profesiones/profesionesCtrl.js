﻿(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('profesionesCtrl', profesionesCtrl);

    profesionesCtrl.$inject = ['$rootScope', 'sYjResource'];

    function profesionesCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.guardar = function () {
            sYjResource.profesiones.save(vm.profesione)
           .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.profesione = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                      $rootScope.$broadcast('actualizarListadoProfesiones');
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
            vm.profesione = {};
        }

        //Captura de evento usuario
        $rootScope.$on('actualizarProfesione', function (event, objValRecibido) {
            vm.profesione = objValRecibido;
        });
    }
})();
