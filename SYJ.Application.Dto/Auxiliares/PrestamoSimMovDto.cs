using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto.Auxiliares {
    /// <summary>
    /// Dto que engloba El pretamo y el detalle del prestamo que esta en los movimientos.
    /// </summary>
    public class PrestamoSimMovDto {
        public PrestamoSimpleDto PrestamoSimple { get; set; }
        public MovEmpleadoDto MovimientoEmpleado { get; set; }
    }
}
