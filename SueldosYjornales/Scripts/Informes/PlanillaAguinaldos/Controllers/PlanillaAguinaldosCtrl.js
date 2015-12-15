(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('planillaAguinaldosCtrl', planillaAguinaldosCtrl);

    planillaAguinaldosCtrl.$inject = ['$scope', '$timeout', '$rootScope', 'sYjResource'];

    function planillaAguinaldosCtrl($scope, $timeout, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        $timeout(function () {
            vm.psfDto = angular.fromJson(vm.psfDto);

            sYjResource.paraPlanillaAguinaldos.save(vm.psfDto)
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
