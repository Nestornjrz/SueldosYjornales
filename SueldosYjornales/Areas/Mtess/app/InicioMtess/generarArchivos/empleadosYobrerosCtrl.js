var app;
(function (app) {
    var inicioMtess;
    (function (inicioMtess) {
        var generarArchivos;
        (function (generarArchivos) {
            var EmpleadosYobrerosCtrl = (function () {
                function EmpleadosYobrerosCtrl(dataAccessService, $scope, $rootScope, $timeout) {
                    this.dataAccessService = dataAccessService;
                    this.$scope = $scope;
                    this.$rootScope = $rootScope;
                    this.$timeout = $timeout;
                }
                //#region Eventos de usuario
                EmpleadosYobrerosCtrl.prototype.traerEmpleadosYobreros = function () {
                    var vm = this;
                    vm.dataAccessService.getEmpleadoYobreroDtoResource()
                        .query(function (listadoEmpleadoYobrero) {
                        vm.listadoEmpleadoYobrero = listadoEmpleadoYobrero;
                    });
                };
                //#endregion
                EmpleadosYobrerosCtrl.$inject = [
                    "dataAccessService",
                    "$scope",
                    "$rootScope",
                    "$timeout"
                ];
                return EmpleadosYobrerosCtrl;
            }());
            angular.module("syjApp").controller("EmpleadosYobrerosCtrl", EmpleadosYobrerosCtrl);
        })(generarArchivos = inicioMtess.generarArchivos || (inicioMtess.generarArchivos = {}));
    })(inicioMtess = app.inicioMtess || (app.inicioMtess = {}));
})(app || (app = {}));
//# sourceMappingURL=empleadosYobrerosCtrl.js.map