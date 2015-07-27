using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class HistoricoTelefonosManagers {

        public List<HistoricoTelefonoDto> ListadoHistoricoTelefonos() {
            throw new NotImplementedException();
        }

        public MensajeDto CargarHistoricoTelefono(HistoricoTelefonoDto htDto, Guid userID) {
            if (htDto.HistoricoTelefonoID > 0) {
                return EditarHistoricoTelefono(htDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var hisTelefonoDb = new HistoricoTelefono();
                hisTelefonoDb.EmpleadoID = htDto.EmpleadoID;
                hisTelefonoDb.Telefonos = htDto.Telefonos;
                hisTelefonoDb.MomentoCarga = DateTime.Now;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                hisTelefonoDb.UsuarioID = usuarioID;

                context.HistoricoTelefonos.Add(hisTelefonoDb);
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                htDto.HistoricoTelefonoID = hisTelefonoDb.HistoricoTelefonoID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el historico telefono : " + htDto.HistoricoTelefonoID,
                    ObjetoDto = htDto
                };
            }
        }

        public List<HistoricoTelefonoDto> ListadoHistoricoTelefonos(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.HistoricoTelefonos
                    .Where(h => h.EmpleadoID == empleadoID)
                    .Select(s => new HistoricoTelefonoDto() {
                        HistoricoTelefonoID = s.HistoricoTelefonoID,
                        EmpleadoID = s.EmpleadoID,
                        Telefonos = s.Telefonos
                    })
                    .ToList();
                return listado;
            }
        }
        private static MensajeDto EditarHistoricoTelefono(HistoricoTelefonoDto htDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var hisTelefonoDb = context.HistoricoTelefonos
                    .Where(h => h.HistoricoTelefonoID == htDto.HistoricoTelefonoID)
                    .FirstOrDefault();
                if (hisTelefonoDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de telefonos : " + htDto.HistoricoTelefonoID
                    };
                }
                hisTelefonoDb.Telefonos = htDto.Telefonos;

                context.Entry(hisTelefonoDb).State = System.Data.Entity.EntityState.Modified;

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el historico de telefono : " + htDto.HistoricoTelefonoID,
                    ObjetoDto = htDto
                };
            }
        }

        public MensajeDto EliminarHistoricoTelefono(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var hisTelefonoDb = context.HistoricoTelefonos
                  .Where(h => h.HistoricoTelefonoID == id)
                  .FirstOrDefault();
                if (hisTelefonoDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de telefonos : " + id
                    };
                }
                context.HistoricoTelefonos.Remove(hisTelefonoDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el historico de telefono : " + id
                };
            }
        }
    }
}
