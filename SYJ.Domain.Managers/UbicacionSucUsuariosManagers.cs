using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class UbicacionSucUsuariosManagers {
        public List<UbicacionSucUsuarioDto> ListadoUbicacionSucUsuarios(Guid userID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.UbicacionSucUsuarios
                    .Select(s => new UbicacionSucUsuarioDto() {
                        Sucursale = new SucursaleDto() {
                            SucursalID = s.SucursalID,
                            NombreSucursal = s.Sucursale.NombreSucursal,
                            Descripcion = s.Sucursale.Descripcion,
                            Abreviatura = s.Sucursale.Abreviatura,
                            Empresa = new EmpresaDto() {
                                EmpresaID = s.Sucursale.EmpresaID,
                                NombreEmpresa = s.Sucursale.Empresa.NombreEmpresa,
                                Abreviatura = s.Sucursale.Empresa.Abreviatura,
                                Descripcion = s.Sucursale.Empresa.Descripcion,
                                Ruc = s.Sucursale.Empresa.Ruc
                            }
                        }
                    }).ToList();
                return listado;
            }
        }
        public MensajeDto RecuperarUbicacionSucUsuario(Guid userID) {
            using (var context = new SueldosJornalesEntities()) {
                //Se busca el usuario
                var usuario = context.Usuarios
                    .Where(u => u.UserID == userID)
                    .FirstOrDefault();
                if (usuario == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "For favor loguese primero",
                        ObjetoDto = new UbicacionSucUsuarioDto() {
                            Sucursale = new SucursaleDto() {
                                SucursalID = 0,
                                NombreSucursal = "Sin logeo",
                                Descripcion = "Falsta usuario logeado",
                                Abreviatura = "SL",
                                Empresa = new EmpresaDto() {
                                    EmpresaID = 0,
                                    NombreEmpresa = "Sin Logeo",
                                    Abreviatura = "SL",
                                    Descripcion = "Falta logeo de usuario",
                                    Ruc = "Sin logeo"
                                }
                            }
                        }
                    };
                }
                var usuarioID = usuario.UsuarioID;

                var ubicacionSucUsuario = context.UbicacionSucUsuarios
                    .Where(u => u.UsuarioID == usuarioID)
                    .Select(s => new UbicacionSucUsuarioDto() {
                        Sucursale = new SucursaleDto() {
                            SucursalID = s.SucursalID,
                            NombreSucursal = s.Sucursale.NombreSucursal,
                            Descripcion = s.Sucursale.Descripcion,
                            Abreviatura = s.Sucursale.Abreviatura,
                            Empresa = new EmpresaDto() {
                                EmpresaID = s.Sucursale.EmpresaID,
                                NombreEmpresa = s.Sucursale.Empresa.NombreEmpresa,
                                Abreviatura = s.Sucursale.Empresa.Abreviatura,
                                Descripcion = s.Sucursale.Empresa.Descripcion,
                                Ruc = s.Sucursale.Empresa.Ruc
                            }
                        }
                    }).First();
                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se recupero la UbicacionSucUsuario",
                    ObjetoDto = ubicacionSucUsuario
                };
            }
        }

        public int RecuperarSucursalSegunUsuario(Guid userID) {
            using (var context = new SueldosJornalesEntities()) {
                //Se busca el usuario
                var usuario = context.Usuarios
                    .Where(u => u.UserID == userID)
                    .First();
                int sucursaleID = context.UbicacionSucUsuarios
                    .Where(u => u.Usuario.UserID == userID)
                    .First().SucursalID;
                return sucursaleID;
            }
        }

        public MensajeDto CargarUbicacionSucUsuario(UbicacionSucUsuarioDto usuDto, Guid userID) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                //Se busca el usuario
                var usuarioID = context.Usuarios
                    .Where(u => u.UserID == userID)
                    .First().UsuarioID;
                //Se consulta si ya tiene seleccion
                var ubicacionSucUsuarioDbEncontrado = context.UbicacionSucUsuarios
                    .Where(u => u.UsuarioID == usuarioID).FirstOrDefault();
                if (ubicacionSucUsuarioDbEncontrado != null) {
                    return EditarUbicacionSucUsuario(usuDto,
                        ubicacionSucUsuarioDbEncontrado, context);
                }

                var ubicacionSucUsuarioDb = new UbicacionSucUsuario();
                ubicacionSucUsuarioDb.UsuarioID = usuarioID;
                ubicacionSucUsuarioDb.SucursalID = usuDto.Sucursale.SucursalID;

                context.UbicacionSucUsuarios.Add(ubicacionSucUsuarioDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se selecciono la sucursal de trabajo: " + usuDto.Sucursale.NombreSucursal
                };
            }
        }
        private MensajeDto EditarUbicacionSucUsuario(UbicacionSucUsuarioDto usuDto, UbicacionSucUsuario ubicacionSucUsuarioDb, SueldosJornalesEntities context) {
            MensajeDto mensajeDto = null;

            ubicacionSucUsuarioDb.SucursalID = usuDto.Sucursale.SucursalID;

            context.Entry(ubicacionSucUsuarioDb).State = System.Data.Entity.EntityState.Modified;
            mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
            if (mensajeDto != null) { return mensajeDto; }

            return new MensajeDto() {
                Error = false,
                MensajeDelProceso = "Se cambio la selecciona de la sucursal a  : " + usuDto.Sucursale.NombreSucursal
            };
        }
    }
}
