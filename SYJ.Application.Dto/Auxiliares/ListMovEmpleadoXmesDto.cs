using System;
using System.Collections.Generic;

namespace SYJ.Application.Dto.Auxiliares {
    public class MovimientosEmpleadoXmesDto {
        public DateTime MesAplicacion { get; set; }
        public MovEmpleadoDto CabeceraLiquidacionSalario { get; set; }
        public List<MovEmpleadoDetDto> Movimientos { get; set; }
    }
}
