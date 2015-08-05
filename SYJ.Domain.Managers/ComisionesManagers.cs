using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class ComisionesManagers {
        public List<ComisioneDto> ListadoHistoricoHorarios(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Comisiones
                    .Where(c => c.EmpleadoID == empleadoID)
                    .Select(s => new ComisioneDto() {
                        ComisionID = s.ComisionID,
                        EmpleadoID = s.EmpleadoID,
                        FechaComision = s.FechaComision,
                        MontoComision = s.MontoComision,
                        Observacion = s.Observacion
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarComision(ComisioneDto cDto, Guid userID) {
            if (cDto.ComisionID > 0) {
                return EditarComision(cDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var comisioneDb = new Comisione();
                comisioneDb.EmpleadoID = cDto.EmpleadoID;
                comisioneDb.FechaComision = cDto.FechaComision;
                comisioneDb.MontoComision = cDto.MontoComision;
                comisioneDb.Observacion = cDto.Observacion;
                comisioneDb.MomentoCarga = DateTime.Now;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                comisioneDb.UsuarioID = usuarioID;

                context.Comisiones.Add(comisioneDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                cDto.ComisionID = comisioneDb.ComisionID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el historico de horario : " + cDto.ComisionID,
                    ObjetoDto = cDto
                };
            }
        }

        private static MensajeDto EditarComision(ComisioneDto cDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var comisioneDb = context.Comisiones
                    .Where(c => c.ComisionID == cDto.ComisionID)
                    .FirstOrDefault();
                if (comisioneDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe la comision : " + cDto.ComisionID
                    };
                }
                comisioneDb.FechaComision = cDto.FechaComision;
                comisioneDb.MontoComision = cDto.MontoComision;
                comisioneDb.Observacion = cDto.Observacion;

                context.Entry(comisioneDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito la comision: " + cDto.ComisionID,
                    ObjetoDto = cDto
                };
            }
        }

        public MensajeDto EliminarComision(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var comisioneDb = context.Comisiones
                    .Where(c => c.ComisionID == id)
                    .FirstOrDefault();
                if (comisioneDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe la comision : " + id
                    };
                }

                context.Comisiones.Remove(comisioneDb);
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino la comision : " + id
                };
            }
        }

        public List<ComisioneDto> ListadoUltimo2meses(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var dosMesesAtras = DateTime.Today.AddMonths(-2);
                var listado = context.Comisiones
                   .Where(c => c.EmpleadoID == empleadoID &&
                               c.FechaComision >= dosMesesAtras)
                   .Select(s => new ComisioneDto() {
                       ComisionID = s.ComisionID,
                       EmpleadoID = s.EmpleadoID,
                       FechaComision = s.FechaComision,
                       MontoComision = s.MontoComision,
                       Observacion = s.Observacion
                   }).ToList();
                return listado;
            }
        }
        public List<ComisioneDto> ListadoSegunMesYanosYempleado(long empleadoID, int mesID, int year) {
            using (var context = new SueldosJornalesEntities()) {
                var dosMesesAtras = DateTime.Today.AddMonths(-2);
                var listado = context.Comisiones
                   .Where(c => c.EmpleadoID == empleadoID &&
                               c.FechaComision.Month == mesID &&
                               c.FechaComision.Year == year)
                   .Select(s => new ComisioneDto() {
                       ComisionID = s.ComisionID,
                       EmpleadoID = s.EmpleadoID,
                       FechaComision = s.FechaComision,
                       MontoComision = s.MontoComision,
                       Observacion = s.Observacion
                   }).ToList();
                return listado;
            }
        }
    }
}
