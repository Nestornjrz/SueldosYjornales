(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('planillaSalarioCtrl', planillaSalarioCtrl);

    planillaSalarioCtrl.$inject = ['$scope', '$rootScope', 'sYjResource'];

    function planillaSalarioCtrl($scope, $rootScope, sYjResource) {
        /* jshint validthis:true */
        var vm = this;
        vm.empresas = sYjResource.empresas.query();

        vm.meses = [
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

        vm.years = [];
        for (var i = -1; i < 5; i++) {
            vm.years.push(yearActual - i);
        }

        vm.planillaSalario = {};
        vm.planillaSalario.mes = _.findWhere(vm.meses, { "mesID": mesActual + 1 });
        vm.planillaSalario.year = _.find(vm.years, function (num) {
            return num == yearActual;
        });

        $scope.$watch('vm.planillaSalario.empresa', function (newVal, oldVal) {
            if (vm.planillaSalario.empresa == null) {
                return;
            }
            vm.sucursales = sYjResource.sucursalesSegunEmpresaID
            .query({ "empresaID": vm.planillaSalario.empresa.empresaID });
        });
        $scope.$watch('vm.planillaSalario.sucursales', function (newVal, oldVal) {
            if (vm.planillaSalario.sucursales == null) {
                return;
            }
            recargarEljsonEnInput();
        });
        $scope.$watch('vm.planillaSalario.mes', function (newVal, oldVal) {
            if (vm.planillaSalario.sucursales == null) {
                return;
            }
            recargarEljsonEnInput();
        });
        $scope.$watch('vm.planillaSalario.year', function (newVal, oldVal) {
            if (vm.planillaSalario.sucursales == null) {
                return;
            }
            recargarEljsonEnInput();
        });
        //funciones
        function recargarEljsonEnInput() {
            var json = angular.toJson(vm.planillaSalario);
            $('#jsonInputPlanillaSalarios').val(json);
        }
    }
})();
