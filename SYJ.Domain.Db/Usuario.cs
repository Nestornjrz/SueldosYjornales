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
    
    public partial class Usuario
    {
        public Usuario()
        {
            this.Anticipos = new HashSet<Anticipos>();
            this.Ausencias = new HashSet<Ausencia>();
            this.Comisiones = new HashSet<Comisione>();
            this.Empleados = new HashSet<Empleado>();
            this.HistoricoDirecciones = new HashSet<HistoricoDireccione>();
            this.HistoricoHorarios = new HashSet<HistoricoHorario>();
            this.HistoricoIngresoSalidas = new HashSet<HistoricoIngresoSalida>();
            this.HistoricoSalarios = new HashSet<HistoricoSalario>();
            this.HistoricoSucursales = new HashSet<HistoricoSucursale>();
            this.HistoricoTelefonos = new HashSet<HistoricoTelefono>();
            this.Imagenes = new HashSet<Imagene>();
            this.PrestamosSimples = new HashSet<PrestamosSimple>();
            this.Vacaciones = new HashSet<Vacacione>();
        }
    
        public long UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public System.Guid UserID { get; set; }
        public string CorreoElectronico { get; set; }
    
        public virtual ICollection<Anticipos> Anticipos { get; set; }
        public virtual ICollection<Ausencia> Ausencias { get; set; }
        public virtual ICollection<Comisione> Comisiones { get; set; }
        public virtual ICollection<Empleado> Empleados { get; set; }
        public virtual ICollection<HistoricoDireccione> HistoricoDirecciones { get; set; }
        public virtual ICollection<HistoricoHorario> HistoricoHorarios { get; set; }
        public virtual ICollection<HistoricoIngresoSalida> HistoricoIngresoSalidas { get; set; }
        public virtual ICollection<HistoricoSalario> HistoricoSalarios { get; set; }
        public virtual ICollection<HistoricoSucursale> HistoricoSucursales { get; set; }
        public virtual ICollection<HistoricoTelefono> HistoricoTelefonos { get; set; }
        public virtual ICollection<Imagene> Imagenes { get; set; }
        public virtual ICollection<PrestamosSimple> PrestamosSimples { get; set; }
        public virtual UbicacionSucUsuario UbicacionSucUsuario { get; set; }
        public virtual ICollection<Vacacione> Vacaciones { get; set; }
    }
}
