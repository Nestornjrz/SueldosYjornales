(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHitoricoSalariosCtrl', listadoHitoricoSalariosCtrl);

    listadoHitoricoSalariosCtrl.$inject = ['$rootScope', '$uibModal', 'sYjResource'];

    function listadoHitoricoSalariosCtrl($rootScope, $uibModal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;

 
        vm.actualizar = function (historicoSalario) {
            $rootScope.$broadcast('actualizarhistoricoSalario', historicoSalario);
        }

        vm.eliminar = function (historicoSalario) {
            var modalInstance = $uibModal.open({
                templateUrl: 'ModalEliminacionHistoricoSalario.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.historicoSalario = historicoSalario;
                    $scope.objeto = {};
                    $scope.objeto.id = historicoSalario.historicoSalarioID;
                    $scope.objeto.mensaje = "Se eliminara el Historico salario numero ";
                    $scope.ok = function () {
                        sYjResource.historicoSalarios
                            .delete({ id: historicoSalario.historicoSalarioID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.historicoSalario = sYjResource
                .historicoSalariosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
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
                vm.historicoSalarios = sYjResource
                .historicoSalariosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
            });
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.historicoSalarios = sYjResource
                .historicoSalariosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadoHistoricoSalario', function (event, objValRecivido) {
            vm.historicoSalarios = sYjResource
              .historicoSalariosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
