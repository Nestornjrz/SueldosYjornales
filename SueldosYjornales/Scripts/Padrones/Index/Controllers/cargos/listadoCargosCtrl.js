(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoCargosCtrl', listadoCargosCtrl);

    listadoCargosCtrl.$inject = ['$rootScope', '$uibModal', 'sYjResource'];

    function listadoCargosCtrl($rootScope, $uibModal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.cargos = sYjResource.cargos.query();

        vm.eliminar = function (cargo) {
            var modalInstance = $uibModal.open({
                templateUrl: 'ModalEliminacionCargo.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.cargo = cargo;
                    $scope.objeto = {};
                    $scope.objeto.id = cargo.cargoID;
                    $scope.objeto.mensaje = "Se eliminara el cargo numero ";
                    $scope.ok = function () {
                        sYjResource.cargos.delete({ id: cargo.cargoID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.cargos = sYjResource.cargos.query();
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
                vm.cargos = sYjResource.cargos.query();
            });
        }
        vm.actualizar = function (cargo) {
            $rootScope.$broadcast('actualizarCargo', cargo);
        }
        //Captura de eventos
        $rootScope.$on('actualizarListadoCargos', function (evento, datorecibido) {
            vm.cargos = sYjResource.cargos.query();
        });
    }
})();
