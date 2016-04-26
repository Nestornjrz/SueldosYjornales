// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module appinicioMtess.generarArchivos {
    "use strict";

    interface IMenu {
        empleadosYobreros: {
            class: string,
            mostrar: boolean
        };
        sueldosYjornales: {
            class: string,
            mostrar: boolean
        };
        resumenGeneral: {
            class: string,
            mostrar: boolean
        };
    }

    interface IMenuCtrl {
        menu: IMenu;
        activate: () => void;
    }

    class MenuCtrl implements IMenuCtrl {
        //#region PROPIEDADES
        menu: IMenu;
        //#endregion

        static $inject: string[] = ["$location"];

        constructor(private $location: angular.ILocationService) {
            this.activate();
        }

        activate() {
            this.ocultar();
        }
        //#region EVENTOS DE USUARIO
        empleadosYobrerosFn($event) {
            $event.preventDefault();
            $event.stopPropagation();
            this.ocultar();
            this.menu.empleadosYobreros = { class: "active", mostrar: true };
        }
        sueldosYjornalesFn($event) {
            $event.preventDefault();
            $event.stopPropagation();
            this.ocultar();
            this.menu.sueldosYjornales = { class: "active", mostrar: true };
        }
        resumenGeneralFn($event) {
            $event.preventDefault();
            $event.stopPropagation();
            this.ocultar();
            this.menu.resumenGeneral = { class: "active", mostrar: true };
        }

        ocultar() {
            var vm = this;
            vm.menu = {
                empleadosYobreros: { class: "", mostrar: false },
                sueldosYjornales: { class: "", mostrar: false },
                resumenGeneral: { class: "", mostrar: false }
            };
        }
        //#endregion
    }

    angular.module("syjApp").controller("MenuCtrl", MenuCtrl);
}