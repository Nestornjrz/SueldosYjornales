using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers.Auxiliares {
    /// <summary>
    /// Manejo aqui los conceptos y el devito y el credito para 
    /// no fallar
    /// </summary>
    public static class Liquidacion {
        public static class DevCred {
            public static bool Devito = true;
            public static bool Credito = false;
        }
        public enum Conceptos {
            SueldoBase = 1,
            Comision = 2,
            Anticipo = 3,
            Prestamo = 4,
            /// <summary>
            /// Es el total pagado al fin del mes, es decir
            /// el SUELDO
            /// </summary>
            TotalPagado = 5,
            Ips = 6,
            Aguinaldo = 7
        }
    }
}
