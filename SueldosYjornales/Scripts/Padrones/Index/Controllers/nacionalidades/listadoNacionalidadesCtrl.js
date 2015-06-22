(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoNacionalidadesCtrl', listadoNacionalidadesCtrl);

    listadoNacionalidadesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoNacionalidadesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.nacionalidades = sYjResource.nacionalidades.query();
        vm.actualizar = function (nacionalidade) {
            $rootScope.$broadcast('actualizarNacionalidade', nacionalidade);
        }
    }
})();
