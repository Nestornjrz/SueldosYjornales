﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SueldosJornalesEntities : DbContext
    {
        public SueldosJornalesEntities()
            : base("name=SueldosJornalesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cargo> Cargos { get; set; }
        public virtual DbSet<Empleado> Empleados { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<EstadoCivile> EstadoCiviles { get; set; }
        public virtual DbSet<HistoricoDireccione> HistoricoDirecciones { get; set; }
        public virtual DbSet<HistoricoIngresoSalida> HistoricoIngresoSalidas { get; set; }
        public virtual DbSet<HistoricoSalario> HistoricoSalarios { get; set; }
        public virtual DbSet<HistoricosHorario> HistoricosHorarios { get; set; }
        public virtual DbSet<HistoricoTelefono> HistoricoTelefonos { get; set; }
        public virtual DbSet<Nacionalidade> Nacionalidades { get; set; }
        public virtual DbSet<Profesione> Profesiones { get; set; }
        public virtual DbSet<Sucursale> Sucursales { get; set; }
        public virtual DbSet<UbicacionSucUsuario> UbicacionSucUsuarios { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
    }
}
