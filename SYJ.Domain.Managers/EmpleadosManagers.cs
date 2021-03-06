﻿using SYJ.Application.Dto;
using SYJ.Application.Dto.Auxiliares;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SYJ.Domain.Managers {
    public class EmpleadosManagers {
        /// <summary>
        /// Es el listado de empleados con su sucursal actual y su cargo actual
        /// </summary>
        /// <returns></returns>
        public List<EmpleadoDto> ListadoEmpleadosConMarcaDeActivo() {
            var listado = this.ListadoEmpleados();
            HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();
            SucursalesManagers sm = new SucursalesManagers();
            var sucursales = sm.ListadoSucursales();
            foreach (EmpleadoDto empleado in listado) {
                //Se ve si esta activo
                if (HistoricoIngresoSalidasManagers.EmpleadoTrabajaTodaviaEnLaEmpresa(empleado.EmpleadoID, DateTime.Today.Month, DateTime.Today.Year)) {
                    empleado.Activo = true;
                } else {
                    empleado.Activo = false;
                }
                //Se ve en que sucursal trabaja
                var sucursalID = int.Parse(hsm.UltimoSucursales(empleado.EmpleadoID).Valor);
                empleado.Sucursale = sucursales.Where(s => s.SucursalID == sucursalID).FirstOrDefault();
            }

            return listado.OrderBy(o => o.Apellidos).ToList();
        }

        public List<EmpleadoDto> RecuperarTodosLosEmpleados() {
            var listado = this.ListadoEmpleados();
            //Se carga las sucursales actuales
            HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();
            var hism = new HistoricoIngresoSalidasManagers();
            SucursalesManagers sm = new SucursalesManagers();
            var sucursales = sm.ListadoSucursales();
            foreach (EmpleadoDto empleado in listado) {
                //Se ve en que sucursal trabaja
                var sucursalID = int.Parse(hsm.UltimoSucursales(empleado.EmpleadoID).Valor);
                empleado.Sucursale = sucursales.Where(s => s.SucursalID == sucursalID).FirstOrDefault();
                // Se carga su fecha de ingreso y egreso
                var mensaje = hism.UltimoIngreso(empleado.EmpleadoID);
                var hismDto = (HistoricoIngresoSalidaDto)mensaje.ObjetoDto;
                empleado.FechaEntrada = hismDto.FechaIngreso;
                if (hismDto.FechaSalida != null) {
                    empleado.FechaSalida = hismDto.FechaSalida.Value;
                }
            }
            return listado;
        }

        public List<EmpleadoDto> ListadoEmpleadosConMarcaDeActivo(DateTime mesActivo) {
            var listado = this.ListadoEmpleados();
            HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();
            SucursalesManagers sm = new SucursalesManagers();
            var sucursales = sm.ListadoSucursales();
            foreach (EmpleadoDto empleado in listado) {
                //Se ve si esta activo
                if (HistoricoIngresoSalidasManagers.EmpleadoTrabajaTodaviaEnLaEmpresa(empleado.EmpleadoID, mesActivo.Month, mesActivo.Year)) {
                    empleado.Activo = true;
                } else {
                    empleado.Activo = false;
                }
                //Se ve en que sucursal trabaja
                var sucursalID = int.Parse(hsm.UltimoSucursales(empleado.EmpleadoID).Valor);
                empleado.Sucursale = sucursales.Where(s => s.SucursalID == sucursalID).FirstOrDefault();
            }

            return listado.OrderBy(o => o.Apellidos).ToList();
        }
        public List<EmpleadoDto> ListadoEmpleados() {
            using (var context = new SueldosJornalesEntities()) {
                List<EmpleadoDto> listado = context.Empleados
                    .Select(s => new EmpleadoDto() {
                        EmpleadoID = s.EmpleadoID,
                        Nombres = s.Nombres,
                        Apellidos = s.Apellidos,
                        FechaNacimiento = s.FechaNacimiento,
                        Sexo = new SexoDto() {
                            SexoID = s.Sexo,
                            NombreSexo = (s.Sexo == 1) ? "Masculino" : "Femenino"
                        },
                        NroCedula = s.NroCedula,
                        EstadoCivile = new EstadoCivileDto() {
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
                        CantidadHijos = s.CantidadHijos,
                        UsuarioID = s.UsuarioID
                    }).ToList();
                CargarCargoActualesAEmpleados(listado);
                return listado;
            }
        }
        public List<EmpleadoDto> ListadoEmpleadosConUsuarioIndeterminado() {
            var listadoEmpleados = this.ListadoEmpleadosConMarcaDeActivo();
            listadoEmpleados =  listadoEmpleados.Where(w => w.UsuarioID == 5).ToList();
            return listadoEmpleados;
        }
        private static void CargarCargoActualesAEmpleados(List<EmpleadoDto> empleados) {
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();
            foreach (var empleado in empleados) {
                var mensajeSalCargo = hsm.SalarioYCargoActual(empleado.EmpleadoID);
                if (!mensajeSalCargo.Error) {
                    var historicoSalDto = (HistoricoSalarioDto)mensajeSalCargo.ObjetoDto;
                    empleado.Cargo = historicoSalDto.Cargo;
                }
            }
        }

        public MensajeDto CargarEmpleado(EmpleadoDto eDto, Guid userID) {
            if (eDto.EmpleadoID > 0) {
                return EditarEmpleado(eDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                using (var dbContextTransaction = context.Database.BeginTransaction()) {
                    MensajeDto mensajeDto = null;
                    var empleadoDb = new Empleado();
                    //Se recupera la sucursal
                    long usuarioID;
                    int sucursalID;
                    if (eDto.Sucursale == null) {
                        usuarioID = context.Usuarios
                           .Where(u => u.UserID == userID)
                           .First().UsuarioID;

                        sucursalID = context.UbicacionSucUsuarios
                           .Where(u => u.UsuarioID == usuarioID)
                           .First().SucursalID;
                    } else {
                        usuarioID = 5;// Indeterminado
                        sucursalID = eDto.Sucursale.SucursalID;
                    }

                    empleadoDb.Nombres = eDto.Nombres;
                    empleadoDb.Apellidos = eDto.Apellidos;
                    empleadoDb.FechaNacimiento = eDto.FechaNacimiento;
                    empleadoDb.Sexo = eDto.Sexo.SexoID;
                    empleadoDb.NroCedula = eDto.NroCedula;
                    empleadoDb.EstadoCivilID = eDto.EstadoCivile.EstadoCivilID;
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

                    //Se carga el historico sucursales
                    HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();
                    HistoricoSucursaleDto hsDto = new HistoricoSucursaleDto();
                    hsDto.EmpleadoID = eDto.EmpleadoID;
                    hsDto.Sucursal = new SucursaleDto() {
                        SucursalID = sucursalID
                    };
                    MensajeDto mensajeDto2 = hsm.CargarHistoricoSucursal(hsDto, userID, context);
                    if (mensajeDto2.Error) { return mensajeDto2; }

                    dbContextTransaction.Commit();
                    return new MensajeDto() {
                        Error = false,
                        MensajeDelProceso = "Se cargo el empleado : " + eDto.EmpleadoID + " " +
                        mensajeDto2.MensajeDelProceso,
                        ObjetoDto = eDto
                    };
                }
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
                empleadoDb.EstadoCivilID = eDto.EstadoCivile.EstadoCivilID;
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

        public MensajeDto RecuperarEmpleado(int id) {
            using (var context = new SueldosJornalesEntities()) {
                var empleadoDto = context.Empleados.
                      Select(s => new EmpleadoDto() {
                          EmpleadoID = s.EmpleadoID,
                          Nombres = s.Nombres,
                          Apellidos = s.Apellidos,
                          FechaNacimiento = s.FechaNacimiento,
                          Sexo = new SexoDto() {
                              SexoID = s.Sexo,
                              NombreSexo = (s.Sexo == 1) ? "Masculino" : "Femenino"
                          },
                          NroCedula = s.NroCedula,
                          EstadoCivile = new EstadoCivileDto() {
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
                      })
                  .Where(e => e.EmpleadoID == id)
                  .FirstOrDefault();
                if (empleadoDto == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "El empleado ID : " + id + " no existe en la base de datos"
                    };
                }
                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se encontro al empleado : " + empleadoDto.Nombres + ' ' +
                    empleadoDto.Apellidos,
                    ObjetoDto = empleadoDto
                };
            }
        }
        public List<EmpleadoDto> ListadoEmpleadosInactivosSegunUbicacionSucursal(Guid userID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = GetListadoEmpleados(context);
                UbicacionSucUsuariosManagers usum = new UbicacionSucUsuariosManagers();
                HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();
                //Se filtra los empleados que trabajan en la sucursal donde esta posicionado en el sistema
                //el usuario
                var sucursaleIDposicionUsuario = usum.RecuperarSucursalSegunUsuario(userID);
                var listadoFiltrado = new List<EmpleadoDto>();
                foreach (var l in listado) {
                    int ultimaSucursalDeTrabajoEmpleado = int.Parse(hsm.UltimoSucursales(l.EmpleadoID).Valor);
                    if (sucursaleIDposicionUsuario == ultimaSucursalDeTrabajoEmpleado) {
                        listadoFiltrado.Add(l);
                    }
                }
                //Se recupera solo los empleados que no trabajan mas en la empresa
                var listadoFiltrado2 = new List<EmpleadoDto>();
                foreach (var l in listadoFiltrado) {
                    if (!HistoricoIngresoSalidasManagers.EmpleadoTrabajaTodaviaEnLaEmpresa(l.EmpleadoID)) {
                        listadoFiltrado2.Add(l);
                    }
                }
                return listadoFiltrado2;
            }
        }
        public List<EmpleadoDto> ListadoEmpleadosActivos(int year, int mes) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = GetListadoEmpleados(context);
                var listadoFiltrado = new List<EmpleadoDto>();
                foreach (var empleado in listado) {
                    if (HistoricoIngresoSalidasManagers.EmpleadoTrabajaTodaviaEnLaEmpresa(empleado.EmpleadoID, year, mes)) {
                        listadoFiltrado.Add(empleado);
                    }
                }
                return listadoFiltrado;
            }
        }
        public List<EmpleadoDto> ListadoEmpleadosSegunUbicacionSucursal(Guid userID) {
            var diaHoy = DateTime.Today;
            return ListadoEmpleadosSegunUbicacionSucursal(userID, diaHoy.Month, diaHoy.Year);
        }
        public List<EmpleadoDto> ListadoEmpleadosSegunUbicacionSucursal(Guid userID, int mes, int year) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = GetListadoEmpleados(context);
                UbicacionSucUsuariosManagers usum = new UbicacionSucUsuariosManagers();
                HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();

                var sucursaleIDposicionUsuario = usum.RecuperarSucursalSegunUsuario(userID);
                var listadoFiltrado = new List<EmpleadoDto>();
                foreach (var l in listado) {
                    int ultimaSucursalDeTrabajoEmpleado = int.Parse(hsm.UltimoSucursales(l.EmpleadoID).Valor);
                    if (sucursaleIDposicionUsuario == ultimaSucursalDeTrabajoEmpleado) {
                        listadoFiltrado.Add(l);
                    }
                }
                //Se quita del listado los empleados que tengan salida en su ultimo historico de entrada salida
                //Si salio en el mes actual todavia se le considera como que trabaja dentro del mes
                var listadoFiltrado2 = new List<EmpleadoDto>();
                foreach (var l in listadoFiltrado) {
                    if (HistoricoIngresoSalidasManagers.EmpleadoTrabajaTodaviaEnLaEmpresa(l.EmpleadoID, mes, year)) {
                        listadoFiltrado2.Add(l);
                    }
                }
                return listadoFiltrado2;
            }
        }

        private static List<EmpleadoDto> GetListadoEmpleados(SueldosJornalesEntities context) {
            var listado = context.Empleados
                .Select(s => new EmpleadoDto() {
                    EmpleadoID = s.EmpleadoID,
                    Nombres = s.Nombres,
                    Apellidos = s.Apellidos,
                    FechaNacimiento = s.FechaNacimiento,
                    Sexo = new SexoDto() {
                        SexoID = s.Sexo,
                        NombreSexo = (s.Sexo == 1) ? "Masculino" : "Femenino"
                    },
                    NroCedula = s.NroCedula,
                    EstadoCivile = new EstadoCivileDto() {
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
            CargarCargoActualesAEmpleados(listado);
            return listado;
        }

        public MensajeDto DetalleLiquidacion(EmpleadoDto eDto, Guid userID) {
            throw new NotImplementedException();
        }

        public long[] EmpleadosSeleccionados(MesYearEmpresaSucursalesDto psDto) {
            using (var context = new SueldosJornalesEntities()) {
                List<long> empleadoIDs = new List<long>();
                var empleados = context.Empleados.ToList();
                var mesSolicitado = new DateTime(psDto.Year, psDto.Mes.MesID, DateTime.DaysInMonth(psDto.Year, psDto.Mes.MesID));
                empleados.ForEach(delegate (Empleado e) {
                    var sucursalActual = e.HistoricoSucursales
                        .Where(h => h.MomentoCarga <= mesSolicitado)
                        .OrderByDescending(h => h.MomentoCarga)
                        .FirstOrDefault();
                    if (sucursalActual != null) {
                        if (psDto.Sucursales.Exists(s => s.SucursalID == sucursalActual.SucursalID)) {
                            if (HistoricoIngresoSalidasManagers.EmpleadoTrabajaTodaviaEnLaEmpresa(e.EmpleadoID, psDto.Mes.MesID, psDto.Year)) {
                                empleadoIDs.Add(e.EmpleadoID);
                            }
                        }
                    }
                });
                return empleadoIDs.ToArray();
            }
        }

        public static EmpleadoDto GetEmpleado(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var empleado = context.Empleados
                    .Where(e => e.EmpleadoID == empleadoID)
                     .Select(s => new EmpleadoDto() {
                         EmpleadoID = s.EmpleadoID,
                         Nombres = s.Nombres,
                         Apellidos = s.Apellidos,
                         FechaNacimiento = s.FechaNacimiento,
                         Sexo = new SexoDto() {
                             SexoID = s.Sexo,
                             NombreSexo = (s.Sexo == 1) ? "Masculino" : "Femenino"
                         },
                         NroCedula = s.NroCedula,
                         EstadoCivile = new EstadoCivileDto() {
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
                     })
                    .FirstOrDefault();
                return empleado;
            }
        }
    }
}
