using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class HistoricoIngresoSalidasManagers {
        public List<HistoricoIngresoSalidaDto> ListadoHistoricoIngresoSalidas(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.HistoricoIngresoSalidas
                    .Where(h => h.EmpleadoID == empleadoID)
                    .OrderByDescending(h=>h.MomentoCarga)
                    .Select(s => new HistoricoIngresoSalidaDto() {
                        HistoricoIngresoSalidaID = s.HistoricoIngresoSalidaID,
                        EmpleadoID = s.EmpleadoID,
                        FechaIngreso = s.FechaIngreso,
                        FechaSalida = s.FechaSalida,
                        MomentoCarga = s.MomentoCarga
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarHistoricoIngresoSalida(HistoricoIngresoSalidaDto hisDto, Guid userID) {
            if (hisDto.HistoricoIngresoSalidaID > 0) {
                return EditarHistoricoIngresoSalida(hisDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoIngresoSalidaDb = new HistoricoIngresoSalida();
                historicoIngresoSalidaDb.EmpleadoID = hisDto.EmpleadoID;
                historicoIngresoSalidaDb.FechaIngreso = hisDto.FechaIngreso;
                historicoIngresoSalidaDb.FechaSalida = hisDto.FechaSalida;
                historicoIngresoSalidaDb.MomentoCarga = DateTime.Now;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                historicoIngresoSalidaDb.UsuarioID = usuarioID;

                context.HistoricoIngresoSalidas.Add(historicoIngresoSalidaDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                hisDto.HistoricoIngresoSalidaID = historicoIngresoSalidaDb.HistoricoIngresoSalidaID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el historico de ingreso egreso : " + hisDto.HistoricoIngresoSalidaID,
                    ObjetoDto = hisDto
                };
            }
        }

        public MensajeDto EliminarHistoricoSalario(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoIngresoSalidaDb = context.HistoricoIngresoSalidas
                   .Where(h => h.HistoricoIngresoSalidaID == id)
                   .FirstOrDefault();
                if (historicoIngresoSalidaDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de ingreso salida : "
                        + id
                    };
                }
                context.HistoricoIngresoSalidas.Remove(historicoIngresoSalidaDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el historico de ingreso/salida : " + id
                };
            }
        }

        private MensajeDto EditarHistoricoIngresoSalida(HistoricoIngresoSalidaDto hisDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoIngresoSalidaDb = context.HistoricoIngresoSalidas
                    .Where(h => h.HistoricoIngresoSalidaID == hisDto.HistoricoIngresoSalidaID)
                    .FirstOrDefault();
                if (historicoIngresoSalidaDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de ingreso salida : "
                        + hisDto.HistoricoIngresoSalidaID
                    };
                }
                historicoIngresoSalidaDb.FechaIngreso = hisDto.FechaIngreso;
                historicoIngresoSalidaDb.FechaSalida = hisDto.FechaSalida;

                context.Entry(historicoIngresoSalidaDb).State = System.Data.Entity.EntityState.Modified;

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el historico de Ingreso/Salida : " + hisDto.HistoricoIngresoSalidaID,
                    ObjetoDto = hisDto
                };
            }
        }

        public MensajeDto UltimoIngreso(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var ingresoSalidaDb = context.HistoricoIngresoSalidas
                    .Where(h => h.FechaSalida == null && h.EmpleadoID == empleadoID)
                    .OrderByDescending(h => h.FechaIngreso)
                    .FirstOrDefault();
                if (ingresoSalidaDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existen datos de Ingreso y egreso"
                    };
                }
                var hismDto = new HistoricoIngresoSalidaDto();
                hismDto.EmpleadoID = ingresoSalidaDb.EmpleadoID;
                hismDto.FechaIngreso = ingresoSalidaDb.FechaIngreso;
                hismDto.FechaSalida = ingresoSalidaDb.FechaSalida;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Ultimo dato de ingreso a la empresa",
                    ObjetoDto = hismDto
                };
            }
        }
        public static bool EmpleadoTrabajaTodaviaEnLaEmpresa(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                //Se busca el ultimo movimiento del empleado
                var historico = context.HistoricoIngresoSalidas
                    .Where(h => h.EmpleadoID == empleadoID)
                    .OrderByDescending(h => h.MomentoCarga)
                    .FirstOrDefault();
                if (historico == null) {
                    return true;
                }
                //Se ve si el ultimo historico tiene fecha de salida.
                if (historico.FechaSalida == null) {
                    return true;
                } else {
                    //Se ve si salio dentro del mes actual, si es asi se considera que todavia trabaja
                    if (historico.FechaSalida.Value.Year == DateTime.Now.Year &&
                        historico.FechaSalida.Value.Month == DateTime.Now.Month) {
                            return true;
                    }
                    //Se ve si la fecha de salida esta todavia en el futuro o ya paso
                    if (historico.FechaSalida > DateTime.Now) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }
        public static DateTime? fechaSalida(long empleadoID, DateTime fechaDondeSeEsta) {
            using (var context = new SueldosJornalesEntities()) {
                //Se busca el ultimo movimiento del empleado
                var historico = context.HistoricoIngresoSalidas
                    .Where(h => h.EmpleadoID == empleadoID &&
                           h.FechaSalida < fechaDondeSeEsta)
                    .OrderByDescending(h => h.MomentoCarga)
                    .FirstOrDefault();
                if (historico == null) {
                    return null;
                }
                return historico.FechaSalida.Value;
            }
        }
    }
}
