(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('mensajesCtrl', mensajesCtrl);

    mensajesCtrl.$inject = ['$scope', '$modal', '$rootScope', 'sYjResource'];

    function mensajesCtrl($scope, $modal, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.menu = {};
        vm.menu.introduccion = true;

        vm.traerDetallePrestamo = function (movimiento) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalVerDetallePrestamo.html',
                controller: function ($scope, $modalInstance) {
                    $scope.movimiento = movimiento;
                    $scope.objeto = {};
                    $scope.objeto.id = movimiento.movEmpleadoID;
                    $scope.objeto.mensaje = "Detalles del prestamo ";

                    sYjResource.prestamoSimMovs
                    .get(
                    { "movEmpleadoID": movimiento.movEmpleadoID },
                    function (respuesta) {
                        $scope.respuesta = respuesta;
                    });

                    $scope.getClassCuotaDeLaLiquidacion =
                        function (movEmpleadoDetID_Cuota) {
                            if (movimiento.movEmpleadoDetID == movEmpleadoDetID_Cuota) {
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

        vm.eliminarLiquidacion = function (movimiento) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalEliminarLiquidacion.html',
                controller: function ($scope, $modalInstance) {
                    $scope.movimiento = movimiento;
                    $scope.objeto = {};
                    $scope.objeto.id = movimiento.movEmpleadoID;
                    $scope.objeto.mensaje = "Se eliminara la liquidacion de sueldo ";
                    $scope.eliminar = function () {
                        sYjResource.liquidacionSalarios
                            .delete({ id: movimiento.movEmpleadoID },
                              function (respuesta) {
                                  $scope.respuesta = respuesta;
                              });
                        //$rootScope.$broadcast('actualizarTodos', {});
                    };
                    $scope.cancel = function () {
                        $modalInstance.dismiss('cancel');
                    };
                }
                , size: 'lg'
            });
            modalInstance.result.then(function (selectedItem) {

            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());               
            });
        }

        vm.logsFn = function ($event) {
            if ($event != null) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            ocultar();
            vm.menu.logs.class = "active";
            vm.menu.logs.mostrar = true;
        }

        vm.detalleEmpleadoFn = function ($event) {
            if ($event != null) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            ocultar();
            vm.menu.detalleEmpleado.class = "active";
            vm.menu.detalleEmpleado.mostrar = true;
        }

        function ocultar() {
            vm.menu.introduccion = false;
            vm.menu.logs = {};
            vm.menu.detalleEmpleado = {};
        }

        vm.getClassEmpleadoDet = function (detalle) {
            if (detalle.liquidacionConcepto.liquidacionConceptoID == 5) {
                return "resaltado";
            }
        }

        $rootScope.$on('actualizarLogs', function (event, objValRecibido) {
            vm.logs = objValRecibido;
            vm.logsFn(null);
        });

        $rootScope.$on('actualizarDetalles', function (event, objValRecibido) {
            vm.movimientos = objValRecibido;
            vm.detalleEmpleadoFn(null);
        });

        $rootScope.$on('formLiquidacionSalario', function (event, objValRecibido) {
            vm.formLiquidacionSalario = objValRecibido;
            var json = angular.toJson(vm.formLiquidacionSalario);
            $('#jsonInput').val(json);//Esto le pasa al formulario para poder imprimir          
        });

        $scope.modificarPrestamo = function (movEmpleadoDet) {
            sYjResource.modificarPrestamos.save(movEmpleadoDet)
           .$promise.then(
            function (mensaje) {
                mostrarModal(mensaje);
                //LLama de nuevo al detalle
                sYjResource.liquidacionSalariosDetalles.save(vm.formLiquidacionSalario)
               .$promise.then(
               function (mensaje) {
                   vm.movimientos = mensaje;
               },
               function (mensaje) {
                   //$scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
               });
            },
            function (mensaje) {
                $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            });
        }

        //Funcion
        //se llama al modal
        function mostrarModal(mensaje) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalMensajeModificacionPrestamo.html',
                controller: function ($scope, $modalInstance) {
                    $scope.mensaje = mensaje;
                    $scope.objeto = {};
                    $scope.objeto.id = mensaje.movEmpleadoDetID;
                    $scope.objeto.mensaje = "Mensaje de la modificacion del prestamo ";
                    $scope.ok = function () {
                        //$rootScope.$broadcast('actualizarTodos', {});
                    };
                    $scope.cancel = function () {
                        $modalInstance.dismiss('cancel');
                    };
                }
                //size: 'lg'
            });
        }
    }
})();
