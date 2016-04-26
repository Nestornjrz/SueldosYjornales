module app.common {
    interface ISyjPath {
        empleadosYobreros: string;
    }
    interface IDataAccessService {
        getEmpleadoYobreroDtoResource(): ng.resource.IResourceClass<IEmpleadoYobreroDtoResource>;
    }

    interface IEmpleadoYobreroDtoResource extends ng.resource.IResource<app.dto.IEmpleadoYobreroDto> { }

    export class DataAccessService implements IDataAccessService {

        static $inject = ["$resource", "syjPath"];
        constructor(private $resource: ng.resource.IResourceService,
            private syjPath: ISyjPath) { }

        getEmpleadoYobreroDtoResource(): ng.resource.IResourceClass<IEmpleadoYobreroDtoResource> {
            //return this.$resource("/GerenciaAlbor/api/Contabilidad/ContabilidadTreeMap/:Id");
            return this.$resource(this.syjPath.empleadosYobreros);
        }
    }
    angular
        .module("common.services")
        .service("dataAccessService",
        DataAccessService);

}