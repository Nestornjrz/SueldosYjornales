(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoEmpresasCtrl', listadoEmpresasCtrl);

    listadoEmpresasCtrl.$inject = ['$rootScope','$modal', 'sYjResource'];

    function listadoEmpresasCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.empresas = sYjResource.empresas.query();

        vm.eliminar = function (empresa) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacion.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.empresa = empresa;
                    $scope.objeto = {};
                    $scope.objeto.id = empresa.empresaID;
                    $scope.objeto.mensaje = "Se eliminara la empresa numero ";
                    $scope.ok = function () {
                        sYjResource.empresas.delete({ id: empresa.empresaID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.empresas = sYjResource.empresas.query();
                              });

                        //$rootScope.$broadcast('actualizarTodos', {});
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                },
                size: 'sm'
            });
        }
        vm.actualizar = function (empresa) {
            $rootScope.$broadcast('actualizarEmpresa', empresa);
        }

        //Eventos
        $rootScope.$on("actualizarListadoEmpresas", function (event, datoRecibido) {
            vm.empresas = sYjResource.empresas.query();
        });
    }
})();
