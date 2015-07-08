(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHistoricoIngresoSalidasCtrl', listadoHistoricoIngresoSalidasCtrl);

    listadoHistoricoIngresoSalidasCtrl.$inject = ['$rootScope', 'sYjResource'];

    function listadoHistoricoIngresoSalidasCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        
        vm.actualizar = function (historicoIngresoSalida) {
            $rootScope.$broadcast('actualizarHistoricoIngresoSalida', historicoIngresoSalida);
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.historicoIngresoSalidas = sYjResource
                .historicoIngresoSalidasByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadohistoricoIngresoSalidas', function (event, objValRec) {
            vm.historicoIngresoSalidas = sYjResource
                .historicoIngresoSalidasByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
