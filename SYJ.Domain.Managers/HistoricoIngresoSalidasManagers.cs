﻿using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SYJ.Domain.Managers {
    public class HistoricoIngresoSalidasManagers {
        public List<HistoricoIngresoSalidaDto> ListadoHistoricoIngresoSalidas(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.HistoricoIngresoSalidas
                    .Where(h => h.EmpleadoID == empleadoID)
                    .OrderByDescending(h => h.MomentoCarga)
                    .Select(s => new HistoricoIngresoSalidaDto() {
                        HistoricoIngresoSalidaID = s.HistoricoIngresoSalidaID,
                        EmpleadoID = s.EmpleadoID,
                        FechaIngreso = s.FechaIngreso,
                        FechaSalida = s.FechaSalida,
                        MomentoCarga = s.MomentoCarga,
                        MotivoSalida = s.MotivoSalida,
                        MotivoIngreso = s.MotivoIngreso,
                        ConceptosIngreEgre = new ConceptosIngreEgreDto() {
                            ConceptoIngreEgreID = s.ConceptoIngreEgreID,
                            Concepto = s.ConceptosIngreEgre.Concepto
                        }
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarHistoricoIngresoSalida(HistoricoIngresoSalidaDto hisDto, Guid userID) {
            if (hisDto.HistoricoIngresoSalidaID > 0) {
                return EditarHistoricoIngresoSalida(hisDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoIngresoSalidaDb = new HistoricoIngresoSalida();
                historicoIngresoSalidaDb.EmpleadoID = hisDto.EmpleadoID;
                historicoIngresoSalidaDb.FechaIngreso = hisDto.FechaIngreso;
                historicoIngresoSalidaDb.FechaSalida = hisDto.FechaSalida;
                historicoIngresoSalidaDb.MomentoCarga = DateTime.Now;
                historicoIngresoSalidaDb.MotivoSalida = hisDto.MotivoSalida;
                historicoIngresoSalidaDb.MotivoIngreso = hisDto.MotivoIngreso;
                historicoIngresoSalidaDb.ConceptoIngreEgreID = hisDto.ConceptosIngreEgre.ConceptoIngreEgreID;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                historicoIngresoSalidaDb.UsuarioID = usuarioID;

                context.HistoricoIngresoSalidas.Add(historicoIngresoSalidaDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                hisDto.HistoricoIngresoSalidaID = historicoIngresoSalidaDb.HistoricoIngresoSalidaID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el historico de ingreso egreso : " + hisDto.HistoricoIngresoSalidaID,
                    ObjetoDto = hisDto
                };
            }
        }

        public MensajeDto EliminarHistoricoSalario(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoIngresoSalidaDb = context.HistoricoIngresoSalidas
                   .Where(h => h.HistoricoIngresoSalidaID == id)
                   .FirstOrDefault();
                if (historicoIngresoSalidaDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de ingreso salida : "
                        + id
                    };
                }
                context.HistoricoIngresoSalidas.Remove(historicoIngresoSalidaDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el historico de ingreso/salida : " + id
                };
            }
        }

        private MensajeDto EditarHistoricoIngresoSalida(HistoricoIngresoSalidaDto hisDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoIngresoSalidaDb = context.HistoricoIngresoSalidas
                    .Where(h => h.HistoricoIngresoSalidaID == hisDto.HistoricoIngresoSalidaID)
                    .FirstOrDefault();
                if (historicoIngresoSalidaDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el historico de ingreso salida : "
                        + hisDto.HistoricoIngresoSalidaID
                    };
                }
                historicoIngresoSalidaDb.FechaIngreso = hisDto.FechaIngreso;
                historicoIngresoSalidaDb.FechaSalida = hisDto.FechaSalida;
                historicoIngresoSalidaDb.MotivoSalida = hisDto.MotivoSalida;
                historicoIngresoSalidaDb.MotivoIngreso = hisDto.MotivoIngreso;
                historicoIngresoSalidaDb.ConceptoIngreEgreID = hisDto.ConceptosIngreEgre.ConceptoIngreEgreID;

                context.Entry(historicoIngresoSalidaDb).State = System.Data.Entity.EntityState.Modified;

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el historico de Ingreso/Salida : " + hisDto.HistoricoIngresoSalidaID,
                    ObjetoDto = hisDto
                };
            }
        }

        public MensajeDto UltimoIngreso(long empleadoID, DateTime? fechaHasta = null) {
            using (var context = new SueldosJornalesEntities()) {
                // Se ve el registro del ultimo ingreso
                var ingresoSalidaDb = context.HistoricoIngresoSalidas
                    .Where(h => h.EmpleadoID == empleadoID && h.ConceptoIngreEgreID == 1) // a empresa
                    .OrderByDescending(h => h.FechaIngreso)
                    .FirstOrDefault();
                // Se ve si existe un ultimo ingreso con fecha de salida hasta el parametro fechaHasta
                if (fechaHasta != null) {
                    var ingresoSalidaDb2 = context.HistoricoIngresoSalidas
                       .Where(h => h.EmpleadoID == empleadoID && h.ConceptoIngreEgreID == 1 &&
                                   h.FechaSalida <= fechaHasta.Value) // a empresa
                       .OrderByDescending(h => h.FechaIngreso)
                       .FirstOrDefault();
                    if (ingresoSalidaDb2 != null) {
                        if(ingresoSalidaDb2.FechaSalida.Value.Year == fechaHasta.Value.Year) {
                            ingresoSalidaDb = ingresoSalidaDb2;
                        }
                    }
                }

                if (ingresoSalidaDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existen datos de Ingreso y egreso"
                    };
                }
                var hismDto = new HistoricoIngresoSalidaDto();
                hismDto.EmpleadoID = ingresoSalidaDb.EmpleadoID;
                hismDto.FechaIngreso = ingresoSalidaDb.FechaIngreso;
                hismDto.FechaSalida = ingresoSalidaDb.FechaSalida;
                hismDto.MotivoSalida = ingresoSalidaDb.MotivoSalida;
                hismDto.MotivoIngreso = ingresoSalidaDb.MotivoIngreso;
                hismDto.ConceptosIngreEgre = new ConceptosIngreEgreDto() {
                    ConceptoIngreEgreID = ingresoSalidaDb.ConceptoIngreEgreID,
                    Concepto = ingresoSalidaDb.ConceptosIngreEgre.Concepto
                };

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Ultimo dato de ingreso a la empresa",
                    ObjetoDto = hismDto
                };
            }
        }
        /// <summary>
        /// Calcula si el emplado trabaja todavia en la empresa en la fecha actual
        /// </summary>
        /// <param name="empleadoID"></param>
        /// <returns></returns>
        public static bool EmpleadoTrabajaTodaviaEnLaEmpresa(long empleadoID) {
            var fechaHoy = DateTime.Today;
            return EmpleadoTrabajaTodaviaEnLaEmpresa(empleadoID, fechaHoy.Month, fechaHoy.Year);
        }
        /// <summary>
        /// Calcula si el empleado trabaja todavia en la empresa
        /// </summary>
        /// <param name="empleadoID"></param>
        /// <param name="mes">Es el mes activo, se calcula hacia el pasado, por lo tanto si alguien sale despues del mes activo se considera como que trabaja todavia en la empresa</param>
        /// <param name="year"></param>
        /// <param name="salidoEnElMesActivo">True para considerar alque salen en el mes activo, false para marcar como salido al que sale no importa si sale en el mes activo</param>
        /// <returns></returns>
        public static bool EmpleadoTrabajaTodaviaEnLaEmpresa(long empleadoID, int mes, int year, bool salidoEnElMesActivo = true) {
            using (var context = new SueldosJornalesEntities()) {
                var mesSeleccionado = new DateTime(year, mes, DateTime.DaysInMonth(year, mes));
                //Se busca el ultimo movimiento del empleado, concepto salida de la empresa
                var historico = context.HistoricoIngresoSalidas
                    .Where(h => h.EmpleadoID == empleadoID && h.ConceptoIngreEgreID == 1 && //Concepto IngreEgre a empresa
                    h.FechaIngreso <= mesSeleccionado)
                    .OrderByDescending(h => h.FechaIngreso)
                    .FirstOrDefault();
                if (historico == null) {
                    return false;
                }
                //Se ve si el ultimo historico tiene fecha de salida.
                if (historico.FechaSalida == null) {
                    return true;
                } else {
                    //Se ve si salio dentro del mes actual, si es asi se considera que todavia trabaja
                    if (salidoEnElMesActivo) {
                        if (historico.FechaSalida.Value.Year == year &&
                            historico.FechaSalida.Value.Month == mes) {
                            return true;
                        }
                    }
                    //Se ve si su fecha de ingreso todavia esta en el futuro con respecto
                    //a la fecha seleccionada
                    if (historico.FechaIngreso > mesSeleccionado) {
                        return false;
                    }
                    //Se ve si la fecha de salida esta todavia en el futuro o ya paso
                    if (historico.FechaSalida > mesSeleccionado) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }
        public static DateTime? fechaSalida(long empleadoID, DateTime fechaDondeSeEsta) {
            using (var context = new SueldosJornalesEntities()) {
                //Se busca el ultimo movimiento del empleado
                var historico = context.HistoricoIngresoSalidas
                    .Where(h => h.ConceptoIngreEgreID == 1 && //Es una entrada y salida de la empresa
                           h.EmpleadoID == empleadoID &&
                           h.FechaSalida < fechaDondeSeEsta)
                    .OrderByDescending(h => h.MomentoCarga)
                    .FirstOrDefault();
                if (historico == null) {
                    return null;
                }
                return historico.FechaSalida.Value;
            }
        }

        internal static int DiasReposoEnElMes(long empleadoID, DateTime mesLiquidacion) {
            var ultimoDiaMesLiquidacion = new DateTime(mesLiquidacion.Year, mesLiquidacion.Month, DateTime.DaysInMonth(mesLiquidacion.Year, mesLiquidacion.Month));
            using (var context = new SueldosJornalesEntities()) {
                var historicoReposo = context.HistoricoIngresoSalidas
                    .Where(w => ultimoDiaMesLiquidacion >= w.FechaIngreso &&
                                mesLiquidacion <= w.FechaSalida &&
                               w.ConceptoIngreEgreID == 2 &&
                               w.EmpleadoID == empleadoID)//Concepto de reposo
                    .OrderBy(o => o.FechaIngreso)
                    .FirstOrDefault();
                var cantidadReposo = 0;
                if (historicoReposo != null) {
                    if (mesLiquidacion > historicoReposo.FechaIngreso &&
                        ultimoDiaMesLiquidacion < historicoReposo.FechaSalida.Value) {// Cando el rengo no esta dentro del mes de liquidacion
                        cantidadReposo = ultimoDiaMesLiquidacion.Day; // Por si el mes es 31
                    } else if (historicoReposo.FechaIngreso.Month == mesLiquidacion.Month && historicoReposo.FechaIngreso.Year == mesLiquidacion.Year &&
                        historicoReposo.FechaSalida.Value.Month == mesLiquidacion.Month && historicoReposo.FechaSalida.Value.Year == mesLiquidacion.Year) {
                        cantidadReposo = ultimoDiaMesLiquidacion.Day - ((historicoReposo.FechaIngreso.Day - 1) + (ultimoDiaMesLiquidacion.Day - historicoReposo.FechaSalida.Value.Day));
                    } else {
                        if (historicoReposo.FechaIngreso.Month == mesLiquidacion.Month) {
                            cantidadReposo = ultimoDiaMesLiquidacion.Day - (historicoReposo.FechaIngreso.Day - 1);
                        } else {
                            cantidadReposo = historicoReposo.FechaSalida.Value.Day;
                        }
                    }
                }
                return cantidadReposo;
            }
        }
    }
}
