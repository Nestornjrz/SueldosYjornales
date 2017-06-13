import { EmpleadoDto } from "app/dtos/empleado-dto";
import { LiquidacionConceptoDto } from "app/dtos/liquidacion-concepto-dto";

export class MovEmpleadoDetDto {
    movEmpleadoDetID: number;
    movEmpleadoID: number;
    empleado: EmpleadoDto;
    debito: number;
    credito: number;
    mesAplicacion: Date;
    liquidacionConcepto: LiquidacionConceptoDto;
    observacion: string;
    //#region Auxiliar
    /// <summary>
    /// Se utiliza para mostrar el saldo en listado de movimientos, cuando se agrupa por mes
    /// o cualquier otra agrupacion
    /// </summary>
    saldo: number;
    /// <summary>
    /// Es el id de la cabecera de los movimientos del empleado pero este movimiento
    /// tiene que ver con la liquidacion de salario, por eso si se encuentra una liquidacion
    /// de salario para el mes de esta cuota se coloca aqui.
    /// </summary>
    movEmpleadoIDdeLaLiquidacion: number;
    //#endregion
}
