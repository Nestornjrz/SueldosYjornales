using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Auxiliares;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace SYJ.Domain.Managers {
    public class PrestamosSimplesManagers {
        public List<PrestamoSimpleDto> ListadoPrestamo(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.PrestamosSimples
                    .Where(p => p.EmpleadoID == empleadoID)
                    .Select(s => new PrestamoSimpleDto() {
                        PrestamoSimpleID = s.PrestamoSimpleID,
                        EmpleadoID = s.EmpleadoID,
                        Fecha1erVencimiento = s.Fecha1erVencimiento,
                        Monto = s.Monto,
                        Cuotas = s.Cuotas,
                        MovEmpleadoID = s.MovEmpleadoID,
                        Observacion = s.Observacion
                    }).ToList();
                return listado;
            }
        }
        /// <summary>
        /// Las cuotas son debitos en los movimientos del empleado
        /// por lo tanto se colocan los debitos que son las cuotas
        /// </summary>
        /// <param name="empleadoID"></param>
        /// <returns></returns>
        public List<PrestamoSimpleDto> ListadoPrestamoConSusDebitos(long empleadoID) {
            var prestamosSimples = this.ListadoPrestamo(empleadoID);
            using (var context = new SueldosJornalesEntities()) {
                foreach (PrestamoSimpleDto p in prestamosSimples) {
                    var movimientosDto = new List<MovEmpleadoDetDto>();
                    var cuotasMov = context.MovEmpleadosDets
                       .Where(m => m.MovEmpleadoID == p.MovEmpleadoID).ToList();
                    foreach (MovEmpleadosDet meDb in cuotasMov) {
                        var mov = new MovEmpleadoDetDto();
                        mov.MovEmpleadoDetID = meDb.MovEmpleadoDetID;
                        mov.MovEmpleadoID = meDb.MovEmpleadoID;
                        //mov.Empleado = empleado;
                        mov.Debito = (meDb.DevCred == true) ? meDb.Monto : 0;//Devito
                        mov.Credito = (meDb.DevCred == false) ? meDb.Monto : 0;//Devito
                        mov.MesAplicacion = meDb.MesAplicacion;
                        mov.LiquidacionConcepto = new LiquidacionConceptoDto() {
                            LiquidacionConceptoID = meDb.LiquidacionConceptoID,
                            NombreConcepto = context.LiquidacionConceptos
                                            .Where(l => l.LiquidacionConceptoID == meDb.LiquidacionConceptoID)
                                            .First().NombreConcepto
                        };
                        mov.MovEmpleadoIDdeLaLiquidacion = RecuperarIDliquiSalario(mov.MesAplicacion, meDb.EmpleadoID);
                        movimientosDto.Add(mov);
                    }
                    p.CuotasMov = movimientosDto;
                    p.SumaMontoCuotas = p.CuotasMov.Sum(s => s.Debito);
                }
            }
            return prestamosSimples;
        }

        private long RecuperarIDliquiSalario(DateTime mesAplicacion, long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var movTotalPagado = context.MovEmpleadosDets
                    .Where(m => m.MesAplicacion.Year == mesAplicacion.Year &&
                                m.MesAplicacion.Month == mesAplicacion.Month &&
                                m.EmpleadoID == empleadoID &&
                                m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.TotalPagado)
                    .FirstOrDefault();
                if (movTotalPagado != null) {
                    return movTotalPagado.MovEmpleadoID;
                }
                return 0;
            }
        }

        public MensajeDto CargarPrestamo(PrestamoSimpleDto psDto, Guid userID) {
            if (psDto.PrestamoSimpleID > 0) {
                return EditarPrestamo(psDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var prestamoSimpleDb = new PrestamosSimple();
                prestamoSimpleDb.EmpleadoID = psDto.EmpleadoID;
                prestamoSimpleDb.Fecha1erVencimiento = psDto.Fecha1erVencimiento;
                prestamoSimpleDb.Monto = psDto.Monto;
                prestamoSimpleDb.Cuotas = psDto.Cuotas;
                prestamoSimpleDb.Observacion = psDto.Observacion;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                prestamoSimpleDb.UsuarioID = usuarioID;
                prestamoSimpleDb.MomentoCarga = DateTime.Now;

                context.PrestamosSimples.Add(prestamoSimpleDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                psDto.PrestamoSimpleID = prestamoSimpleDb.PrestamoSimpleID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el prestamo: " + psDto.PrestamoSimpleID,
                    ObjetoDto = psDto
                };
            }
        }
        private MensajeDto EditarPrestamo(PrestamoSimpleDto psDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var prestamoSimpleDb = context.PrestamosSimples
                 .Where(p => p.PrestamoSimpleID == psDto.PrestamoSimpleID)
                 .FirstOrDefault();
                if (prestamoSimpleDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el prestamo id : " + psDto.PrestamoSimpleID
                    };
                }
                prestamoSimpleDb.Fecha1erVencimiento = psDto.Fecha1erVencimiento;
                prestamoSimpleDb.Monto = psDto.Monto;
                prestamoSimpleDb.Cuotas = psDto.Cuotas;
                prestamoSimpleDb.Observacion = psDto.Observacion;

                context.Entry(prestamoSimpleDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el prestamo: " + psDto.PrestamoSimpleID,
                    ObjetoDto = psDto
                };
            }
        }
        public MensajeDto EliminarPrestamo(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var prestamoSimpleDb = context.PrestamosSimples
                    .Where(p => p.PrestamoSimpleID == id)
                    .FirstOrDefault();
                if (prestamoSimpleDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el prestamo id : " + id
                    };
                }
                context.PrestamosSimples.Remove(prestamoSimpleDb);
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el prestamo : " + id
                };
            }
        }
        /// <summary>
        /// Listado de prestamos hasta el año y el mes especificados
        /// </summary>
        /// <param name="empleadoID"></param>
        /// <param name="year"></param>
        /// <param name="mesID"></param>
        /// <returns></returns>
        internal List<PrestamoSimpleDto> ListadoPrestamo(long empleadoID, int year, int mesID) {
            using (var context = new SueldosJornalesEntities()) {
                DateTime dtComienzoMes = new DateTime(year, mesID, 1);
                DateTime dtFinMes = new DateTime(year, mesID, DateTime.DaysInMonth(year, mesID));

                List<PrestamoSimpleDto> listado = context.PrestamosSimples
                   .Where(p => p.EmpleadoID == empleadoID &&
                               dtFinMes >= p.Fecha1erVencimiento &&
                               dtComienzoMes <= SqlFunctions.DateAdd("month", p.Cuotas, p.Fecha1erVencimiento))
                   .Select(s => new PrestamoSimpleDto() {
                       PrestamoSimpleID = s.PrestamoSimpleID,
                       MovEmpleadoID = s.MovEmpleadoID,
                       EmpleadoID = s.EmpleadoID,
                       Empleado = new EmpleadoDto() {
                           EmpleadoID = s.EmpleadoID,
                           Nombres = s.Empleado.Nombres,
                           Apellidos = s.Empleado.Apellidos
                       },
                       Fecha1erVencimiento = s.Fecha1erVencimiento,
                       Monto = s.Monto,
                       Cuotas = s.Cuotas,
                       Observacion = s.Observacion
                   }).ToList();

                //Se recuperan las cuotas para saber si entran dentro del mes solicitado
                var listadoFiltrado = new List<PrestamoSimpleDto>();

                listado.ForEach(delegate (PrestamoSimpleDto ps) {
                    var movEmpleadosDets = context.MovEmpleadosDets
                        .Where(m => m.MovEmpleadoID == ps.MovEmpleadoID &&
                                    m.MesAplicacion.Month == mesID).ToList();
                    if (movEmpleadosDets.Count > 0) {
                        listadoFiltrado.Add(ps);
                    }
                });

                return listadoFiltrado;
            }
        }
    }
}
