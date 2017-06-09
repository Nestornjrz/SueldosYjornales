import { SexoDto } from 'app/dtos/sexo-dto';
import { EstadoCivileDto } from 'app/dtos/estado-civile-dto';
import { NacionalidadeDto } from 'app/dtos/nacionalidade-dto';
import { ProfesioneDto } from 'app/dtos/profesione-dto';
import { SucursaleDto } from 'app/dtos/sucursale-dto';
import { CargoDto } from 'app/dtos/cargo-dto';

interface IPermitirAddPropDim {// Con este interfaz se permite que se pueda agregar dinamicamente propiedades
    [key: string]: any
}
export class EmpleadoDto implements IPermitirAddPropDim {
    empleadoID: number;
    nombres: string;
    spellidos: string;
    fechaNacimiento: Date;
    sexo: SexoDto;
    nroCedula: number;
    estadoCivile: EstadoCivileDto;
    nacionalidade: NacionalidadeDto;
    numeroIps: number;
    numeroMjt: number;
    profesione: ProfesioneDto;
    cantidadHijos: number;
    sucursale: SucursaleDto;
    cargo: CargoDto;
    tieneIpsSn: boolean;
    /// <summary>
    /// Propiedad utilizada para determinar si el empleado esta activo
    /// o si ya salio de la empresa
    /// </summary>
    activo: boolean;
    // Propiedad auxiliar
    edad: number;
    // Metodos auxiliares
    static getEdad(fechaNacimiento: any): number {
        const hoy = new Date();
        const cumpleanos = new Date(fechaNacimiento);
        let edad = hoy.getFullYear() - cumpleanos.getFullYear();
        const m = hoy.getMonth() - cumpleanos.getMonth();

        if (m < 0 || (m === 0 && hoy.getDate() < cumpleanos.getDate())) {
            edad--;
        }
        return edad;
    }
}
