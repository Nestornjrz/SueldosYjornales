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
    
    public partial class Anticipos
    {
        public long AnticipoID { get; set; }
        public long EmpleadoID { get; set; }
        public System.DateTime FechaAnticipo { get; set; }
        public decimal MontoAnticipo { get; set; }
        public string Observacion { get; set; }
        public long UsuarioID { get; set; }
        public System.DateTime MomentoCarga { get; set; }
        public Nullable<bool> DevitoGeneradoSn { get; set; }
    
        public virtual Empleado Empleado { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
