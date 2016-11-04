using System;

namespace SYJ.Application.Dto {
    public class EmpleadoDto {
        public long EmpleadoID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public SexoDto Sexo { get; set; }
        public int NroCedula { get; set; }
        public EstadoCivileDto EstadoCivile { get; set; }
        public NacionalidadeDto Nacionalidade { get; set; }
        public Nullable<int> NumeroIps { get; set; }
        public Nullable<int> NumeroMjt { get; set; }
        public ProfesioneDto Profesione { get; set; }
        public int CantidadHijos { get; set; }
        #region auxiliares
        public SucursaleDto Sucursale { get; set; }
        public CargoDto Cargo { get; set; }
        public bool TieneIpsSn { get; set; }
        /// <summary>
        /// Propiedad utilizada para determinar si el empleado esta activo
        /// o si ya salio de la empresa
        /// </summary>
        public bool Activo { get; set; }
        #endregion
    }
}
