(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('menuCtrl', menuCtrl);

    menuCtrl.$inject = ['$scope', '$rootScope', '$modal', 'sYjResource'];

    function menuCtrl($scope, $rootScope, $modal, sYjResource) {
        var vm = this;
        
    }
})();
