(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHistoricoDireccionesCtrl', listadoHistoricoDireccionesCtrl);

    listadoHistoricoDireccionesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoHistoricoDireccionesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;


        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.historicoDirecciones = sYjResource
                .historicoDireccionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadohistoricoDirecciones', function (event,valor) {
            vm.historicoDirecciones = sYjResource
              .historicoDireccionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        vm.actualizar = function (historicoDireccion) {
            $rootScope.$broadcast('actualizarHistoricoDireccion', historicoDireccion);
        }
    }
})();
