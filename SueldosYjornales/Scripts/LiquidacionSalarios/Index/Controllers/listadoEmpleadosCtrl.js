(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('listadoEmpleadosCtrl', listadoEmpleadosCtrl);

    listadoEmpleadosCtrl.$inject = ['$scope', '$rootScope', '$uibModal', 'sYjResource'];

    function listadoEmpleadosCtrl($scope, $rootScope, $uibModal, sYjResource) {
        var vm = this;
       
        //#region OBSERVANDO VARIABLES
        $scope.$watch('liqui.mes', function (newVal, oldVal) {
            if ($scope.liqui == null) {
                return;
            }
            if (newVal === oldVal) {
                return;
            }
            recuperarListadoDeEmpleados()
            //alert('Se modifico liqui mes');
        });
        $scope.$watch('liqui.year', function (newVal, oldVal) {
            if ($scope.liqui == null) {
                return;
            }
            if (newVal === oldVal) {
                return;
            }
            recuperarListadoDeEmpleados();
            //alert('Se modifico liqui year');
        });
        //#endregion
        function recuperarListadoDeEmpleados() {
            if ($scope.liqui.mes == null || $scope.liqui.year == null) {
                return;
            }
            $scope.empleados = sYjResource.empleadosSegunUbicacionSucursal
                .query({
                    mes: $scope.liqui.mes.mesID,
                    year:$scope.liqui.year
                });
            $('#input_SeleccionarTodo').prop("checked", false);
            $scope.cantSeleccionados = 0;
        }
        //#region SE MANEJA EL TAB ENTRE LA LIQUIDACION DE SALARIO Y AGUINALDO
        vm.menu = {};
        vm.menu.liqSalario = {};
        vm.menu.liqAguinaldo = {};
        vm.liqSalarioFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.liqSalario.class = "active";
            vm.menu.liqSalario.mostrar = true;
        }
        vm.liqAguinaldoFn = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            ocultar();
            vm.menu.liqAguinaldo.class = "active";
            vm.menu.liqAguinaldo.mostrar = true;
        }
        function ocultar() {
            vm.menu.liqSalario = {};
            vm.menu.liqAguinaldo = {};
        }
        vm.menu.liqSalario.class = "active";
        vm.menu.liqSalario.mostrar = true;
        //#endregion
        $scope.liqui = {};
        $scope.liqui.empleadosSeleccionados = [];
        $scope.cantSeleccionados = 0;
        //#region SELECCION DE FUNCIONARIOS
        $scope.seleccionarTodo = function (event) {
            var cantidad = 0;
            $scope.liqui.empleadosSeleccionados = [];
            _.each($scope.empleados, function (empleado, key) {
                if (event.target.checked) {
                    cantidad++;
                    $scope.liqui.empleadosSeleccionados.push(empleado.empleadoID);
                }
                empleado.seleccionadoSn = (event.target.checked) ? true : false;
            });
            $scope.cantSeleccionados = cantidad;
        };
        $scope.seleccionIndividual = function () {
            $scope.cantSeleccionados = 0;
            var cantidad = 0;
            $scope.liqui.empleadosSeleccionados = [];
            _.each($scope.empleados, function (empleado, key) {
                cantidad++;
                if (empleado.seleccionadoSn) {
                    $scope.cantSeleccionados++;
                    $scope.liqui.empleadosSeleccionados.push(empleado.empleadoID);
                }
            });
            if (cantidad == $scope.cantSeleccionados) {
                $scope.varSeleccionarTodo = true;
            } else {
                $scope.varSeleccionarTodo = false;
            }
        };
        //#endregion
        //#region Datos del formulario para mes y año
        $scope.meses = [
          { "mesID": 1, "nombreMes": "Enero" },
          { "mesID": 2, "nombreMes": "Febrero" },
          { "mesID": 3, "nombreMes": "Marzo" },
          { "mesID": 4, "nombreMes": "Abril" },
          { "mesID": 5, "nombreMes": "Mayo" },
          { "mesID": 6, "nombreMes": "Junio" },
          { "mesID": 7, "nombreMes": "Julio" },
          { "mesID": 8, "nombreMes": "Agosto" },
          { "mesID": 9, "nombreMes": "Setiembre" },
          { "mesID": 10, "nombreMes": "Octubre" },
          { "mesID": 11, "nombreMes": "Noviembre" },
          { "mesID": 12, "nombreMes": "Diciembre" }
        ];

        var fechaActual = new Date();
        var yearActual = fechaActual.getFullYear();
        var mesActual = fechaActual.getMonth();//desde 0 - 11

        $scope.years = [];
        for (var i = -1; i < 5; i++) {
            $scope.years.push(yearActual - i);
        }
        //Se cargan el año y mes por defecto
        //$scope.liqui.mes = _.findWhere($scope.meses, { "mesID": mesActual + 1 });
        //$scope.liqui.year = _.find($scope.years, function (num) {
        //    return num == yearActual;
        //});
        //#endregion
        //#region Se genera la liquidacion de salarios
        $scope.guardar = function () {
            var estaSeguro = confirm("Esta seguro de generar estas liquidaciones?");
            if (!estaSeguro) {
                alert("A cancelado la generacion de Liquidacion para los empleados seleccionados");
                return;
            }
            sYjResource.liquidacionSalarios.save($scope.liqui)
           .$promise.then(
            function (mensaje) {
                $scope.mensaje = mensaje;
                if (!mensaje.error) {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                } else {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                }
                $rootScope.$broadcast('actualizarLogs',$scope.mensaje);
                $scope.recuperarDetalles();
                $rootScope.$broadcast('mostrarImprimirLiquidacion', true);
            },
            function (mensaje) {
                $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            });
        }
        //Se recuperan los detalles de las liquidaciones para mostrarlo
        $scope.recuperarDetalles = function () {
            sYjResource.liquidacionSalariosDetalles.save($scope.liqui)
           .$promise.then(
            function (mensaje) {
                $scope.mensaje = mensaje;
                if (!mensaje.error) {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                } else {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                }
                $rootScope.$broadcast('actualizarDetalles', $scope.mensaje);
                $rootScope.$broadcast('formLiquidacionSalario', $scope.liqui);
                $rootScope.$broadcast('mostrarImprimirLiquidacion', true);                
            },
            function (mensaje) {
                $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            });
        }
        //Se recupera los detalles de un solo empleado
        $scope.recuperarDetallesUnSoloEmpleado = function () {
            sYjResource.liquiSalariosDetallesUnSoloEmpleado.save($scope.liqui)
           .$promise.then(
            function (mensaje) {
                $scope.mensaje = mensaje;
                if (!mensaje.error) {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                } else {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                }
                $rootScope.$broadcast('actualizarDetalles', $scope.mensaje);
                $rootScope.$broadcast('formLiquidacionSalario', $scope.liqui);
                $rootScope.$broadcast('mostrarImprimirLiquidacion', false);
            },
            function (mensaje) {
                $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            });
        }
        //#endregion
        //#region Liquidacion de aguinaldo
        $scope.generarLiqAguinaldo = function () {
            sYjResource.liquidacionAguinaldos.save($scope.liqui)
             .$promise.then(
            function (mensaje) {
                $scope.mensaje = mensaje;
                if (!mensaje.error) {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                } else {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                }
                $rootScope.$broadcast('actualizarLogsAguinaldo', $scope.mensaje);
                $scope.recuperarDetallesAguinaldo();
            },
            function (mensaje) {
                $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            });
        }
        $scope.recuperarDetallesAguinaldo = function () {
            sYjResource.liquidacionAguinaldosDetalles.save($scope.liqui)
           .$promise.then(
            function (mensaje) {
                $scope.mensaje = mensaje;
                if (!mensaje.error) {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                } else {
                    $scope.mensajeDelServidor = mensaje.mensajeDelProceso;
                }
                $rootScope.$broadcast('actualizarDetallesAguinaldos', $scope.mensaje);
                $rootScope.$broadcast('formLiquidacionAguinaldo', $scope.liqui);
            },
            function (mensaje) {
                $scope.mensajeDelServidor = mensaje.data.mensajeDelProceso;
            });
        }
        //#endregion
    }
})();