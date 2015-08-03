using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto.Auxiliares {
    public class LiquidacionSueldoFormDto {
        public MesDto Mes { get; set; }
        public int Year { get; set; }
        public EmpresaDto Empresa { get; set; }
        public SucursaleDto Sucursal { get; set; }
    }
}
