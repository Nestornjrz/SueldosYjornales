using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class MovEmpleadoDto {
        public long MovEmpleadoID { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string Descripcion { get; set; }
        public List<MovEmpleadoDetDto> MovEmpleadosDets { get; set; }
    }
}
