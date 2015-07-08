using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class HistoricoSalariosManagers {
        public List<HistoricoSalarioDto> ListadoHistoricoSalarios() {
            throw new NotImplementedException();
        }

        public MensajeDto CargarHistoricoSalario(HistoricoSalarioDto hsDto, Guid userID) {
            if (hsDto.HistoricoSalarioID > 0) {
                return EditarHistoricoSalario(hsDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoSalarioDb = new HistoricoSalario();
                historicoSalarioDb.EmpleadoID = hsDto.EmpleadoID;
                historicoSalarioDb.Monto = hsDto.Monto;
                historicoSalarioDb.CargoID = hsDto.Cargo.CargoID;
                historicoSalarioDb.Observacion = hsDto.Observacion;
                historicoSalarioDb.FechaSalario = hsDto.FechaSalario;
                historicoSalarioDb.MomentoCarga = DateTime.Now;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                historicoSalarioDb.UsuarioID = usuarioID;

                context.HistoricoSalarios.Add(historicoSalarioDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                hsDto.HistoricoSalarioID = historicoSalarioDb.HistoricoSalarioID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el historico salario : " + hsDto.HistoricoSalarioID,
                    ObjetoDto = hsDto
                };
            }
        }

        private MensajeDto EditarHistoricoSalario(HistoricoSalarioDto hsDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoSalarioDb = context.HistoricoSalarios
                    .Where(h => h.HistoricoSalarioID == hsDto.HistoricoSalarioID)
                    .FirstOrDefault();
                if (historicoSalarioDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de salario : " 
                        + hsDto.HistoricoSalarioID 
                    };
                }
                historicoSalarioDb.Monto = hsDto.Monto;
                historicoSalarioDb.CargoID = hsDto.Cargo.CargoID;
                historicoSalarioDb.Observacion = hsDto.Observacion;
                historicoSalarioDb.FechaSalario = hsDto.FechaSalario;

                context.Entry(historicoSalarioDb).State = System.Data.Entity.EntityState.Modified;

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el historico de salario : " + hsDto.HistoricoSalarioID,
                    ObjetoDto = hsDto
                };
            }
        }

        public MensajeDto EliminarHistoricoSalario(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoSalarioDb = context.HistoricoSalarios
                    .Where(h => h.HistoricoSalarioID == id)
                    .FirstOrDefault();
                if (historicoSalarioDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de salario : "
                        + id
                    };
                }
                context.HistoricoSalarios.Remove(historicoSalarioDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el historico de salario : " + id
                };
            }
        }

        public List<HistoricoSalarioDto> ListadoHistoricoSalarios(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.HistoricoSalarios
                    .Where(h => h.EmpleadoID == empleadoID)
                    .Select(s => new HistoricoSalarioDto() {
                        HistoricoSalarioID = s.HistoricoSalarioID,
                        EmpleadoID = s.EmpleadoID,
                        Cargo = new CargoDto() {
                            CargoID = s.CargoID,
                            NombreCargo = s.Cargo.NombreCargo,
                            Abreviatura = s.Cargo.Abreviatura
                        },
                        FechaSalario = s.FechaSalario,
                        Monto = s.Monto,
                        Observacion = s.Observacion
                    }).ToList();
                return listado;
            }
        }
    }
}
