(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoSucursalesCtrl', listadoSucursalesCtrl);

    listadoSucursalesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoSucursalesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        traerSucursalesPorEmpresa();
        vm.eliminar = function (sucursale) {

        }

        vm.actualizar = function (sucursale) {
            $rootScope.$broadcast('actualizarSucursale', sucursale);
        }

        //Eventos
        $rootScope.$on("actualizarListadoSucursales", function (event, datoRecibido) {
            traerSucursalesPorEmpresa();
        });
        //Funciones
        function traerSucursalesPorEmpresa() {
            sYjResource.sucursales.query(function (respuesta) {
                vm.sucursales = respuesta;
                var grupoEmpresasByID = _.groupBy(vm.sucursales, function (value, index, list) {
                    return value.empresa.empresaID;
                });
                vm.grupoEmpresas = _.collect(grupoEmpresasByID, function (empresaValue, empresaKey, empresaList) {
                    var empresa = {};
                    var sucursales = [];
                    _.each(empresaValue, function (sucursalValue, sucursalKey) {
                        empresa = sucursalValue.empresa;
                        sucursales.push(sucursalValue);
                    });
                    return { 'empresa': empresa, 'sucursales': sucursales }
                });
            });
        }
    }
})();
