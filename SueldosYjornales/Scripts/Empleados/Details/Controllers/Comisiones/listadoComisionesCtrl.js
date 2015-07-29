(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoComisionesCtrl', listadoComisionesCtrl);

    listadoComisionesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoComisionesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.actualizar = function (comisione) {
            $rootScope.$broadcast('actualizarComisione', comisione);
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.comisiones = sYjResource
                .comisionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadoComisiones', function (event, objValRecibido) {
            vm.comisiones = sYjResource
               .comisionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
