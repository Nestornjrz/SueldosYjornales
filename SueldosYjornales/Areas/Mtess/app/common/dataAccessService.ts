module app.common {
    interface ISyjPath {
        empleadosYobreros: string;
        sueldosYjornales: string;
        resumenesGenerales: string;
    }
    interface IDataAccessService {
        getEmpleadoYobreroDtoResource(): ng.resource.IResourceClass<IEmpleadoYobreroDtoResource>;
        getSueldoYjornaleDtoResource(): ng.resource.IResourceClass<ISueldoYjornaleDtoResource>;
        getResumenGeneralDtoResource(): ng.resource.IResourceClass<IResumenGeneralDtoResource>;
    }

    interface IEmpleadoYobreroDtoResource extends ng.resource.IResource<app.dto.IEmpleadoYobreroDto> { }
    interface ISueldoYjornaleDtoResource extends ng.resource.IResource<app.dto.ISueldoYjornaleDto> { }
    interface IResumenGeneralDtoResource extends ng.resource.IResource<app.dto.IResumenGeneralDto> { }

    export class DataAccessService implements IDataAccessService {

        static $inject = ["$resource", "syjPath"];
        constructor(private $resource: ng.resource.IResourceService,
            private syjPath: ISyjPath) { }

        getEmpleadoYobreroDtoResource(): ng.resource.IResourceClass<IEmpleadoYobreroDtoResource> {            
            return this.$resource(this.syjPath.empleadosYobreros);
        }
        getSueldoYjornaleDtoResource(): ng.resource.IResourceClass<ISueldoYjornaleDtoResource> {            
            return this.$resource(this.syjPath.sueldosYjornales);
        }
        getResumenGeneralDtoResource(): ng.resource.IResourceClass<IResumenGeneralDtoResource> {
            return this.$resource(this.syjPath.resumenesGenerales);
        }
    }
    angular
        .module("common.services")
        .service("dataAccessService",
        DataAccessService);

}