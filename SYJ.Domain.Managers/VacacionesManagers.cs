using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class VacacionesManagers {
        public decimal TotalVacacionesPagadasYear(int year, long empleadoID) {
            //Se calcula primero el sueldo
            decimal sueldo = 0;
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();
            var mensajeSalarioYCargo = hsm.SalarioYCargo(empleadoID, new DateTime(year, 12, DateTime.DaysInMonth(year, 12)));
            if (!mensajeSalarioYCargo.Error) {
                var historicoSalarioDto = (HistoricoSalarioDto)mensajeSalarioYCargo.ObjetoDto;
                sueldo = historicoSalarioDto.Monto;
            }
            var sueldoDiario = sueldo / 30;

            using (var context = new SueldosJornalesEntities()) {
                var vacaciones = context.Vacaciones
                    .Where(v => v.FechaSalida.Year == year &&
                                v.EmpleadoID == empleadoID)
                    .ToList();
                int cantidadHorasVacaciones = 0;
                if (vacaciones != null) {
                    cantidadHorasVacaciones = vacaciones.Sum(s => s.DiasUsufructuados);
                }
                return sueldoDiario * cantidadHorasVacaciones;
            }
        }
        public MensajeDto CargarVacacion(VacacioneDto vDto, Guid userID) {
            if (vDto.VacacionID > 0) {
                return EditarVacacion(vDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var vacacionDb = new Vacacione();
                vacacionDb.EmpleadoID = vDto.EmpleadoID;
                vacacionDb.FechaSalida = vDto.FechaSalida;
                vacacionDb.DiasUsufructuados = vDto.DiasUsufructuados;
                vacacionDb.Observacion = vDto.Observacion;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                vacacionDb.UsuarioID = usuarioID;
                context.Vacaciones.Add(vacacionDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                vDto.VacacionID = vacacionDb.VacacionID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo la vacacion : " + vDto.VacacionID,
                    ObjetoDto = vDto
                };
            }
        }

        public List<VacacioneDto> ListadoVacaciones(int empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Vacaciones
                    .Where(v => v.EmpleadoID == empleadoID)
                    .Select(s => new VacacioneDto() {
                        VacacionID = s.VacacionID,
                        EmpleadoID = s.EmpleadoID,
                        FechaSalida = s.FechaSalida,
                        DiasUsufructuados = s.DiasUsufructuados,
                        Observacion = s.Observacion
                    }).ToList();
                return listado;
            }
        }

        private MensajeDto EditarVacacion(VacacioneDto vDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var vacacionDb = context.Vacaciones
                    .Where(v => v.VacacionID == vDto.VacacionID)
                    .FirstOrDefault();
                if (vacacionDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe la vacacion : " + vDto.VacacionID
                    };
                }
                vacacionDb.FechaSalida = vDto.FechaSalida;
                vacacionDb.DiasUsufructuados = vDto.DiasUsufructuados;
                vacacionDb.Observacion = vDto.Observacion;

                context.Entry(vacacionDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito la vacacion: " + vDto.VacacionID,
                    ObjetoDto = vDto
                };
            }
        }
        public MensajeDto EliminarVacacion(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var vacacionDb = context.Vacaciones
                   .Where(v => v.VacacionID == id)
                   .FirstOrDefault();
                if (vacacionDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe la vacacion : " + id
                    };
                }

                context.Vacaciones.Remove(vacacionDb);
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino la Vacacion : " + id
                };
            }
        }
    }
}
