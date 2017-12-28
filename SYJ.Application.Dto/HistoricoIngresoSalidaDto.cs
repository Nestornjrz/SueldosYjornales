using System;

namespace SYJ.Application.Dto {
    public class HistoricoIngresoSalidaDto {
        public long HistoricoIngresoSalidaID { get; set; }
        public long EmpleadoID { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public Nullable<System.DateTime> FechaSalida { get; set; }
        public string MotivoSalida { get; set; }
        public string MotivoIngreso { get; set; }
        public ConceptosIngreEgreDto ConceptosIngreEgre { get; set; }
        public System.DateTime MomentoCarga { get; set; }
    }
}
