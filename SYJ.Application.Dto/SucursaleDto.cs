using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class SucursaleDto {
        public int SucursalID { get; set; }
        public EmpresaDto Empresa { get; set; }
        public string NombreSucursal { get; set; }
        public string Abreviatura { get; set; }
        public string Descripcion { get; set; }
    }
}
