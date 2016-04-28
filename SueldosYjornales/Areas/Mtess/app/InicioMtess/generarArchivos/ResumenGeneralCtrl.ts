// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module app.inicioMtess.generarArchivos {
    "use strict";

    interface IResumenGeneralCtrl {        
        activate: () => void;
        listadoResumenGeneral: [app.dto.IResumenGeneralDto];
        mostrarLoading: boolean;
    }
    interface IMyScope extends ng.IScope { }
    interface IMyroostCope extends ng.IRootScopeService { }

    class ResumenGeneralCtrl implements IResumenGeneralCtrl {        
        //#region Propiedades
        listadoResumenGeneral: [app.dto.IResumenGeneralDto];
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

        //#region EVENTOS DE USUARIO
        traerResumenGeneral() {
            var vm = this;
            vm.mostrarLoading = true;
            vm.dataAccessService.getResumenGeneralDtoResource()
                .query((listadoResumenGeneral: [app.dto.IResumenGeneralDto]) => {
                    vm.listadoResumenGeneral = listadoResumenGeneral;
                    vm.mostrarLoading = false;
                });
        }
        //#endregion
    }

    angular.module("syjApp").controller("ResumenGeneralCtrl", ResumenGeneralCtrl);
}