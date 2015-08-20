using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SueldosYjornales.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        // GET: Empleados
        public ActionResult Index()
        {
            return View();
        }

        // GET: Empleados/Details/5
        public ActionResult Details(int id) {
            ViewBag.empleadoID = id;
            return View();
        }

        // GET: Empleados/Historicos/5
        public ActionResult Historicos(int id) {
            ViewBag.empleadoID = id;
            return View();
        }

        // GET: Empleados/Resumen/5
        public ActionResult Resumen(int id) {
            ViewBag.empleadoID = id;
            return View();
        }

        public ActionResult SubirImagenes(int id) {
            ViewBag.empleadoID = id;
            return View();
        }
        public ActionResult ImprimirCedula(int id) {
            ViewBag.empleadoID = id;
            return View();
        }
    }
}