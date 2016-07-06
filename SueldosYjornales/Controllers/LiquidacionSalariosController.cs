using Newtonsoft.Json;
using SYJ.Application.Dto.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SueldosYjornales.Controllers
{
     [Authorize(Roles = "LiquidadorSalario")]
    public class LiquidacionSalariosController : Controller
    {
        // GET: LiquidacionSalarios
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImpresionLiqSalarios(string jsonInput) {
            ViewBag.flDto = jsonInput;
            return View();
        }
        public ActionResult ImpresionLiqAguinaldo(string jsonInput) {
            ViewBag.flDto = jsonInput;
            return View();
        }
        public ActionResult ImpresionLiqAguinaldoReporte(string jsonInput) {
            ViewBag.flDto = jsonInput;
            return View();
        }
    }
}