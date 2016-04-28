(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoVacacionesCtrl', listadoVacacionesCtrl);

    listadoVacacionesCtrl.$inject = ['$rootScope', '$uibModal', 'sYjResource'];

    function listadoVacacionesCtrl($rootScope, $uibModal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.actualizar = function (vacacione) {
            $rootScope.$broadcast('actualizarVacacione', vacacione);
        }

        vm.eliminar = function (vacacione) {
            var modalInstance = $uibModal.open({
                templateUrl: 'ModalEliminacionVacacione.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.vacacione = vacacione;
                    $scope.objeto = {};
                    $scope.objeto.id = vacacione.vacacionID;
                    $scope.objeto.mensaje = "Se eliminara la comision numero ";
                    $scope.ok = function () {
                        sYjResource.vacaciones
                            .delete({ id: vacacione.vacacionID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.vacaciones = sYjResource
                                      .vacacionesByEmpleadoID
                                      .query({ 'empleadoID': vm.empleadoID });
                              });
                        //$rootScope.$broadcast('actualizarTodos', {});
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                },
                size: 'lg'
            });
            modalInstance.result.then(function (selectedItem) {

            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
                vm.vacaciones = sYjResource
                .vacacionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
            });
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.vacaciones = sYjResource
                .vacacionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadoVacaciones', function (event, objValRecibido) {
            vm.vacaciones = sYjResource
               .vacacionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
