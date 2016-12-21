(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoNacionalidadesCtrl', listadoNacionalidadesCtrl);

    listadoNacionalidadesCtrl.$inject = ['$rootScope', '$uibModal', 'sYjResource'];

    function listadoNacionalidadesCtrl($rootScope, $uibModal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.nacionalidades = sYjResource.nacionalidades.query();
        vm.actualizar = function (nacionalidade) {
            $rootScope.$broadcast('actualizarNacionalidade', nacionalidade);
        }
        vm.eliminar = function (nacionalidade) {
            var modalInstance = $uibModal.open({
                templateUrl: 'ModalEliminacionNacionalidad.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.nacionalidade = nacionalidade;
                    $scope.objeto = {};
                    $scope.objeto.id = nacionalidade.nacionalidadID;
                    $scope.objeto.mensaje = "Se eliminara la nacionalidad numero ";
                    $scope.ok = function () {
                        sYjResource.nacionalidades.delete({ id: nacionalidade.nacionalidadID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.nacionalidades = sYjResource.nacionalidades.query();
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
                vm.nacionalidades = sYjResource.nacionalidades.query();
            });
        }
        //Captura de eventos
        $rootScope.$on('actualizarListadoNacionalidad', function (event,recibido) {
            vm.nacionalidades = sYjResource.nacionalidades.query();
        });
    }
})();
