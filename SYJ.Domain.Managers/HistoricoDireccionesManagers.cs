using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class HistoricoDireccionesManagers {
        public List<HistoricoDireccioneDto> ListadoHistoricoDirecciones() {
            throw new NotImplementedException();
        }

        public MensajeDto CargarHistoricoDirecciones(HistoricoDireccioneDto hdDto, Guid userID) {
            if (hdDto.HistoricoDireccionID > 0) {
                return EditarHistoricoDirecciones(hdDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoDireccioneDb = new HistoricoDireccione();
                historicoDireccioneDb.EmpleadoID = hdDto.EmpleadoID;
                historicoDireccioneDb.Direccion = hdDto.Direccion;
                historicoDireccioneDb.MomentoCarga = DateTime.Now;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                historicoDireccioneDb.UsuarioID = usuarioID;

                context.HistoricoDirecciones.Add(historicoDireccioneDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                hdDto.HistoricoDireccionID = historicoDireccioneDb.HistoricoDireccionID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo la direccion : " + hdDto.HistoricoDireccionID,
                    ObjetoDto = hdDto
                };
            }
        }

        public MensajeDto EliminarHistoricoDireccion(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoDireccioneDb = context.HistoricoDirecciones
                   .Where(h => h.HistoricoDireccionID == id)
                   .FirstOrDefault();
                if (historicoDireccioneDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de direccion numero: " +
                        id
                    };
                }
                context.HistoricoDirecciones.Remove(historicoDireccioneDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el historico de direccion : " + id
                };
            }
        }

        public List<HistoricoDireccioneDto> ListadoHistoricoDirecciones(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.HistoricoDirecciones
                    .Where(h => h.EmpleadoID == empleadoID)
                    .Select(s => new HistoricoDireccioneDto() {
                        HistoricoDireccionID = s.HistoricoDireccionID,
                        Direccion = s.Direccion
                    }).ToList();
                return listado;
            }
        }
        private MensajeDto EditarHistoricoDirecciones(HistoricoDireccioneDto hdDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoDireccioneDb = context.HistoricoDirecciones
                    .Where(h => h.HistoricoDireccionID == hdDto.HistoricoDireccionID)
                    .FirstOrDefault();
                if (historicoDireccioneDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de direccion numero: " + hdDto.HistoricoDireccionID
                    };
                }             
                historicoDireccioneDb.Direccion = hdDto.Direccion;

                context.Entry(historicoDireccioneDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el historico de direccion : " + hdDto.HistoricoDireccionID,
                    ObjetoDto = hdDto
                };
            }
        }

        public MensajeDto DireccionaActual(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var direccionActualDb = context.HistoricoDirecciones
                    .Where(h=>h.EmpleadoID == empleadoID)
                    .OrderByDescending(a => a.MomentoCarga)
                    .FirstOrDefault();
                if (direccionActualDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existen datos de Direccion"
                    };
                }
                var hdDto = new HistoricoDireccioneDto();
                hdDto.HistoricoDireccionID = direccionActualDb.HistoricoDireccionID;
                hdDto.HistoricoDireccionID = direccionActualDb.HistoricoDireccionID;
                hdDto.EmpleadoID = direccionActualDb.EmpleadoID;
                hdDto.Direccion = direccionActualDb.Direccion;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso ="Ultima direccion encontrada",
                    ObjetoDto = hdDto
                };
            }
        }
    }
}
