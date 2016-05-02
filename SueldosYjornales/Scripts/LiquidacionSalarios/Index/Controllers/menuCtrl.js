(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$scope', '$rootScope', '$uibModal', 'sYjResource'];

    function menuCtrl($scope, $rootScope, $uibModal, sYjResource) {
        var vm = this;      
        
    }
})();
