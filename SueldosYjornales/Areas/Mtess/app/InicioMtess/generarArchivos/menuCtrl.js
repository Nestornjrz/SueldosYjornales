// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
var appinicioMtess;
(function (appinicioMtess) {
    var generarArchivos;
    (function (generarArchivos) {
        "use strict";
        var MenuCtrl = (function () {
            function MenuCtrl($location) {
                this.$location = $location;
                this.activate();
            }
            MenuCtrl.prototype.activate = function () {
                this.ocultar();
            };
            //#region EVENTOS DE USUARIO
            MenuCtrl.prototype.empleadosYobrerosFn = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
                this.ocultar();
                this.menu.empleadosYobreros = { class: "active", mostrar: true };
            };
            MenuCtrl.prototype.sueldosYjornalesFn = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
                this.ocultar();
                this.menu.sueldosYjornales = { class: "active", mostrar: true };
            };
            MenuCtrl.prototype.resumenGeneralFn = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
                this.ocultar();
                this.menu.resumenGeneral = { class: "active", mostrar: true };
            };
            MenuCtrl.prototype.ocultar = function () {
                var vm = this;
                vm.menu = {
                    empleadosYobreros: { class: "", mostrar: false },
                    sueldosYjornales: { class: "", mostrar: false },
                    resumenGeneral: { class: "", mostrar: false }
                };
            };
            //#endregion
            MenuCtrl.$inject = ["$location"];
            return MenuCtrl;
        }());
        angular.module("syjApp").controller("MenuCtrl", MenuCtrl);
    })(generarArchivos = appinicioMtess.generarArchivos || (appinicioMtess.generarArchivos = {}));
})(appinicioMtess || (appinicioMtess = {}));
//# sourceMappingURL=menuCtrl.js.map