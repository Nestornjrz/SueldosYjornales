using SYJ.Application.Dto;
using SYJ.Application.Dto.Auxiliares;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SYJ.Domain.Managers {
    public class MovEmpleadosDetsManagers {
        public decimal TotalSalarioPercibidoMes(int mesID, int year, long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var movimientos = context.MovEmpleadosDets
                    .Where(m => m.EmpleadoID == empleadoID &&
                              m.MesAplicacion.Year == year &&
                              m.MesAplicacion.Month == mesID &&
                              m.DevCred == Liquidacion.DevCred.Credito &&
                              (m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.SueldoBase ||
                               m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Comision))
                               .ToList();
                decimal montoTotalCobrado = 0;
                if (movimientos != null) {
                    montoTotalCobrado = movimientos.Sum(s => s.Monto);
                }
                return Math.Round(montoTotalCobrado);
            }
        }
        /// <summary>
        /// Calcula la cantidad de horas trabajadas con 8 horas por dia dentro del mes
        /// sin tener el cuenta el domingo
        /// </summary>
        /// <param name="mesID"></param>
        /// <param name="year"></param>
        /// <param name="empleadoID">Por ahora este parametro esta solo por si acaso
        /// en el futuro el calculo es mas fino</param>
        /// <returns></returns>
        public int TotalHorasTrabajadas(int mesID, int year, long empleadoID) {
            //feriados
            List<DateTime> feriados = new List<DateTime>();
            feriados.Add(new DateTime(2015, 08, 15));//Fundacion de asuncion
            feriados.Add(new DateTime(2015, 09, 29));//Batalla de boqueron
            feriados.Add(new DateTime(2015, 12, 8));//Dia de la virgen de caacupe
            feriados.Add(new DateTime(2015, 12, 25));//Navidad

            //Se calcula la fecha de salida
            var fechaSalida = HistoricoIngresoSalidasManagers.fechaSalida(empleadoID,
                new DateTime(year, mesID, DateTime.DaysInMonth(year, mesID)));

            int cantidadHoras = 0;
            for (int i = 1; i < DateTime.DaysInMonth(year, mesID); i++) {
                var fecha = new DateTime(year, mesID, i);
                if (fechaSalida != null) {
                    if (fecha > fechaSalida) {
                        break;
                    }
                }
                if (fecha.DayOfWeek != DayOfWeek.Sunday && !feriados.Contains(fecha)) {
                    cantidadHoras += 8;
                }
            }
            return cantidadHoras;
        }
        public decimal Aguinaldo(int year, long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var aguinaldos = context.MovEmpleadosDets
                    .Where(m => m.EmpleadoID == empleadoID &&
                              m.MesAplicacion.Year == year &&
                              m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Aguinaldo)
                              .ToList();
                decimal montoAguinaldo = 0;
                if (aguinaldos != null) {
                    montoAguinaldo = aguinaldos.Sum(s => s.Monto);
                }
                return montoAguinaldo;
            }
        }
        public static List<MovEmpleadoDetDto> ListadoMovimientos(DateTime fechaDesde, DateTime fechaHasta, long empleadoID) {
            //Se recupera el empleado, para cargar sus datos
            EmpleadoDto empleado = EmpleadosManagers.GetEmpleado(empleadoID);
            //Ahora recuperar todos sus movimientos
            using (var context = new SueldosJornalesEntities()) {
                var movimientos = context.MovEmpleadosDets
                    .Where(m => m.EmpleadoID == empleadoID &&
                                m.MesAplicacion >= fechaDesde &&
                                m.MesAplicacion <= fechaHasta)
                    .ToList();
                var movimientosDto = new List<MovEmpleadoDetDto>();
                foreach (MovEmpleadosDet s in movimientos) {
                    var mov = new MovEmpleadoDetDto();
                    mov.MovEmpleadoDetID = s.MovEmpleadoDetID;
                    mov.MovEmpleadoID = s.MovEmpleadoID;
                    mov.Empleado = empleado;
                    mov.Debito = (s.DevCred == true) ? s.Monto : 0;//Devito
                    mov.Credito = (s.DevCred == false) ? s.Monto : 0;//Devito
                    mov.MesAplicacion = s.MesAplicacion;
                    mov.LiquidacionConcepto = new LiquidacionConceptoDto() {
                        LiquidacionConceptoID = s.LiquidacionConceptoID,
                        NombreConcepto = context.LiquidacionConceptos
                                        .Where(l => l.LiquidacionConceptoID == s.LiquidacionConceptoID)
                                        .First().NombreConcepto
                    };
                    movimientosDto.Add(mov);
                }
                return movimientosDto.OrderByDescending(o => o.MesAplicacion.Year)
                    .ThenByDescending(o => o.MesAplicacion.Month)
                    .ThenByDescending(o => o.Debito)
                    .ToList();
            }
        }
        public static List<MovimientosEmpleadoXmesDto> MovimientosEmpleadoXmes(List<MovEmpleadoDetDto> lisMov) {
            var agrupacionXmes = new List<MovimientosEmpleadoXmesDto>();
            var porMes = lisMov
                         .GroupBy(l =>
                                  new { l.MesAplicacion.Year, l.MesAplicacion.Month },
                                  (key, g) => new {
                                      mes = new DateTime(key.Year, key.Month, 1),
                                      movimientos = g.ToList()
                                  }
                         );
            foreach (var grupo in porMes) {
                var movM = new MovimientosEmpleadoXmesDto();
                movM.MesAplicacion = grupo.mes;
                //Calculo del saldo
                decimal saldo = 0;
                foreach (MovEmpleadoDetDto me in grupo.movimientos) {
                    saldo += me.Debito - me.Credito;
                    me.Saldo = saldo;
                }
                movM.Movimientos = grupo.movimientos;
                movM.CabeceraLiquidacionSalario = RecuperarCabeceraLiq(grupo.movimientos);
                agrupacionXmes.Add(movM);
            }
            return agrupacionXmes;
        }

        private static MovEmpleadoDto RecuperarCabeceraLiq(List<MovEmpleadoDetDto> movimientos) {
            var movTotalPagado = movimientos
                .Where(m => m.LiquidacionConcepto.LiquidacionConceptoID == (int)Liquidacion.Conceptos.TotalPagado)
                .FirstOrDefault();
            if (movTotalPagado != null) {
                using (var context = new SueldosJornalesEntities()) {
                    var cabecera = context.MovEmpleados
                        .Where(m => m.MovEmpleadoID == movTotalPagado.MovEmpleadoID)
                        .Select(s => new MovEmpleadoDto() {
                            MovEmpleadoID = s.MovEmpleadoID,
                            FechaMovimiento = s.FechaMovimiento,
                            Descripcion = s.Descripcion
                        }).First();
                    return cabecera;
                }
            }
            return null;
        }
    }
}
