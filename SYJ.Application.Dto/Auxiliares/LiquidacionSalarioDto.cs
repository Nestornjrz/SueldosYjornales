using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto.Auxiliares {
    public class LiquidacionSalarioDto {
        public EmpleadoDto Empleado { get; set; }
        public int DiasTrabajados { get; set; }
        public Decimal SalarioBase { get; set; }
        public Decimal SubTotalIngresos { get; set; }
        public Decimal Comisiones { get; set; }
        public Decimal TotalIngreso { get; set; }
        public Decimal DescIPS { get; set; }
        public Decimal DescOtros { get; set; }
        public Decimal TotalDescuentos { get; set; }
        public Decimal NetoAcobrar { get; set; }
        public DateTime Periodo { get; set; }
        public DateTime UltimoDiaPeriodo { get; set; }
        public string MensajeCalculos { get; set; }
    }
}
