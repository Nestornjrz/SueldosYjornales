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
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacion.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.sucursale = sucursale;
                    $scope.objeto = {};
                    $scope.objeto.id = sucursale.sucursalID;
                    $scope.objeto.mensaje = "Se eliminara la sucursal numero ";
                    $scope.ok = function () {
                        sYjResource.sucursales.delete({ id: sucursale.sucursalID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.sucursales = sYjResource.sucursales.query();
                              });

                        //$rootScope.$broadcast('actualizarTodos', {});
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                },
                size: 'sm'
            });
            modalInstance.result.then(function (selectedItem) {
                
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
                traerSucursalesPorEmpresa();
            });
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
