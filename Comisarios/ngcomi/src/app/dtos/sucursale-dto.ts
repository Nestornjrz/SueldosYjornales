import { EmpresaDto } from 'src/app/dtos/empresa-dto';

export class SucursaleDto {
    sucursalID: number;
    empresa: EmpresaDto;
    nombreSucursal: string;
    abreviatura: string;
    descripcion: string;
}
