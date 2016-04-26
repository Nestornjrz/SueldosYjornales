(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHistoricoHorariosCtrl', listadoHistoricoHorariosCtrl);

    listadoHistoricoHorariosCtrl.$inject = ['$rootScope', '$uibModal', 'sYjResource'];

    function listadoHistoricoHorariosCtrl($rootScope, $uibModal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;

        vm.actualizar = function (historicoHorario) {
            $rootScope.$broadcast('actualizarHistoricoHorario', historicoHorario);
        }

        vm.eliminar = function (historicoHorario) {
            var modalInstance = $uibModal.open({
                templateUrl: 'ModalEliminacionHorarios.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.historicoHorario = historicoHorario;
                    $scope.objeto = {};
                    $scope.objeto.id = historicoHorario.historicoHorarioID;
                    $scope.objeto.mensaje = "Se eliminara el Historico horarios numero ";
                    $scope.ok = function () {
                        sYjResource.historicoHorarios
                            .delete({ id: historicoHorario.historicoHorarioID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.historicoHorarios = sYjResource
                                      .historicoHorariosByEmpleadoID
                                      .query({ 'empleadoID': vm.empleadoID });
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
                vm.historicoHorarios = sYjResource
                .historicoHorariosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
            });
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
