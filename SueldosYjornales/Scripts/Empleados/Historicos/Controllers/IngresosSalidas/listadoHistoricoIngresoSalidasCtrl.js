(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHistoricoIngresoSalidasCtrl', listadoHistoricoIngresoSalidasCtrl);

    listadoHistoricoIngresoSalidasCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoHistoricoIngresoSalidasCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        
        vm.actualizar = function (historicoIngresoSalida) {
            $rootScope.$broadcast('actualizarHistoricoIngresoSalida', historicoIngresoSalida);
        }

        vm.eliminar = function (historicoIngresoSalida) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacionHistoricoIngresoSalida.html',
                controller: function ($scope, $modalInstance) {
                    $scope.historicoIngresoSalida = historicoIngresoSalida;
                    $scope.objeto = {};
                    $scope.objeto.id = historicoIngresoSalida.historicoIngresoSalidaID;
                    $scope.objeto.mensaje = "Se eliminara el Historico salario numero ";
                    $scope.ok = function () {
                        sYjResource.historicoIngresoSalidas
                            .delete({ id: historicoIngresoSalida.historicoIngresoSalidaID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.historicoIngresoSalidas = sYjResource
                .historicoIngresoSalidasByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
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
                vm.historicoIngresoSalidas = sYjResource
                .historicoIngresoSalidasByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
            });
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.historicoIngresoSalidas = sYjResource
                .historicoIngresoSalidasByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadohistoricoIngresoSalidas', function (event, objValRec) {
            vm.historicoIngresoSalidas = sYjResource
                .historicoIngresoSalidasByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
