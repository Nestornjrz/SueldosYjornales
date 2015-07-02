using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class UsuariosManagers {

        public List<UsuarioDto> ListadoUsuarios() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Usuarios
                    .Select(s => new UsuarioDto() {
                        UsuarioID = s.UsuarioID,
                        NombreUsuario = s.NombreUsuario,
                        CorreoElectronico = s.CorreoElectronico
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarUsuarios(UsuarioDto uDto) {
            if (uDto.UsuarioID > 0) {
                return EditarUsuario(uDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                if (context.AspNetUsers
                    .Where(u => u.Email == uDto.CorreoElectronico)
                    .Count() <= 0) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe un registro del correo del usuario"
                    };
                }
                MensajeDto mensajeDto = null;
                var usuarioDb = new Usuario();
                usuarioDb.NombreUsuario = uDto.NombreUsuario;
                usuarioDb.CorreoElectronico = uDto.CorreoElectronico;
                usuarioDb.UserID = Guid.Parse(context.AspNetUsers
                    .Where(u => u.Email == uDto.CorreoElectronico).First().Id);

                context.Usuarios.Add(usuarioDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                uDto.UsuarioID = usuarioDb.UsuarioID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el usuario : " + uDto.UsuarioID,
                    ObjetoDto = uDto
                };
            }
        }
        private MensajeDto EditarUsuario(UsuarioDto uDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var usuarioDb = context.Usuarios
                    .Where(u => u.UsuarioID == uDto.UsuarioID)
                    .FirstOrDefault();
                if (usuarioDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el usuario con id " + uDto.UsuarioID
                    };
                }
                context.Entry(usuarioDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el usuario : " + uDto.UsuarioID,
                    ObjetoDto = uDto
                };
            }
        }

        public MensajeDto EliminarUsuarios(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var usuarioDb = context.Usuarios
                   .Where(u => u.UsuarioID == id)
                   .FirstOrDefault();
                if (usuarioDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "El usuario ID : " + id + " no existe en la base de datos"
                    };
                }

                context.Usuarios.Remove(usuarioDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el usuario : " + id
                };
            }
        }
    }
}
