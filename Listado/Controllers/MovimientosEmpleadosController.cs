using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Listado.Controllers {
    public class MovimientosEmpleadosController : Controller {
        // GET: MovimientosEmpleados
        public ActionResult Index() {
            var fechaDesde = new DateTime(2016, 7, 1);
            var fechaHasta = new DateTime(2017, 6, 30);
            //Se recupera el listado de empleados
            var em = new EmpleadosManagers();
            var empleados = em.ListadoEmpleadosConMarcaDeActivo(fechaHasta);

            var listadoResultado = new List<MovEmpleadoDetDto>();

            foreach (EmpleadoDto emp in empleados) {
                var listadoMovDet = MovEmpleadosDetsManagers.ListadoMovimientos(
                fechaDesde, fechaHasta, emp.EmpleadoID);
                foreach (MovEmpleadoDetDto mov in listadoMovDet) {
                    mov.Empleado = emp;
                }
                listadoResultado.AddRange(listadoMovDet);
            }

            ViewBag.movimientos = listadoResultado;
            return View();
        }
    }
}