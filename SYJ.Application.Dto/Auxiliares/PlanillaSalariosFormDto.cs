using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto.Auxiliares {
    /// <summary>
    /// Esta clase esta pensada para desde un formulario web solicitar los salarios
    /// por mes año y por sucursales PERO TAMBIEN se utiliza para cualquier consulta
    /// que se quera hacer por los parametros que esta clase puede sostener en su estructura
    /// </summary>
    public class MesYearEmpresaSucursalesDto {
        public MesDto Mes { get; set; }
        public int Year { get; set; }
        public EmpresaDto Empresa { get; set; }
        public List<SucursaleDto> Sucursales { get; set; }
    }
}
