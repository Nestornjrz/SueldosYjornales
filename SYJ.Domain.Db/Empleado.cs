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
    
    public partial class Empleado
    {
        public Empleado()
        {
            this.HistoricoDirecciones = new HashSet<HistoricoDireccione>();
            this.HistoricoSalarios = new HashSet<HistoricoSalario>();
            this.HistoricoSucursales = new HashSet<HistoricoSucursale>();
            this.HistoricoTelefonos = new HashSet<HistoricoTelefono>();
            this.HistoricoIngresoSalidas = new HashSet<HistoricoIngresoSalida>();
            this.Anticipos = new HashSet<Anticipos>();
            this.Ausencias = new HashSet<Ausencia>();
            this.Comisiones = new HashSet<Comisione>();
            this.HistoricoHorarios = new HashSet<HistoricoHorario>();
        }
    
        public long EmpleadoID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public int Sexo { get; set; }
        public int NroCedula { get; set; }
        public int EstadoCivilID { get; set; }
        public int NacionalidadID { get; set; }
        public Nullable<int> NumeroIps { get; set; }
        public Nullable<int> NumeroMjt { get; set; }
        public int ProfesionID { get; set; }
        public int CantidadHijos { get; set; }
        public long UsuarioID { get; set; }
        public System.DateTime MomentoCarga { get; set; }
    
        public virtual EstadoCivile EstadoCivile { get; set; }
        public virtual Nacionalidade Nacionalidade { get; set; }
        public virtual Profesione Profesione { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<HistoricoDireccione> HistoricoDirecciones { get; set; }
        public virtual ICollection<HistoricoSalario> HistoricoSalarios { get; set; }
        public virtual ICollection<HistoricoSucursale> HistoricoSucursales { get; set; }
        public virtual ICollection<HistoricoTelefono> HistoricoTelefonos { get; set; }
        public virtual ICollection<HistoricoIngresoSalida> HistoricoIngresoSalidas { get; set; }
        public virtual ICollection<Anticipos> Anticipos { get; set; }
        public virtual ICollection<Ausencia> Ausencias { get; set; }
        public virtual ICollection<Comisione> Comisiones { get; set; }
        public virtual ICollection<HistoricoHorario> HistoricoHorarios { get; set; }
    }
}
