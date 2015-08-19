using SYJ.Application.Dto;
using SYJ.Domain.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class TipoImagenesManagers {
        public List<TipoImageneDto> ListadoTipoImagenes() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.TipoImagenes
                    .Select(s => new TipoImageneDto() {
                        TipoImagenID = s.TipoImagenID,
                        NombreTipoImagen = s.NombreTipoImagen
                    })
                    .ToList();
                return listado;
            }
        }
    }
}
