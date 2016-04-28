module app.inicioMtess.generarArchivos {
    interface IEmpleadosYobrerosCtrl {
        listadoEmpleadoYobrero: [app.dto.IEmpleadoYobreroDto];
        mostrarLoading: boolean;
    }
    interface IMyScope extends ng.IScope { }
    interface IMyroostCope extends ng.IRootScopeService {
        motrarBotonImprimir: boolean;
    }
    class EmpleadosYobrerosCtrl implements IEmpleadosYobrerosCtrl {
        //#region Propiedades
        listadoEmpleadoYobrero: [app.dto.IEmpleadoYobreroDto];
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

        }
        //#region Eventos de usuario
        traerEmpleadosYobreros() {
            var vm = this;
            vm.mostrarLoading = true;
            vm.dataAccessService.getEmpleadoYobreroDtoResource()
                .query((listadoEmpleadoYobrero: [app.dto.IEmpleadoYobreroDto]) => {
                    vm.listadoEmpleadoYobrero = listadoEmpleadoYobrero;
                    vm.mostrarLoading = false;
                });
        }
        //#endregion
    }

    angular.module("syjApp").controller("EmpleadosYobrerosCtrl", EmpleadosYobrerosCtrl);
}