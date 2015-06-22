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

        vm.eliminar = function () {

        }
        vm.actualizar = function (cargo) {
            $rootScope.$broadcast('actualizarCargo', cargo);
        }
        //Captura de eventos
        $rootScope.$on('actualizarListadoCargos', function (evento, datorecibido) {
            vm.cargos = sYjResource.cargos.query();
        });
    }
})();
