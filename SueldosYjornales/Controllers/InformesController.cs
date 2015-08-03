using SYJ.Application.Dto.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SueldosYjornales.Controllers {
    public class InformesController : Controller {
        // GET: Informes
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult LiquidacionSueldo(LiquidacionSueldoFormDto lsfDto) {
            HttpRequestBase request  = ControllerContext.HttpContext.Request;
            var empresa = request.Form.Get("empresa");
            return View();
        }
    }
}