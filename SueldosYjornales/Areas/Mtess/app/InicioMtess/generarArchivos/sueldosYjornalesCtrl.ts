// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module app.inicioMtess.generarArchivos {
    "use strict";

    interface ISueldosYjornalesCtrl {
        activate: () => void;
        listadoSueldoYjornales: [app.dto.ISueldoYjornaleDto];
        mostrarLoading: boolean;
    }
    interface IMyScope extends ng.IScope { }
    interface IMyroostCope extends ng.IRootScopeService { }

    class SueldosYjornalesCtrl implements ISueldosYjornalesCtrl {
        //#region Propiedades
        listadoSueldoYjornales: [app.dto.ISueldoYjornaleDto];
        mostrarLoading: boolean;
        //#endregion
        public static $inject: string[] = [
            "dataAccessService",
            "$scope",
            "$rootScope",
            "$timeout"
            //"$uibModal"
        ];

        constructor(
            private dataAccessService: app.common.DataAccessService,
            private $scope: IMyScope,
            private $rootScope: IMyroostCope,
            private $timeout: ng.ITimeoutService
            //private $uibModal: angular.ui.bootstrap.IModalService
        ) {
            this.activate();
        }

        activate() {

        }
        traerSueldosYjornales() {
            var vm = this;
            vm.mostrarLoading = true;
            vm.dataAccessService.getSueldoYjornaleDtoResource()
                .query((listadoSueldoYjornales: [app.dto.ISueldoYjornaleDto]) => {
                    vm.listadoSueldoYjornales = listadoSueldoYjornales;
                    vm.mostrarLoading = false;
                });
        }
    }

    angular.module("syjApp").controller("SueldosYjornalesCtrl", SueldosYjornalesCtrl);
}