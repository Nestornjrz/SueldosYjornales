using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class AnticipoDto {
        public long AnticipoID { get; set; }
        public long EmpleadoID { get; set; }
        public System.DateTime FechaAnticipo { get; set; }
        public decimal MontoAnticipo { get; set; }
        public string Observacion { get; set; }
    }
}
