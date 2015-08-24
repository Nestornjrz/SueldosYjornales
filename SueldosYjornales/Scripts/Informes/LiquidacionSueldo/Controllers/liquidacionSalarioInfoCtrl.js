(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('liquidacionSalarioInfoCtrl', liquidacionSalarioInfoCtrl);

    liquidacionSalarioInfoCtrl.$inject = ['$scope', '$timeout', '$rootScope', 'sYjResource'];

    function liquidacionSalarioInfoCtrl($scope, $timeout, $rootScope, sYjResource) {
        var vm = this;
        vm.repeticion = [1,2];
        vm.lsfDto = null;
        $timeout(function () {
            vm.lsfDto = angular.fromJson(vm.lsfDto);
        });

        $scope.$watch('vm.lsfDto', function (newVal, oldVal) {
            if (vm.lsfDto == null) {
                return;
            }
            sYjResource.InfoLiqSalarios.save(vm.lsfDto)
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
