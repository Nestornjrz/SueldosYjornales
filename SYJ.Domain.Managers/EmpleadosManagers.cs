using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class EmpleadosManagers {
        public List<EmpleadoDto> ListadoEmpleados() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Empleados
                    .Select(s => new EmpleadoDto() {
                        EmpleadoID = s.EmpleadoID,
                        Sucursale = new SucursaleDto() {
                            SucursalID = s.SucursalID,
                            NombreSucursal = s.Sucursale.NombreSucursal,
                            Abreviatura = s.Sucursale.Abreviatura,
                            Descripcion = s.Sucursale.Descripcion
                        },
                        Nombres = s.Nombres,
                        Apellidos = s.Apellidos,
                        FechaNacimiento = s.FechaNacimiento,
                        Sexo = new SexoDto() {
                            SexoID = s.Sexo,
                            NombreSexo = (s.Sexo == 1)?"Masculino":"Femenino"
                        },
                        NroCedula = s.NroCedula,
                        EstadoCivil = new EstadoCivileDto() {
                            EstadoCivilID = s.EstadoCivilID,
                            NombreEstadoCivil = s.EstadoCivile.NombreEstadoCivil
                        },
                        Nacionalidade = new NacionalidadeDto() {
                            NacionalidadID = s.NacionalidadID,
                            NombreNacionalidad = s.Nacionalidade.NombreNacionalidad
                        },
                        NumeroIps = s.NumeroIps,
                        NumeroMjt = s.NumeroMjt,
                        Profesione = new ProfesioneDto() {
                            ProfesionID = s.ProfesionID,
                            NombreProfesion = s.Profesione.NombreProfesion,
                            Abreviatura = s.Profesione.Abreviatura,
                            Descripcion = s.Profesione.Descripcion
                        },
                        CantidadHijos = s.CantidadHijos
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarEmpleado(EmpleadoDto eDto, Guid userID) {
            if (eDto.EmpleadoID > 0) {
                return EditarEmpleado(eDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var empleadoDb = new Empleado();
                //Se recupera la sucursal
                var usuarioID = context.Usuarios
                    .Where(u => u.UserID == userID)
                    .First().UsuarioID;

                var sucursalID = context.UbicacionSucUsuarios
                    .Where(u => u.UsuarioID == usuarioID)
                    .First().SucursalID;

                empleadoDb.SucursalID = sucursalID;
                empleadoDb.Nombres = eDto.Nombres;
                empleadoDb.Apellidos = eDto.Apellidos;
                empleadoDb.FechaNacimiento = eDto.FechaNacimiento;
                empleadoDb.Sexo = eDto.Sexo.SexoID;
                empleadoDb.NroCedula = eDto.NroCedula;
                empleadoDb.EstadoCivilID = eDto.EstadoCivil.EstadoCivilID;
                empleadoDb.NacionalidadID = eDto.Nacionalidade.NacionalidadID;
                empleadoDb.NumeroIps = eDto.NumeroIps;
                empleadoDb.NumeroMjt = eDto.NumeroMjt;
                empleadoDb.ProfesionID = eDto.Profesione.ProfesionID;
                empleadoDb.CantidadHijos = eDto.CantidadHijos;
                empleadoDb.UsuarioID = usuarioID;
                empleadoDb.MomentoCarga = DateTime.Now;

                context.Empleados.Add(empleadoDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                eDto.EmpleadoID = empleadoDb.EmpleadoID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el empleado : " + eDto.EmpleadoID,
                    ObjetoDto = eDto
                };
            }
        }

        private MensajeDto EditarEmpleado(EmpleadoDto eDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var empleadoDb = context.Empleados
                    .Where(e => e.EmpleadoID == eDto.EmpleadoID)
                    .FirstOrDefault();
                if (empleadoDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el empleado : " + eDto.EmpleadoID +
                        " no puede ser editado."
                    };
                }
                empleadoDb.Nombres = eDto.Nombres;
                empleadoDb.Apellidos = eDto.Apellidos;
                empleadoDb.FechaNacimiento = eDto.FechaNacimiento;
                empleadoDb.Sexo = eDto.Sexo.SexoID;
                empleadoDb.NroCedula = eDto.NroCedula;
                empleadoDb.EstadoCivilID = eDto.EstadoCivil.EstadoCivilID;
                empleadoDb.NacionalidadID = eDto.Nacionalidade.NacionalidadID;
                empleadoDb.NumeroIps = eDto.NumeroIps;
                empleadoDb.NumeroMjt = eDto.NumeroMjt;
                empleadoDb.ProfesionID = eDto.Profesione.ProfesionID;
                empleadoDb.CantidadHijos = eDto.CantidadHijos;

                context.Entry(empleadoDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el empleado : " + eDto.EmpleadoID,
                    ObjetoDto = eDto
                };
            }
        }

        public MensajeDto EliminarEmpleado(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var empleadoDb = context.Empleados
                   .Where(e => e.EmpleadoID == id)
                   .FirstOrDefault();
                if (empleadoDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "El empleado ID : " + id + " no existe en la base de datos"
                    };
                }
                context.Empleados.Remove(empleadoDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el empleado : " + id
                };
            }
        }
    }
}
