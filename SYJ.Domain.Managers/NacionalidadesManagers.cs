using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class NacionalidadesManagers {

        public List<NacionalidadeDto> ListadoNacionalidades() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Nacionalidades
                    .Select(n => new NacionalidadeDto() {
                        NacionalidadID = n.NacionalidadID,
                        NombreNacionalidad = n.NombreNacionalidad,
                        Abreviatura = n.Abreviatura
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarNacionalidad(NacionalidadeDto nDto) {
            using (var context = new SueldosJornalesEntities()) {
                if (context.Nacionalidades.Where(n => n.NacionalidadID == nDto.NacionalidadID).Count() > 0) {
                    return EditarNacionalidad(nDto);
                }
                MensajeDto mensajeDto = null;
                var nacionalidadeDb = new Nacionalidade();
                nacionalidadeDb.NacionalidadID = nDto.NacionalidadID;
                nacionalidadeDb.NombreNacionalidad = nDto.NombreNacionalidad;
                nacionalidadeDb.Abreviatura = nDto.Abreviatura;

                context.Nacionalidades.Add(nacionalidadeDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                nDto.NacionalidadID = nacionalidadeDb.NacionalidadID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo la nacionalidad : " + nDto.NacionalidadID,
                    ObjetoDto = nDto
                };
            }
        }

        private MensajeDto EditarNacionalidad(NacionalidadeDto nDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var nacionalidadeDb = context.Nacionalidades
                    .Where(n => n.NacionalidadID == nDto.NacionalidadID)
                    .First();
                nacionalidadeDb.NombreNacionalidad = nDto.NombreNacionalidad;
                nacionalidadeDb.Abreviatura = nDto.Abreviatura;

                context.Entry(nacionalidadeDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito la nacionalidad : " + nDto.NacionalidadID,
                    ObjetoDto = nDto
                };
            }
        }

        public MensajeDto EliminarNacionalidad(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var nacionalidadeDb = context.Nacionalidades
                   .Where(n => n.NacionalidadID == id)
                   .First();

                if (nacionalidadeDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "La nacionalidad ID : " + id + " no existe en la base de datos"
                    };
                }

                context.Nacionalidades.Remove(nacionalidadeDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino la nacionalidad : " + id
                };
            }
        }
    }
}
