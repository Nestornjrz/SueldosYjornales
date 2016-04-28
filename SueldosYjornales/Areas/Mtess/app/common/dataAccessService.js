var app;
(function (app) {
    var common;
    (function (common) {
        var DataAccessService = (function () {
            function DataAccessService($resource, syjPath) {
                this.$resource = $resource;
                this.syjPath = syjPath;
            }
            DataAccessService.prototype.getEmpleadoYobreroDtoResource = function () {
                return this.$resource(this.syjPath.empleadosYobreros);
            };
            DataAccessService.prototype.getSueldoYjornaleDtoResource = function () {
                return this.$resource(this.syjPath.sueldosYjornales);
            };
            DataAccessService.prototype.getResumenGeneralDtoResource = function () {
                return this.$resource(this.syjPath.resumenesGenerales);
            };
            DataAccessService.$inject = ["$resource", "syjPath"];
            return DataAccessService;
        }());
        common.DataAccessService = DataAccessService;
        angular
            .module("common.services")
            .service("dataAccessService", DataAccessService);
    })(common = app.common || (app.common = {}));
})(app || (app = {}));
//# sourceMappingURL=dataAccessService.js.map