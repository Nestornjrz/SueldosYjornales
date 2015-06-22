(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoCargosCtrl', listadoCargosCtrl);

    listadoCargosCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoCargosCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.cargos = sYjResource.cargos.query();
    }
})();
