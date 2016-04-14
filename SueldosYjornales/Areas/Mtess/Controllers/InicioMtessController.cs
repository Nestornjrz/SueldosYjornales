using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SueldosYjornales.Areas.Mtess.Controllers
{
    [Authorize(Roles = "Padrones,Operador,Admin,Supervisor,LiquidadorSalario")]
    public class InicioMtessController : Controller
    {
        // GET: Mtess/InicioMtess
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerarArchivos() {
            return View();
        }
    }
}
