(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoAnticiposCtrl', listadoAnticiposCtrl);

    listadoAnticiposCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoAnticiposCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.actualizar = function (anticipo) {
            $rootScope.$broadcast('actualizarAnticipo', anticipo);
        }

        vm.eliminar = function (anticipo) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacionAnticipo.html',
                controller: function ($scope, $modalInstance) {
                    $scope.anticipo = anticipo;
                    $scope.objeto = {};
                    $scope.objeto.id = anticipo.anticipoID;
                    $scope.objeto.mensaje = "Se eliminara el anticipo numero ";
                    $scope.ok = function () {
                        sYjResource.anticipos
                            .delete({ id: anticipo.anticipoID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.anticipos = sYjResource
                                      .anticiposByEmpleadoID
                                      .query({ 'empleadoID': vm.empleadoID });
                              });
                        //$rootScope.$broadcast('actualizarTodos', {});
                    };
                    $scope.cancel = function () {
                        $modalInstance.dismiss('cancel');
                    };
                }
                //size: 'sm'
            });
            modalInstance.result.then(function (selectedItem) {

            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
                vm.comisiones = sYjResource
                .anticiposByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
            });
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.anticipos = sYjResource
                .anticiposByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
        $rootScope.$on('actualizarListadoAnticipos', function (event, objValRecibido) {
            vm.anticipos = sYjResource
              .anticiposByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
