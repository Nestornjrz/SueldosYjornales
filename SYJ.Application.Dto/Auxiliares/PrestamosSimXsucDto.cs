using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto.Auxiliares {
    /// <summary>
    /// Sirve para presentar los listados de prestamos y movimientos por sucursal
    /// </summary>
    public class PrestamosSimXsucDto {
        public SucursaleDto Sucursal { get; set; }
        public List<PrestamoSimMovDto> PrestamoSimpleMovimiento { get; set; }
    }
}
