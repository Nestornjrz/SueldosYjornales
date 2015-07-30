(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoAnticiposCtrl', listadoAnticiposCtrl);

    listadoAnticiposCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoAnticiposCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.actualizar = function (comisione) {
            $rootScope.$broadcast('actualizarAnticipo', comisione);
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.anticipos = sYjResource
                .anticiposByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
