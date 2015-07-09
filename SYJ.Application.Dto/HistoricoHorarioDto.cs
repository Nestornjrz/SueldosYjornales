using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class HistoricoHorarioDto {
        public long HistoricoHorarioID { get; set; }
        public long EmpleadoID { get; set; }
        public Nullable<System.DateTime> HoraEntradaManana { get; set; }
        public Nullable<System.DateTime> HoraSalidaManana { get; set; }
        public Nullable<System.DateTime> HoraEntradaTarde { get; set; }
        public Nullable<System.DateTime> HoraSalidaTarde { get; set; }
        public Nullable<System.DateTime> HoraEntradaNoche { get; set; }
        public Nullable<System.DateTime> HoraSalidaNoche { get; set; }
    }
}
