using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class EmpleadoDto {
        public long EmpleadoID { get; set; }
        public SucursaleDto Sucursale { get; set; }
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
    }
}
