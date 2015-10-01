(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('planillaSalariosCtrl', planillaSalariosCtrl);

    planillaSalariosCtrl.$inject = ['$scope', '$timeout', '$rootScope', 'sYjResource'];

    function planillaSalariosCtrl($scope, $timeout, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        //vm.psfDto = null;
        $timeout(function () {
            vm.psfDto = angular.fromJson(vm.psfDto);

            sYjResource.liquidacionSalariosParaPlanillaSueldos.save(vm.psfDto)
           .$promise.then(
              function (mensaje) {
                  if (!mensaje.error) {
                      vm.listLiquidaciones = mensaje.objetoDto;
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
