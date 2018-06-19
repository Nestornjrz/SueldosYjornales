import { SexoDto } from 'src/app/dtos/sexo-dto';
import { EstadoCivileDto } from 'src/app/dtos/estado-civile-dto';
import { NacionalidadeDto } from 'src/app/dtos/nacionalidade-dto';
import { ProfesioneDto } from 'src/app/dtos/profesione-dto';

export class EmpleadoDto {
    constructor() {
        this.empleadoID = 0;
    }
    empleadoID: number;
    nombres: string;
    apellidos: string;
    fechaNacimiento: Date;
    sexo: SexoDto;
    nroCedula: number;
    estadoCivile: EstadoCivileDto;
    nacionalidade: NacionalidadeDto;
    profesione: ProfesioneDto;
    cantidadHijos: number;
}
