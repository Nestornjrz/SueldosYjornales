(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('empresasCtrl', empresasCtrl);

    empresasCtrl.$inject = ['$location']; 

    function empresasCtrl($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.empresa = {};
        vm.nuevoParaCargar = function () {
            vm.empresa = {};
        };

        vm.guardar = function () {
            proveedoresResource.empresas.save(vm.empresa)
           .$promise.then(
               function (mensaje) {
                   if (!mensaje.error) {
                       vm.empresa = mensaje.objetoDto;
                       vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                       //$rootScope.$broadcast('actualizarListadoEmpresas', {});
                   } else {
                       vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                   }
               },
             function (mensaje) {
                 vm.mensajeDelServidor = mensaje.data.mensajeDelProceso;
             }
           );
        }
    }
})();
