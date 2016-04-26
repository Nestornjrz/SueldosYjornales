(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoUsuariosCtrl', listadoUsuariosCtrl);

    listadoUsuariosCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadoUsuariosCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.usuarios = sYjResource.usuarios.query();
        vm.actualizar = function (usuario) {
            $rootScope.$broadcast('actualizarUsuario', usuario);
        }
        vm.eliminar = function (usuario) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacionUsuario.html',
                controller: function ($scope, $uibModalInstance) {
                    $scope.usuario = usuario;
                    $scope.objeto = {};
                    $scope.objeto.id = usuario.usuarioID;
                    $scope.objeto.mensaje = "Se eliminara el usuario numero ";
                    $scope.ok = function () {
                        sYjResource.usuarios.delete({ id: usuario.usuarioID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.usuarios = sYjResource.usuarios.query();
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
                vm.usuarios = sYjResource.usuarios.query();
            });
        }
        //Eventos
        $rootScope.$on('actualizarListadoUsuarios', function (event,objValRecibido) {
            vm.usuarios = sYjResource.usuarios.query();
        });
    }
})();
