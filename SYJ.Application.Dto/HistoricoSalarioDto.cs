using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class HistoricoSalarioDto {
        public long HistoricoSalarioID { get; set; }
        public long EmpleadoID { get; set; }
        public decimal Monto { get; set; }
        public CargoDto Cargo { get; set; }
        public string Observacion { get; set; }
        public System.DateTime FechaSalario { get; set; }
        public bool Ips_Sn { get; set; }
    }
}
