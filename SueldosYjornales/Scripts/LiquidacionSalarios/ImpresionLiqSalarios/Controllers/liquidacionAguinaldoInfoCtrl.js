﻿(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('liquidacionAguinaldoInfoCtrl', liquidacionAguinaldoInfoCtrl);

    liquidacionAguinaldoInfoCtrl.$inject = ['$scope', '$timeout', '$rootScope', 'sYjResource'];

    function liquidacionAguinaldoInfoCtrl($scope, $timeout, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.repeticion = [1, 2];
        vm.flDto = null;
        $timeout(function () {
            vm.flDto = angular.fromJson(vm.flDto);
        });
        $scope.$watch('vm.flDto', function (newVal, oldVal) {
            if (vm.flDto == null) {
                return;
            }
            sYjResource.liquidacionAguinaldosParaImprimir.save(vm.flDto)
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
