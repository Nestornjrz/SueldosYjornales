using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class ComisioneDto {
        public long ComisionID { get; set; }
        public long EmpleadoID { get; set; }
        public System.DateTime FechaComision { get; set; }
        public decimal MontoComision { get; set; }
        public string Observacion { get; set; }
    }
}
