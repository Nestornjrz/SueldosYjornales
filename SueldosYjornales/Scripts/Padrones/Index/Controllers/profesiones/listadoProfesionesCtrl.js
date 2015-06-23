(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoProfesionesCtrl', listadoProfesionesCtrl);

    listadoProfesionesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoProfesionesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.profesiones = sYjResource.profesiones.query();

        vm.actualizar = function (profesione) {
            $rootScope.$broadcast('actualizarProfesione',profesione);
        }


        //Capturando eventos de usuario
        $rootScope.$on('actualizarListadoProfesiones', function (event, objValRecibido) {
            vm.profesiones = sYjResource.profesiones.query();
        });
    }
})();
