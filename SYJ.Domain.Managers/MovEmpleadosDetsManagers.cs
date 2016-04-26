using SYJ.Domain.Db;
using SYJ.Domain.Managers.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class MovEmpleadosDetsManagers {
        public decimal TotalSalarioPercibidoMes(int mesID, int year, long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var montoTotalCobrado = context.MovEmpleadosDets
                    .Where(m => m.EmpleadoID == empleadoID &&
                              m.MesAplicacion.Year == year &&
                              m.MesAplicacion.Month == mesID &&
                              m.DevCred == Liquidacion.DevCred.Devito &&
                              (m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.SueldoBase ||
                               m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Comision))
                               .Sum(s => s.Monto);
                return montoTotalCobrado;
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
            int cantidadHoras = 0;
            for (int i = 1; i < DateTime.DaysInMonth(year, mesID); i++) {
                var fecha = new DateTime(year, mesID, i);
                if (fecha.DayOfWeek != DayOfWeek.Sunday) {
                    cantidadHoras += 8;
                }
            }
            return cantidadHoras;
        }
        public decimal Aguinaldo(int year, long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var montoAguinaldo = context.MovEmpleadosDets
                    .Where(m => m.EmpleadoID == empleadoID &&
                              m.MesAplicacion.Year == year &&
                              m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Aguinaldo)
                              .Sum(s => s.Monto);
                return montoAguinaldo;
            }
        }
    }
}
