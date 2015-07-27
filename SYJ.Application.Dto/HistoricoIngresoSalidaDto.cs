using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class HistoricoIngresoSalidaDto {
        public long HistoricoIngresoSalidaID { get; set; }
        public long EmpleadoID { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public Nullable<System.DateTime> FechaSalida { get; set; }       
    }
}
