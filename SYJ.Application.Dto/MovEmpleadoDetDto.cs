using System;

namespace SYJ.Application.Dto {
    public class MovEmpleadoDetDto {
        public long MovEmpleadoDetID { get; set; }
        public long MovEmpleadoID { get; set; }
        public EmpleadoDto Empleado { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
        public DateTime MesAplicacion { get; set; }
        public LiquidacionConceptoDto LiquidacionConcepto { get; set; }
        public string Observacion { get; set; }
        #region Auxiliar
        /// <summary>
        /// Se utiliza para mostrar el saldo en listado de movimientos, cuando se agrupa por mes
        /// o cualquier otra agrupacion
        /// </summary>
        public decimal Saldo { get; set; }
        /// <summary>
        /// Es el id de la cabecera de los movimientos del empleado pero este movimiento
        /// tiene que ver con la liquidacion de salario, por eso si se encuentra una liquidacion
        /// de salario para el mes de esta cuota se coloca aqui.
        /// </summary>
        public long MovEmpleadoIDdeLaLiquidacion { get; set; }
        #endregion
    }
}
