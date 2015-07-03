using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class SucursalesManagers {
        public List<SucursaleDto> ListadoSucursales() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Sucursales
                    .Select(s => new SucursaleDto() {
                        SucursalID = s.SucursalID,
                        Empresa = new EmpresaDto() {
                            EmpresaID = s.EmpresaID,
                            NombreEmpresa = s.Empresa.NombreEmpresa,
                            Descripcion = s.Empresa.Descripcion,
                            Abreviatura = s.Empresa.Abreviatura,
                            Ruc = s.Empresa.Abreviatura
                        },
                        NombreSucursal = s.NombreSucursal,
                        Descripcion = s.Descripcion,
                        Abreviatura = s.Abreviatura
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarSucursal(SucursaleDto sDto) {
            if (sDto.SucursalID > 0) {
                return EditarSucursal(sDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var sucursaleDb = new Sucursale();
                sucursaleDb.EmpresaID = sDto.Empresa.EmpresaID;
                sucursaleDb.NombreSucursal = sDto.NombreSucursal;
                sucursaleDb.Abreviatura = sDto.Abreviatura;
                sucursaleDb.Descripcion = sDto.Descripcion;

                context.Sucursales.Add(sucursaleDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                sDto.SucursalID = sucursaleDb.SucursalID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo la sucursal : " + sDto.SucursalID,
                    ObjetoDto = sDto
                };
            }
        }
        private MensajeDto EditarSucursal(SucursaleDto sDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var sucursaleDb = context.Sucursales
                    .Where(s => s.SucursalID == sDto.SucursalID)
                    .First();
                sucursaleDb.EmpresaID = sDto.Empresa.EmpresaID;
                sucursaleDb.NombreSucursal = sDto.NombreSucursal;
                sucursaleDb.Abreviatura = sDto.Abreviatura;
                sucursaleDb.Descripcion = sDto.Descripcion;

                context.Entry(sucursaleDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito la sucursal : " + sDto.SucursalID,
                    ObjetoDto = sDto
                };
            }
        }

        public MensajeDto EliminarSucursal(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var sucursalDb = context.Sucursales
                    .Where(s => s.SucursalID == id)
                    .FirstOrDefault();

                if (sucursalDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "La sucursal ID : " + id + " no existe en la base de datos"
                    };
                }

                context.Sucursales.Remove(sucursalDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino la sucursal : " + id
                };
            }
        }

        public List<SucursaleDto> ListadoSucursales(int empresaID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Sucursales
                    .Where(s => s.EmpresaID == empresaID)
                    .Select(s => new SucursaleDto() {
                        SucursalID = s.SucursalID,
                        Empresa = new EmpresaDto() {
                            EmpresaID = s.EmpresaID,
                            NombreEmpresa = s.Empresa.NombreEmpresa,
                            Descripcion = s.Empresa.Descripcion,
                            Abreviatura = s.Empresa.Abreviatura,
                            Ruc = s.Empresa.Abreviatura
                        },
                        NombreSucursal = s.NombreSucursal,
                        Descripcion = s.Descripcion,
                        Abreviatura = s.Abreviatura
                    }).ToList();
                return listado;
            }
        }
    }
}
