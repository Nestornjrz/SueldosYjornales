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
    
    public partial class MovEmpleadosDet
    {
        public long MovEmpleadoDetID { get; set; }
        public long MovEmpleadoID { get; set; }
        public long EmpleadoID { get; set; }
        public bool DevCred { get; set; }
        public decimal Monto { get; set; }
        public System.DateTime MesAplicacion { get; set; }
        public int LiquidacionConceptoID { get; set; }
    
        public virtual Empleado Empleado { get; set; }
        public virtual LiquidacionConcepto LiquidacionConcepto { get; set; }
        public virtual MovEmpleado MovEmpleado { get; set; }
    }
}
