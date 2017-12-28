using SYJ.Application.Dto;
using SYJ.Domain.Db;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class ConceptosIngreEgresManagers {
        public async Task<List<ConceptosIngreEgreDto>> Listado() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = await context.ConceptosIngreEgres
                    .Select(s => new ConceptosIngreEgreDto() {
                        ConceptoIngreEgreID = s.ConceptoIngreEgreID,
                        Concepto = s.Concepto
                    }).ToListAsync();
                return listado;
            }
        }
    }
}
