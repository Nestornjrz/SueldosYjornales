(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('historicoHorariosCtrl', historicoHorariosCtrl);

    historicoHorariosCtrl.$inject = ['$rootScope', 'sYjResource'];

    function historicoHorariosCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        //vm.hora = moment().format();


        vm.historicoHorario = {};

        vm.guardar = function () {
            sYjResource.historicoHorarios.save(vm.historicoHorario)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.historicoHorario = mensaje.objetoDto;
                      asignarObjetoDate(mensaje);
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

        $rootScope.$on('actualizarHistoricoHorario', function (event, objValRecibido) {
            vm.historicoHorario = objValRecibido;
            asignarObjetoDateAhistoricoHorario(objValRecibido);
        });

        //function
        function asignarObjetoDateAhistoricoHorario(objValRecibido) {
            if (objValRecibido.horaEntradaManana != null) {
                vm.historicoHorario.horaEntradaManana = new Date(objValRecibido.horaEntradaManana);
            }
            if (objValRecibido.horaSalidaManana != null) {
                vm.historicoHorario.horaSalidaManana = new Date(objValRecibido.horaSalidaManana);
            }
            if (objValRecibido.horaEntradaTarde != null) {
                vm.historicoHorario.horaEntradaTarde = new Date(objValRecibido.horaEntradaTarde);
            }
            if (objValRecibido.horaSalidaTarde != null) {
                vm.historicoHorario.horaSalidaTarde = new Date(objValRecibido.horaSalidaTarde);
            }
            if (objValRecibido.horaEntradaNoche != null) {
                vm.historicoHorario.horaEntradaNoche = new Date(objValRecibido.horaEntradaNoche);
            }
            if (objValRecibido.horaSalidaNoche != null) {
                vm.historicoHorario.horaSalidaNoche = new Date(objValRecibido.horaSalidaNoche);
            }
        }
        function asignarObjetoDate(mensaje) {
            if (mensaje.objetoDto.horaEntradaManana != null) {
                vm.historicoHorario.horaEntradaManana = new Date(mensaje.objetoDto.horaEntradaManana);
            }
            if (mensaje.objetoDto.horaSalidaManana != null) {
                vm.historicoHorario.horaSalidaManana = new Date(mensaje.objetoDto.horaSalidaManana);
            }
            if (mensaje.objetoDto.horaEntradaTarde != null) {
                vm.historicoHorario.horaEntradaTarde = new Date(mensaje.objetoDto.horaEntradaTarde);
            }
            if (mensaje.objetoDto.horaSalidaTarde != null) {
                vm.historicoHorario.horaSalidaTarde = new Date(mensaje.objetoDto.horaSalidaTarde);
            }
            if (mensaje.objetoDto.horaEntradaNoche != null) {
                vm.historicoHorario.horaEntradaNoche = new Date(mensaje.objetoDto.horaEntradaNoche);
            }
            if (mensaje.objetoDto.horaSalidaNoche != null) {
                vm.historicoHorario.horaSalidaNoche = new Date(mensaje.objetoDto.horaSalidaNoche);
            }

        }
    }
})();
