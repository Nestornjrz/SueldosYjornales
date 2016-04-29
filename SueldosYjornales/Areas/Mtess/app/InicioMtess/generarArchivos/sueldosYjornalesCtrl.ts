// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module app.inicioMtess.generarArchivos {
    "use strict";

    interface ISueldosYjornalesCtrl {
        activate: () => void;
        listadoSueldoYjornales: [app.dto.ISueldoYjornaleDto];
        mostrarLoading: boolean;
        syjDtoTotal: app.dto.ISueldoYjornaleDto;
    }
    interface IMyScope extends ng.IScope { }
    interface IMyroostCope extends ng.IRootScopeService { }

    class SueldosYjornalesCtrl implements ISueldosYjornalesCtrl {
        //#region Propiedades
        listadoSueldoYjornales: [app.dto.ISueldoYjornaleDto];
        mostrarLoading: boolean;
        syjDtoTotal: app.dto.ISueldoYjornaleDto;
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
                    vm.syjDtoTotal = <app.dto.ISueldoYjornaleDto>{
                        nombreReferencia: "TOTALES",
                        nroPatronal : 0,
                        h_Jul: 0,
                        s_Jul: 0,
                        h_Ago: 0,
                        s_Ago: 0,
                        h_Set: 0,
                        s_Set: 0,
                        h_Oct: 0,
                        s_Oct: 0,
                        h_Nov: 0,
                        s_Nov: 0,
                        h_Dic: 0,
                        s_Dic: 0,
                        aguinaldo: 0,
                        vacaciones: 0,
                        total_H: 0,
                        total_S: 0,
                        totalGeneral: 0
                    };
                    _.each(vm.listadoSueldoYjornales, (syjDto: app.dto.ISueldoYjornaleDto) => {                      
                        vm.syjDtoTotal.nroPatronal = 0;
                        vm.syjDtoTotal.documento = 0;
                        vm.syjDtoTotal.h_Jul += syjDto.h_Jul;
                        vm.syjDtoTotal.s_Jul += syjDto.s_Jul;
                        vm.syjDtoTotal.h_Ago += syjDto.h_Ago;
                        vm.syjDtoTotal.s_Ago += syjDto.s_Ago;
                        vm.syjDtoTotal.h_Set += syjDto.h_Set;
                        vm.syjDtoTotal.s_Set += syjDto.s_Set;
                        vm.syjDtoTotal.h_Oct += syjDto.h_Oct;
                        vm.syjDtoTotal.s_Oct += syjDto.s_Oct;
                        vm.syjDtoTotal.h_Nov += syjDto.h_Nov;
                        vm.syjDtoTotal.s_Nov += syjDto.s_Nov;
                        vm.syjDtoTotal.h_Dic += syjDto.h_Dic;
                        vm.syjDtoTotal.s_Dic += syjDto.s_Dic;
                        vm.syjDtoTotal.aguinaldo += syjDto.aguinaldo;
                        vm.syjDtoTotal.vacaciones += syjDto.vacaciones;
                        vm.syjDtoTotal.total_H += syjDto.total_H;
                        vm.syjDtoTotal.total_S += syjDto.total_S;
                        vm.syjDtoTotal.totalGeneral += syjDto.totalGeneral;
                    });
                    vm.listadoSueldoYjornales.push(vm.syjDtoTotal);
                });
        }

        addClass(syjDto: app.dto.ISueldoYjornaleDto) {
            if (syjDto.nombreReferencia == "TOTALES"){
                return "Negrita";
            }
        }
    }

    angular.module("syjApp").controller("SueldosYjornalesCtrl", SueldosYjornalesCtrl);
}