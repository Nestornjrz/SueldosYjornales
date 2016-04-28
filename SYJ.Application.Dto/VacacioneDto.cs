using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class VacacioneDto {
        public long VacacionID { get; set; }
        public long EmpleadoID { get; set; }
        public System.DateTime FechaSalida { get; set; }
        public int DiasUsufructuados { get; set; }
        public string Observacion { get; set; }
    }
}
