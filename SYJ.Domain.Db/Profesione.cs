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
    
    public partial class Profesione
    {
        public Profesione()
        {
            this.Empleados = new HashSet<Empleado>();
        }
    
        public int ProfesionID { get; set; }
        public string NombreProfesion { get; set; }
        public string Abreviatura { get; set; }
        public string Descripcion { get; set; }
    
        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}