using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class MovEmpleadoDetDto {
        public long MovEmpleadoDetID { get; set; }
        public long MovEmpleadoID { get; set; }
        public EmpleadoDto Empleado { get; set; }
        public decimal Devito { get; set; }
        public decimal Credito { get; set; } 
        public DateTime MesAplicacion { get; set; }
        public LiquidacionConceptoDto LiquidacionConcepto { get; set; }
        public string Observacion { get; set; }
    }
}
