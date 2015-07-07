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
        vm.cargos = sYjResource.cargos.query();
        
        vm.guardar = function () {
            sYjResource.historicoSalarios.save(vm.historicoSalario)
          .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.historicoSalario = mensaje.objetoDto;
                      //Crear un objeto date es para evitar el error al asignar la fecha
                      //http://stackoverflow.com/questions/26782917/model-is-not-a-date-object-on-input-in-angularjs
                      vm.historicoSalario.fechaSalario = new Date(mensaje.objetoDto.fechaSalario);
                      refrescarCampoSelect("historicoSalario", vm.cargos, "cargo", "cargoID");
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

        //Funciones
        function refrescarCampoSelect(objetoPrincipal, array, nombreObjeto, campoID) {
            if (array != null) {
                for (var i = 0; i < array.length; i++) {
                    if (vm[objetoPrincipal][nombreObjeto] == null) { break; }
                    if (vm[objetoPrincipal][nombreObjeto][campoID] == null) { break; }
                    if (array[i][campoID] == vm[objetoPrincipal][nombreObjeto][campoID]) {
                        vm[objetoPrincipal][nombreObjeto] = array[i];
                        break;
                    }
                };
            }
        }
    }
})();
