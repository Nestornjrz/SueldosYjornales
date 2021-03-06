﻿(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoHistoricoSucursalesCtrl', listadoHistoricoSucursalesCtrl);

    listadoHistoricoSucursalesCtrl.$inject = ['$rootScope', '$uibModal', 'sYjResource'];

    function listadoHistoricoSucursalesCtrl($rootScope, $uibModal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        
        vm.actualizar = function (historicoSucursale) {
            $rootScope.$broadcast('actualizarhistoricoSucursale', historicoSucursale);
        }

        vm.eliminar = function (historicoSucursale) {
            var modalInstance = $uibModal.open({
                templateUrl: 'ModalEliminacionSucursale.html',
                controller: function ($scope, $uibModalInstance) {
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
            vm.historicoSucursales = sYjResource
              .historicoSucursalesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
        $rootScope.$on('actualizarListadoHistoricoSucursales', function (event, objValRecibido) {
            vm.historicoSucursales = sYjResource
              .historicoSucursalesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
