(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHitoricoHorariosCtrl', listadoHitoricoHorariosCtrl);

    listadoHitoricoHorariosCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoHitoricoHorariosCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;

        vm.actualizar = function (historicoHorario) {
            $rootScope.$broadcast('actualizarHistoricoHorario', historicoHorario);
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.historicoHorarios = sYjResource
              .historicoHorariosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadoHistoricoHorarios', function (event, objValRecibido) {
            vm.historicoHorarios = sYjResource
              .historicoHorariosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
