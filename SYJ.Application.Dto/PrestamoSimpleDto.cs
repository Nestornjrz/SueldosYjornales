using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class PrestamoSimpleDto {
        public long PrestamoSimpleID { get; set; }
        public System.DateTime Fecha1erVencimiento { get; set; }
        public long EmpleadoID { get; set; }
        public decimal Monto { get; set; }
        public int Cuotas { get; set; }
        public string Observacion { get; set; }
    }
}
