//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SYJ.Domain.Db
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comisione
    {
        public long ComisionID { get; set; }
        public long EmpleadoID { get; set; }
        public System.DateTime FechaComision { get; set; }
        public decimal MontoComision { get; set; }
        public string Observacion { get; set; }
        public long UsuarioID { get; set; }
        public System.DateTime MomentoCarga { get; set; }
        public Nullable<bool> CreditoGeneradoSn { get; set; }
    
        public virtual Empleado Empleado { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
