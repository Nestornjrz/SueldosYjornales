(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHistoricoTelefonosCtrl', listadoHistoricoTelefonosCtrl);

    listadoHistoricoTelefonosCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoHistoricoTelefonosCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        
        vm.actualizar = function (historicoTelefono) {
            $rootScope.$broadcast('actualizarhistoricoTelefono', historicoTelefono);
        }

        vm.eliminar = function (historicoTelefono) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacionHistoricoTelefono.html',
                controller: function ($scope, $modalInstance) {
                    $scope.historicoTelefono = historicoTelefono;
                    $scope.objeto = {};
                    $scope.objeto.id = historicoTelefono.historicoTelefonoID;
                    $scope.objeto.mensaje = "Se eliminara el Historico de telefono numero ";
                    $scope.ok = function () {
                        sYjResource.historicoTelefonos
                            .delete({ id: historicoTelefono.historicoTelefonoID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.historicoTelefono = sYjResource
                                    .historicoTelefonosByEmpleadoID
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
                vm.historicoTelefonos = sYjResource
                .historicoTelefonosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
            });
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.historicoTelefonos = sYjResource
                .historicoTelefonosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadoHistoricoTelefonos', function (event, objValRecibido) {
            vm.historicoTelefonos = sYjResource
               .historicoTelefonosByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
