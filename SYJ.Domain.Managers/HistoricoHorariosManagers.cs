using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class HistoricoHorariosManagers {
        public List<HistoricoHorarioDto> ListadoHistoricoHorarios(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.HistoricoHorarios
                    .Where(h => h.EmpleadoID == empleadoID)
                    .Select(s => new HistoricoHorarioDto() {
                        HistoricoHorarioID = s.HistoricoHorarioID,
                        EmpleadoID = s.EmpleadoID,
                        HoraEntradaManana = s.HoraEntradaMananaUTC,
                        HoraSalidaManana = s.HoraSalidaMananaUTC,

                        HoraEntradaTarde = s.HoraEntradaTardeUTC,
                        HoraSalidaTarde = s.HoraSalidaTardeUTC,

                        HoraEntradaNoche = s.HoraEntradaNocheUTC,
                        HoraSalidaNoche = s.HoraSalidaNocheUTC
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto EliminarHistoricoHorario(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoHorarioDb = context.HistoricoHorarios
                 .Where(h => h.HistoricoHorarioID == id)
                 .FirstOrDefault();
                if (historicoHorarioDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de horario : " + id
                    };
                }
                context.HistoricoHorarios.Remove(historicoHorarioDb);
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el historico de horario : " + id
                };
            }
        }

        public MensajeDto CargarHistoricoHorario(HistoricoHorarioDto hhDto, Guid userID) {
            if (hhDto.HistoricoHorarioID > 0) {
                return EditarHistoricoHorario(hhDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoHorarioDb = new HistoricoHorario();
                historicoHorarioDb.EmpleadoID = hhDto.EmpleadoID;
                if (hhDto.HoraEntradaManana == null &&
                    hhDto.HoraSalidaManana == null &&
                    hhDto.HoraEntradaTarde == null &&
                    hhDto.HoraSalidaTarde == null &&
                    hhDto.HoraEntradaNoche == null &&
                    hhDto.HoraSalidaNoche == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "Por lo menos un turno deve tener horario de entrada y salida"
                    };
                }

                if (hhDto.HoraEntradaManana != null && hhDto.HoraSalidaManana != null) {
                    historicoHorarioDb.HoraEntradaMananaUTC = hhDto.HoraEntradaManana.Value;
                    historicoHorarioDb.HoraSalidaMananaUTC = hhDto.HoraSalidaManana.Value;
                }
                if (hhDto.HoraEntradaTarde != null && hhDto.HoraSalidaTarde != null) {
                    historicoHorarioDb.HoraEntradaTardeUTC = hhDto.HoraEntradaTarde.Value;
                    historicoHorarioDb.HoraSalidaTardeUTC = hhDto.HoraSalidaTarde.Value;
                }
                if (hhDto.HoraEntradaNoche != null && hhDto.HoraSalidaNoche != null) {
                    historicoHorarioDb.HoraEntradaNocheUTC = hhDto.HoraEntradaNoche.Value;
                    historicoHorarioDb.HoraSalidaNocheUTC = hhDto.HoraSalidaNoche.Value;
                }
                historicoHorarioDb.MomentoCarga = DateTime.Now;

                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                historicoHorarioDb.UsuarioID = usuarioID;

                context.HistoricoHorarios.Add(historicoHorarioDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                hhDto.HistoricoHorarioID = historicoHorarioDb.HistoricoHorarioID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el historico de horario : " + hhDto.HistoricoHorarioID,
                    ObjetoDto = hhDto
                };
            }
        }

        private MensajeDto EditarHistoricoHorario(HistoricoHorarioDto hhDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoHorarioDb = context.HistoricoHorarios
                    .Where(h => h.HistoricoHorarioID == hhDto.HistoricoHorarioID)
                    .FirstOrDefault();
                if (historicoHorarioDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de horario : " + hhDto.HistoricoHorarioID
                    };
                }
                historicoHorarioDb.HoraEntradaMananaUTC = (hhDto.HoraEntradaManana == null) ? hhDto.HoraEntradaManana : hhDto.HoraEntradaManana.Value;
                historicoHorarioDb.HoraSalidaMananaUTC = (hhDto.HoraSalidaManana == null) ? hhDto.HoraSalidaManana : hhDto.HoraSalidaManana.Value;

                historicoHorarioDb.HoraEntradaTardeUTC = (hhDto.HoraEntradaTarde == null) ? hhDto.HoraEntradaTarde : hhDto.HoraEntradaTarde.Value;
                historicoHorarioDb.HoraSalidaTardeUTC = (hhDto.HoraSalidaTarde == null) ? hhDto.HoraSalidaTarde : hhDto.HoraSalidaTarde.Value;

                historicoHorarioDb.HoraEntradaNocheUTC = (hhDto.HoraEntradaNoche == null) ? hhDto.HoraEntradaNoche : hhDto.HoraEntradaNoche.Value;
                historicoHorarioDb.HoraSalidaNocheUTC = (hhDto.HoraSalidaNoche == null) ? hhDto.HoraSalidaNoche : hhDto.HoraSalidaNoche.Value;

                context.Entry(historicoHorarioDb).State = System.Data.Entity.EntityState.Modified;

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el historico de horarios : " + hhDto.HistoricoHorarioID,
                    ObjetoDto = hhDto
                };
            }
        }
    }
}
