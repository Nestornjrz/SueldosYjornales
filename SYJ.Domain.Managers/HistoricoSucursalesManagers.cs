using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class HistoricoSucursalesManagers {

        public List<HistoricoSucursaleDto> ListadoHistoricoSucursales() {
            throw new NotImplementedException();
        }

        public List<HistoricoSucursaleDto> ListadoHistoricoSucursales(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.HistoricoSucursales
                    .Where(hs => hs.EmpleadoID == empleadoID)
                    .OrderByDescending(hs=>hs.MomentoCarga)
                    .Select(s => new HistoricoSucursaleDto() {
                        HistoricoSucursalID = s.HistoricoSucursalID,
                        EmpleadoID = empleadoID,
                        Sucursal = new SucursaleDto() {
                            SucursalID = s.SucursalID,
                            NombreSucursal = s.Sucursale.NombreSucursal
                        },
                        MomentoCarga = s.MomentoCarga
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarHistoricoSucursal(HistoricoSucursaleDto hsDto,
            Guid userID,
            SueldosJornalesEntities contextAlter = null) {
            if (hsDto.HistoricoSucursalID > 0) {
                return EditarHistoricoSucursale(hsDto);
            }
            var context = CrearContext(contextAlter);
            MensajeDto mensajeDto = null;
            var hisSucursaleDb = new HistoricoSucursale();
            hisSucursaleDb.EmpleadoID = hsDto.EmpleadoID;
            hisSucursaleDb.SucursalID = hsDto.Sucursal.SucursalID;
            hisSucursaleDb.MomentoCarga = DateTime.Now;
            //Se recupera el usuarioID
            var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                .First().UsuarioID;
            hisSucursaleDb.UsuarioID = usuarioID;

            context.HistoricoSucursales.Add(hisSucursaleDb);

            mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
            if (mensajeDto != null) { return mensajeDto; }

            hsDto.HistoricoSucursalID = hisSucursaleDb.HistoricoSucursalID;

            if (contextAlter == null) { context.Dispose(); }

            return new MensajeDto() {
                Error = false,
                MensajeDelProceso = "Se cargo el historico sucursal : " + hsDto.HistoricoSucursalID,
                ObjetoDto = hsDto
            };

        }

        private static SueldosJornalesEntities CrearContext(SueldosJornalesEntities contextAlter) {
            if (contextAlter != null) {
                return contextAlter;
            }
            return new SueldosJornalesEntities();
        }

        private static MensajeDto EditarHistoricoSucursale(HistoricoSucursaleDto hsDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var hisSucursaleDb = context.HistoricoSucursales
                    .Where(hs => hs.HistoricoSucursalID == hsDto.HistoricoSucursalID)
                    .FirstOrDefault();
                if (hisSucursaleDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de sucursales : " + hsDto.HistoricoSucursalID
                    };
                }
                hisSucursaleDb.SucursalID = hsDto.Sucursal.SucursalID;

                context.Entry(hisSucursaleDb).State = System.Data.Entity.EntityState.Modified;

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el historico de sucursales : " + hsDto.HistoricoSucursalID,
                    ObjetoDto = hsDto
                };
            }
        }

        public MensajeDto EliminarHistoricoSucursal(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var hisSucursaleDb = context.HistoricoSucursales
                    .Where(hs => hs.HistoricoSucursalID == id)
                    .FirstOrDefault();
                if (hisSucursaleDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de sucursales : " + id
                    };
                }
                context.HistoricoSucursales.Remove(hisSucursaleDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el historico de sucursal : " + id
                };
            }
        }
        /// <summary>
        /// Devuelve el mensajeDto conteniendo la sucursal actual donde trabaja.
        /// </summary>
        /// <param name="empleadoID"></param>
        /// <returns></returns>
        public MensajeDto UltimoSucursales(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var ultimoSucursal = context.HistoricoSucursales
                    .Where(h=>h.EmpleadoID == empleadoID)
                    .OrderByDescending(h => h.MomentoCarga)
                    .FirstOrDefault();
                if (ultimoSucursal == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existen datos de sucursales"
                    };
                }
                var hsDto = new HistoricoSucursaleDto() {
                    HistoricoSucursalID = ultimoSucursal.HistoricoSucursalID,
                    EmpleadoID = ultimoSucursal.EmpleadoID,
                    Sucursal = new SucursaleDto() {
                        SucursalID = ultimoSucursal.SucursalID,
                        NombreSucursal = ultimoSucursal.Sucursale.NombreSucursal,
                        Descripcion = ultimoSucursal.Sucursale.Descripcion
                    }
                };
                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Ultima sucursal encontrada",
                    ObjetoDto = hsDto,
                    Valor = ultimoSucursal.SucursalID.ToString()
                };
            }
        }
    }
}
