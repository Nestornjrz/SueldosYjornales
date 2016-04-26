(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoProfesionesCtrl', listadoProfesionesCtrl);

    listadoProfesionesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoProfesionesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.profesiones = sYjResource.profesiones.query();

        vm.actualizar = function (profesione) {
            $rootScope.$broadcast('actualizarProfesione',profesione);
        }
        vm.eliminar = function (profesione) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacionProfesion.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.profesione = profesione;
                    $scope.objeto = {};
                    $scope.objeto.id = profesione.profesionID;
                    $scope.objeto.mensaje = "Se eliminara la profesion numero ";
                    $scope.ok = function () {
                        sYjResource.profesiones.delete({ id: profesione.profesionID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.profesiones = sYjResource.profesiones.query();
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
                vm.profesiones = sYjResource.profesiones.query();
            });
        }

        //Capturando eventos de usuario
        $rootScope.$on('actualizarListadoProfesiones', function (event, objValRecibido) {
            vm.profesiones = sYjResource.profesiones.query();
        });
    }
})();
