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


        //Capturando eventos de usuario
        $rootScope.$on('actualizarListadoProfesiones', function (event, objValRecibido) {
            vm.profesiones = sYjResource.profesiones.query();
        });
    }
})();
