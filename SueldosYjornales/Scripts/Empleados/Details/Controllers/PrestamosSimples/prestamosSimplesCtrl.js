(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('prestamosSimplesCtrl', prestamosSimplesCtrl);

    prestamosSimplesCtrl.$inject = ['$rootScope', 'sYjResource'];

    function prestamosSimplesCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.prestamosSimple = {};

        vm.nuevoParaCargar = function () {
            vm.prestamosSimple = {};
            vm.prestamosSimple.empleadoID = vm.empleadoID;
        };

        vm.guardar = function () {
            sYjResource.prestamosSimples.save(vm.prestamosSimple)
           .$promise.then(
               function (mensaje) {
                   if (!mensaje.error) {
                       vm.prestamosSimple = mensaje.objetoDto;
                       vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                       vm.prestamosSimple.fecha1erVencimiento = new Date(mensaje.objetoDto.fecha1erVencimiento);
                       $rootScope.$broadcast('actualizarListadoPrestamos', {});
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
            vm.prestamosSimple.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });

        $rootScope.$on('actualizarPrestamoSimple', function (event, objValRecibido) {
            vm.prestamosSimple = objValRecibido;
            vm.prestamosSimple.fechaPrestamo = new Date(objValRecibido.fechaPrestamo);
        });
    }
})();
