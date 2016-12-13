(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('planillaAguinaldosCtrl', planillaAguinaldosCtrl);

    planillaAguinaldosCtrl.$inject = ['$scope', '$timeout', '$rootScope', 'sYjResource'];

    function planillaAguinaldosCtrl($scope, $timeout, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        $timeout(function () {
            vm.psfDto = angular.fromJson(vm.psfDto);

            sYjResource.paraPlanillaAguinaldos.save(vm.psfDto)
           .$promise.then(
                function (mensaje) {
                    if (!mensaje.error) {
                        vm.listLiquidaciones = mensaje.objetoDto;
                        vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                        //Se le coloca mostrar
                      _.each(vm.listLiquidaciones, function (liqui, key) {
                          liqui.mostrar = true;
                      });
                    } else {
                        vm.mensajeDelServidor = mensaje.mensajeDelProceso;
                    }
                },
                function (mensaje) {
                    vm.mensajeDelServidor = mensaje.data.mensajeDelProceso;
                }
            );
        });
        vm.mostrarOcultarFila = function (event, liquidacion) {
            if (!event.target.checked) {
                console.log(liquidacion);
               
                //Se recalculan los subtotales  
                var subTotal = {
                    sueldo: 0,
                    comision: 0,
                    descIps: 0,
                    descOtros: 0,
                    netoAcobrar: 0
                }
                var granTotal = {
                    sueldo: 0,
                    comision: 0,
                    descIps: 0,
                    descOtros: 0,
                    netoAcobrar: 0
                }

                var contador = 1;

                _.each(vm.listLiquidaciones, function (liqui, key) {
                    if (liqui.empleado.nombres.search("Subtotal") == -1 && liqui.mostrar == true) {
                        subTotal.sueldo += liqui.salarioBase;
                        subTotal.comision += liqui.comisiones;
                        subTotal.descIps += liqui.descIPS;
                        subTotal.descOtros += liqui.descOtros;
                        subTotal.netoAcobrar += liqui.netoAcobrar;
                    }

                    //liqui.puntero = contador++;

                    if (liqui.empleado.nombres.search("Subtotal") > -1) {
                        liqui.salarioBase = subTotal.sueldo;
                        liqui.comisiones = subTotal.comision;
                        liqui.descIPS = subTotal.descIps;
                        liqui.descOtros = subTotal.descOtros;
                        liqui.netoAcobrar = subTotal.netoAcobrar;
                        //Suma de totales para el gran total
                        granTotal.sueldo += subTotal.sueldo;
                        granTotal.comision += subTotal.comision;
                        granTotal.descIps += subTotal.descIps;
                        granTotal.descOtros += subTotal.descOtros;
                        granTotal.netoAcobrar += subTotal.netoAcobrar;

                        //Se serea los datos
                        subTotal.sueldo = 0;
                        subTotal.comision = 0;
                        subTotal.descIps = 0;
                        subTotal.descOtros = 0;
                        subTotal.netoAcobrar = 0;
                    }
                    //Se cambia el gran total
                    if (liqui.empleado.sucursale == null) {
                        liqui.salarioBase = granTotal.sueldo;
                        liqui.comisiones = granTotal.comision;
                        liqui.descIPS = granTotal.descIps;
                        liqui.descOtros = granTotal.descOtros;
                        liqui.netoAcobrar = granTotal.netoAcobrar;
                    }
                });
            }
        }
    }
})();
