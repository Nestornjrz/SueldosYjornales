(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('historicoIngresoSalidasCtrl', historicoIngresoSalidasCtrl);

    historicoIngresoSalidasCtrl.$inject = ['$rootScope', 'sYjResource'];

    function historicoIngresoSalidasCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.historicoIngresoSalida = {};
       
        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.historicoIngresoSalida.empleadoID = objValRecibido;
            vm.empleadoID = objValRecibido;
        });
    }
})();
