(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoEmpresasCtrl', listadoEmpresasCtrl);

    listadoEmpresasCtrl.$inject = ['$rootScope','sYjResource']; 

    function listadoEmpresasCtrl($rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.empresas = sYjResource.empresas.query();

        vm.eliminar = function (estancia) {

        }
        vm.actualizar = function (empresa) {
            $rootScope.$broadcast('actualizarEmpresa', empresa);
        }

        //Eventos
        $rootScope.$on("actualizarListadoEmpresas", function (event, datoRecibido) {
            vm.empresas = sYjResource.empresas.query();
        });
    }
})();
