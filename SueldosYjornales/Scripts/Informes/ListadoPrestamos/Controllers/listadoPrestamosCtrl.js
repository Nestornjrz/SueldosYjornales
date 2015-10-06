(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoPrestamosCtrl', listadoPrestamosCtrl);

    listadoPrestamosCtrl.$inject = ['$scope', '$timeout', '$rootScope', 'sYjResource'];

    function listadoPrestamosCtrl($scope, $timeout, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        $timeout(function () {
            vm.psfDto = angular.fromJson(vm.psfDto);

            sYjResource.paraPlanillaPrestamos.save(vm.psfDto)
           .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.listado = mensaje.objetoDto;
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                  } else {
                      vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                  }
              },
              function (mensaje) {
                  vm.mensajeDelServidor = mensaje.data.mensajeDelProceso;
              }
          );
        });
    }
})();
