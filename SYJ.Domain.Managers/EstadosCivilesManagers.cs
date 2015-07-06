using SYJ.Application.Dto;
using SYJ.Domain.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class EstadosCivilesManagers {
        public List<EstadoCivileDto> ListadoEstadosCiviles() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.EstadoCiviles
                    .Select(s => new EstadoCivileDto() {
                        EstadoCivilID = s.EstadoCivilID,
                        NombreEstadoCivil = s.NombreEstadoCivil
                    }).ToList();
                return listado;
            }
        }
    }
}
