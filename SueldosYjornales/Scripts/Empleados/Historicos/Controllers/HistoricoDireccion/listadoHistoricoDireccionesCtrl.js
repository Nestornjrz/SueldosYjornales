(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHistoricoDireccionesCtrl', listadoHistoricoDireccionesCtrl);

    listadoHistoricoDireccionesCtrl.$inject = ['$rootScope', '$uibModal', 'sYjResource'];

    function listadoHistoricoDireccionesCtrl($rootScope, $uibModal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.actualizar = function (historicoDireccion) {
            $rootScope.$broadcast('actualizarHistoricoDireccion', historicoDireccion);
        }

        vm.eliminar = function (historicoDireccion) {
            var modalInstance = $uibModal.open({
                templateUrl: 'ModalEliminacionHistoricoDireccion.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.historicoDireccion = historicoDireccion;
                    $scope.objeto = {};
                    $scope.objeto.id = historicoDireccion.historicoDireccionID;
                    $scope.objeto.mensaje = "Se eliminara el Historico direccion numero ";
                    $scope.ok = function () {
                        sYjResource.historicoDirecciones
                            .delete({ id: historicoDireccion.historicoDireccionID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.historicoDireccion = sYjResource.historicoDirecciones.query();
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
                vm.historicoDirecciones = sYjResource
                .historicoDireccionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
            });
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.historicoDirecciones = sYjResource
                .historicoDireccionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadohistoricoDirecciones', function (event, valor) {
            vm.historicoDirecciones = sYjResource
              .historicoDireccionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

    }
})();
