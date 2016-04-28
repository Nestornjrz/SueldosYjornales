// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
var app;
(function (app) {
    var inicioMtess;
    (function (inicioMtess) {
        var generarArchivos;
        (function (generarArchivos) {
            "use strict";
            var SueldosYjornalesCtrl = (function () {
                function SueldosYjornalesCtrl(dataAccessService, $scope, $rootScope, $timeout) {
                    this.dataAccessService = dataAccessService;
                    this.$scope = $scope;
                    this.$rootScope = $rootScope;
                    this.$timeout = $timeout;
                    this.activate();
                }
                SueldosYjornalesCtrl.prototype.activate = function () {
                };
                SueldosYjornalesCtrl.prototype.traerSueldosYjornales = function () {
                    var vm = this;
                    vm.mostrarLoading = true;
                    vm.dataAccessService.getSueldoYjornaleDtoResource()
                        .query(function (listadoSueldoYjornales) {
                        vm.listadoSueldoYjornales = listadoSueldoYjornales;
                        vm.mostrarLoading = false;
                    });
                };
                //#endregion
                SueldosYjornalesCtrl.$inject = [
                    "dataAccessService",
                    "$scope",
                    "$rootScope",
                    "$timeout"
                ];
                return SueldosYjornalesCtrl;
            }());
            angular.module("syjApp").controller("SueldosYjornalesCtrl", SueldosYjornalesCtrl);
        })(generarArchivos = inicioMtess.generarArchivos || (inicioMtess.generarArchivos = {}));
    })(inicioMtess = app.inicioMtess || (app.inicioMtess = {}));
})(app || (app = {}));
//# sourceMappingURL=sueldosYjornalesCtrl.js.map