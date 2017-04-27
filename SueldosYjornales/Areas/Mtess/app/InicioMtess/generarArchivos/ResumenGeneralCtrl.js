// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
var app;
(function (app) {
    var inicioMtess;
    (function (inicioMtess) {
        var generarArchivos;
        (function (generarArchivos) {
            "use strict";
            var ResumenGeneralCtrl = (function () {
                function ResumenGeneralCtrl(dataAccessService, $scope, $rootScope, $timeout
                    //private $uibModal: angular.ui.bootstrap.IModalService
                ) {
                    this.dataAccessService = dataAccessService;
                    this.$scope = $scope;
                    this.$rootScope = $rootScope;
                    this.$timeout = $timeout;
                    this.activate();
                }
                ResumenGeneralCtrl.prototype.activate = function () {
                };
                //#region EVENTOS DE USUARIO
                ResumenGeneralCtrl.prototype.traerResumenGeneral = function () {
                    var vm = this;
                    vm.mostrarLoading = true;
                    vm.dataAccessService.getResumenGeneralDtoResource()
                        .query(function (listadoResumenGeneral) {
                        vm.listadoResumenGeneral = listadoResumenGeneral;
                        vm.mostrarLoading = false;
                    });
                };
                return ResumenGeneralCtrl;
            }());
            //#endregion
            ResumenGeneralCtrl.$inject = [
                "dataAccessService",
                "$scope",
                "$rootScope",
                "$timeout"
                //"$uibModal"
            ];
            angular.module("syjApp").controller("ResumenGeneralCtrl", ResumenGeneralCtrl);
        })(generarArchivos = inicioMtess.generarArchivos || (inicioMtess.generarArchivos = {}));
    })(inicioMtess = app.inicioMtess || (app.inicioMtess = {}));
})(app || (app = {}));
//# sourceMappingURL=ResumenGeneralCtrl.js.map