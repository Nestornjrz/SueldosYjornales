using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class HistoricoTelefonoDto {
        public long HistoricoTelefonoID { get; set; }
        public long EmpleadoID { get; set; }
        public string Telefonos { get; set; }
    }
}
