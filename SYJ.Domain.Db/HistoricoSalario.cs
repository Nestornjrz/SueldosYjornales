//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SYJ.Domain.Db
{
    using System;
    using System.Collections.Generic;
    
    public partial class HistoricoSalario
    {
        public long HistoricoSalarioID { get; set; }
        public long EmpleadoID { get; set; }
        public decimal Monto { get; set; }
        public int CargoID { get; set; }
        public string Observacion { get; set; }
        public System.DateTime FechaSalario { get; set; }
        public long UsuarioID { get; set; }
        public System.DateTime MomentoCarga { get; set; }
        public bool Ips_Sn { get; set; }
    
        public virtual Cargo Cargo { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
