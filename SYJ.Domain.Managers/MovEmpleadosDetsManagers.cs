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
            //feriados
            List<DateTime> feriados = new List<DateTime>();
            feriados.Add(new DateTime(2015, 08, 15));//Fundacion de asuncion
            feriados.Add(new DateTime(2015, 09, 29));//Batalla de boqueron
            feriados.Add(new DateTime(2015, 12, 8));//Dia de la virgen de caacupe
            feriados.Add(new DateTime(2015, 12, 25));//Navidad

            int cantidadHoras = 0;
            for (int i = 1; i < DateTime.DaysInMonth(year, mesID); i++) {
                var fecha = new DateTime(year, mesID, i);
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
    }
}
