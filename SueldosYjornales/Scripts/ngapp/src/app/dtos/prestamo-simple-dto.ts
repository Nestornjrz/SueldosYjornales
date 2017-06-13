import { EmpleadoDto } from "app/dtos/empleado-dto";
import { MovEmpleadoDetDto } from "app/dtos/mov-empleado-det-dto";

export class PrestamoSimpleDto {
  prestamoSimpleID: number;
  fecha1erVencimiento: Date;
  empleadoID: number;
  empleado: EmpleadoDto;
  monto: number;
  cuotas: number;
  observacion: string
  movEmpleadoID: number;

  /// <summary>
  /// Se colocan los devitos de los prestamos simples
  /// </summary>
  cuotasMov: MovEmpleadoDetDto[];
  /// <summary>
  /// Es la suma del monto de las cuotas, para presentarlo en la interfaz
  /// </summary>
  sumaMontoCuotas: number;

}
