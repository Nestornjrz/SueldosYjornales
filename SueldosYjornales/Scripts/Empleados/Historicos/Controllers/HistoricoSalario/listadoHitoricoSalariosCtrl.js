(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHitoricoSalariosCtrl', listadoHitoricoSalariosCtrl);

    listadoHitoricoSalariosCtrl.$inject = ['$rootScope', 'sYjResource'];

    function listadoHitoricoSalariosCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        
        vm.actualizar = function (historicoSalario) {
            $rootScope.$broadcast('actualizarhistoricoSalario', historicoSalario);
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.historicoSalarios = sYjResource
                .historicoSalariosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
