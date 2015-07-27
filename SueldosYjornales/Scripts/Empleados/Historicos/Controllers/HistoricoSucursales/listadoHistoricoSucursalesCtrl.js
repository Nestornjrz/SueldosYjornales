(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHistoricoSucursalesCtrl', listadoHistoricoSucursalesCtrl);

    listadoHistoricoSucursalesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoHistoricoSucursalesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        
        vm.actualizar = function (historicoSucursale) {
            $rootScope.$broadcast('actualizarhistoricoSucursale', historicoSucursale);
        }

        vm.eliminar = function (historicoSucursale) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacionSucursale.html',
                controller: function ($scope, $modalInstance) {
                    $scope.historicoSucursale = historicoSucursale;
                    $scope.objeto = {};
                    $scope.objeto.id = historicoSucursale.historicoSucursalID;
                    $scope.objeto.mensaje = "Se eliminara el Historico horarios numero ";
                    $scope.ok = function () {
                        sYjResource.historicoSucursales
                            .delete({ id: historicoSucursale.historicoSucursalID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.historicoSucursales = sYjResource
                                     .historicoSucursalesByEmpleadoID
                                      .query({ 'empleadoID': vm.empleadoID });
                              });
                        //$rootScope.$broadcast('actualizarTodos', {});
                    };
                    $scope.cancel = function () {
                        $modalInstance.dismiss('cancel');
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
            vm.historicoSucursales = sYjResource
              .historicoSucursalesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
