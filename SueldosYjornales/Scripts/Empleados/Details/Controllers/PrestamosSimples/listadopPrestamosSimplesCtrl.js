(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadopPrestamosSimplesCtrl', listadopPrestamosSimplesCtrl);

    listadopPrestamosSimplesCtrl.$inject = ['$rootScope', '$modal', 'sYjResource'];

    function listadopPrestamosSimplesCtrl($rootScope, $modal, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.actualizar = function (prestamoSimple) {
            $rootScope.$broadcast('actualizarPrestamoSimple', prestamoSimple);
        }

        vm.traerDetallePrestamo = function (prestamoSimple) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalVerDetallePrestamo.html',
                controller: function ($scope, $modalInstance) {
                    $scope.prestamoSimple = prestamoSimple;
                    $scope.objeto = {};
                    $scope.objeto.id = prestamoSimple.movEmpleadoID;
                    $scope.objeto.mensaje = "Detalles del prestamo ";

                    sYjResource.prestamoSimMovs
                    .get(
                    { "movEmpleadoID": prestamoSimple.movEmpleadoID },
                    function (respuesta) {
                        $scope.respuesta = respuesta;
                    });

                    $scope.getClassCuotaDeLaLiquidacion =
                        function (mesAplicacion) {
                            var now = new Date();
                            var hoy_utc = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds());
                            var mesAplicacionDate = new Date(mesAplicacion.slice(0, 10));
                            if (mesAplicacionDate.getUTCMonth() == hoy_utc.getMonth() &&
                                mesAplicacionDate.getUTCFullYear() == hoy_utc.getFullYear()) {
                                return "resaltarFila";
                            }
                        }

                    $scope.cancel = function () {
                        $modalInstance.dismiss('cancel');
                    };
                }
                , size: 'lg'
            });
        }

        vm.eliminar = function (prestamoSimple) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminacionPrestamoSimple.html',
                controller: function ($scope, $modalInstance) {
                    $scope.prestamoSimple = prestamoSimple;
                    $scope.objeto = {};
                    $scope.objeto.id = prestamoSimple.prestamoSimpleID;
                    $scope.objeto.mensaje = "Se eliminara el prestamo numero ";
                    $scope.ok = function () {
                        sYjResource.prestamosSimples
                            .delete({ id: prestamoSimple.prestamoSimpleID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                                  vm.prestamosSimples = sYjResource
                                      .prestamosSimplesByEmpleadoID
                                      .query({ 'empleadoID': vm.empleadoID });
                              });
                        //$rootScope.$broadcast('actualizarTodos', {});
                    };
                    $scope.cancel = function () {
                        $modalInstance.dismiss('cancel');
                    };
                },
                size: ''//sm,lg
            });
            modalInstance.result.then(function (selectedItem) {

            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
                //vm.comisiones = sYjResource
                //.comisionesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
            });
        }

        //Eventos
        $rootScope.$on('empleadoID', function (event, objValRecibido) {
            vm.empleadoID = objValRecibido;
            vm.prestamosSimples = sYjResource
                .prestamosSimplesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });

        $rootScope.$on('actualizarListadoPrestamos', function (event, objValRecibido) {
            vm.prestamosSimples = sYjResource
                 .prestamosSimplesByEmpleadoID.query({ 'empleadoID': vm.empleadoID });
        });
    }
})();
