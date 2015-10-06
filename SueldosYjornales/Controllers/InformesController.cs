using Newtonsoft.Json;
using SYJ.Application.Dto.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SueldosYjornales.Controllers {
    [Authorize(Roles = "Padrones,Operador,Admin,Supervisor,LiquidadorSalario")]
    public class InformesController : Controller {
        // GET: Informes
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        /// Este metodo sera eliminado porque ya es reemplazado por otro
        public ActionResult LiquidacionSueldo(string jsonInput) {
            LiquidacionSueldoFormDto lsfDto = JsonConvert.DeserializeObject<LiquidacionSueldoFormDto>(jsonInput);
            ViewBag.lsfDto = jsonInput;
            return View();
        }

        [HttpPost]
        public ActionResult PlanillaSalarios(string jsonInputPlanillaSalarios) {
            ViewBag.psfDto = jsonInputPlanillaSalarios;
            return View();
        }
        [HttpPost]
        public ActionResult ListadoPrestamos(string jsonInput) {
            ViewBag.psfDto = jsonInput;
            return View();
        }
    }
}