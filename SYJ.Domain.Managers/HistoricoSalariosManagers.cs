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

        public MensajeDto EliminarHistoricoSalario(int id) {
            throw new NotImplementedException();
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
