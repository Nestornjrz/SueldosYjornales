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
    
    public partial class LiquidacionConcepto
    {
        public LiquidacionConcepto()
        {
            this.MovEmpleadosDets = new HashSet<MovEmpleadosDet>();
        }
    
        public int LiquidacionConceptoID { get; set; }
        public string NombreConcepto { get; set; }
    
        public virtual ICollection<MovEmpleadosDet> MovEmpleadosDets { get; set; }
    }
}
