(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoComisionesCtrl', listadoComisionesCtrl);

    listadoComisionesCtrl.$inject = ['$rootScope', '$uibModal', 'sYjResource'];

    function listadoComisionesCtrl($rootScope, $uibModal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.actualizar = function (comisione) {
            $rootScope.$broadcast('actualizarComisione', comisione);
        }

        vm.eliminar = function (comisione) {
            var modalInstance = $uibModal.open({
                templateUrl: 'ModalEliminacionComisione.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.comisione = comisione;
                    $scope.objeto = {};
                    $scope.objeto.id = comisione.comisionID;
                    $scope.objeto.mensaje = "Se eliminara la comision numero ";
                    $scope.ok = function () {
                        sYjResource.comisiones
                            .delete({ id: comisione.comisionID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.comisiones = sYjResource
                                      .comisionesByEmpleadoID
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
                vm.comisiones = sYjResource
                .comisionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
            });
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.comisiones = sYjResource
                .comisionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadoComisiones', function (event, objValRecibido) {
            vm.comisiones = sYjResource
               .comisionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
