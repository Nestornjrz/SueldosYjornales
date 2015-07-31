using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SueldosYjornales.Controllers
{
    public class PadronesController : Controller
    {
        // GET: Padrones
         [Authorize(Roles = "Padrones,Operador,Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}