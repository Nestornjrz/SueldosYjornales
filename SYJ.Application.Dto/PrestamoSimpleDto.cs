using System;
using System.Collections.Generic;

namespace SYJ.Application.Dto {
    public class PrestamoSimpleDto {
        public long PrestamoSimpleID { get; set; }
        public System.DateTime Fecha1erVencimiento { get; set; }
        public long EmpleadoID { get; set; }
        public EmpleadoDto Empleado { get; set; }
        public decimal Monto { get; set; }
        public int Cuotas { get; set; }
        public string Observacion { get; set; }
        public Nullable<long> MovEmpleadoID { get; set; }
        #region Auxiliar
        /// <summary>
        /// Se colocan los devitos de los prestamos simples
        /// </summary>
        public List<MovEmpleadoDetDto> CuotasMov { get; set; }
        /// <summary>
        /// Es la suma del monto de las cuotas, para presentarlo en la interfaz
        /// </summary>
        public decimal SumaMontoCuotas { get; set; }
        #endregion
    }
}
