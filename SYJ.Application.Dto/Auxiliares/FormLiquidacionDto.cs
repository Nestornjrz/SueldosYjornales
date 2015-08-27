using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto.Auxiliares {
    public class FormLiquidacionDto {
        public MesDto Mes { get; set; }
        public int Year { get; set; }
        public int[] EmpleadosSeleccionados { get; set; }
    }
}
