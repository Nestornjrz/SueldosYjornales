(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('liquidacionSalarioInfoCtrl', liquidacionSalarioInfoCtrl);

    liquidacionSalarioInfoCtrl.$inject = ['$scope', '$timeout', '$rootScope', 'sYjResource'];

    function liquidacionSalarioInfoCtrl($scope, $timeout, $rootScope, sYjResource) {
        var vm = this;
        $timeout(function () {
            vm.lsfDto = angular.fromJson(vm.lsfDto);
        });
    }
})();
