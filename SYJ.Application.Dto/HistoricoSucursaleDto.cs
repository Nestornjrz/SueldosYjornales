using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class HistoricoSucursaleDto {
        public long HistoricoSucursalID { get; set; }
        public long EmpleadoID { get; set; }
        public SucursaleDto Sucursal { get; set; }
    }
}
