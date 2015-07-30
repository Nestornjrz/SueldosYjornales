using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class AnticiposManagers {
        public List<AnticipoDto> ListadoAnticipos(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Anticipos
                    .Where(a => a.EmpleadoID == empleadoID)
                    .Select(s => new AnticipoDto() {
                        AnticipoID = s.AnticipoID,
                        EmpleadoID = s.EmpleadoID,
                        FechaAnticipo = s.FechaAnticipo,
                        MontoAnticipo = s.MontoAnticipo,
                        Observacion = s.Observacion
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarAnticipo(AnticipoDto aDto, Guid userID) {
            if (aDto.AnticipoID > 0) {
                return EditarAnticipo(aDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var anticipoDb = new Anticipos();
                anticipoDb.EmpleadoID = aDto.EmpleadoID;
                anticipoDb.FechaAnticipo = aDto.FechaAnticipo;
                anticipoDb.MontoAnticipo = aDto.MontoAnticipo;
                anticipoDb.Observacion = aDto.Observacion;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                anticipoDb.UsuarioID = usuarioID;
                anticipoDb.MomentoCarga = DateTime.Now;

                context.Anticipos.Add(anticipoDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                aDto.AnticipoID = anticipoDb.AnticipoID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el anticipo : " + aDto.AnticipoID,
                    ObjetoDto = aDto
                };
            }
        }

        private static MensajeDto EditarAnticipo(AnticipoDto aDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var anticipoDb = context.Anticipos
                    .Where(a => a.AnticipoID == aDto.AnticipoID)
                    .FirstOrDefault();
                if (anticipoDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el anticipo : " + aDto.AnticipoID
                    };
                }
                anticipoDb.FechaAnticipo = aDto.FechaAnticipo;
                anticipoDb.MontoAnticipo = aDto.MontoAnticipo;
                anticipoDb.Observacion = aDto.Observacion;

                context.Entry(anticipoDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el anticipo: " + aDto.AnticipoID,
                    ObjetoDto = aDto
                };
            }
        }

        public MensajeDto EliminarAnticipo(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var anticipoDb = context.Anticipos
                   .Where(a => a.AnticipoID == id)
                   .FirstOrDefault();
                if (anticipoDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el anticipo : " + id
                    };
                }
                context.Anticipos.Remove(anticipoDb);
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el anticipo : " + id
                };
            }
        }
    }
}
