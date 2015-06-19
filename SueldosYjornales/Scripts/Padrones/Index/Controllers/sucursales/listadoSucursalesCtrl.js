(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoSucursalesCtrl', listadoSucursalesCtrl);

    listadoSucursalesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoSucursalesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;        
        sYjResource.sucursales.query(function (respuesta) {
            vm.sucursales = respuesta;  
        });



        vm.eliminar = function (sucursale) {

        }

        vm.actualizar = function (sucursale) {
            $rootScope.$broadcast('actualizarSucursale', sucursale);
        }

        //Eventos
        $rootScope.$on("actualizarListadoSucursales", function (event, datoRecibido) {
            vm.sucursales = sYjResource.sucursales.query();
        });
    }
})();
