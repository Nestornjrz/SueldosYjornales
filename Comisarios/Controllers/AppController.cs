using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Comisarios.Controllers
{
    public class AppController : Controller
    {
        private Uri _baseUri;
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {

            var uri = HttpContext.GetOwinContext().Request.Uri;
            string comisarios = "";
            if (uri.Authority == "www.laaragonesa.com.py") {
                comisarios = "/Comisarios";
            }
            var baseUriString = uri.Scheme + "://" + uri.Authority + comisarios;
            _baseUri = new Uri(baseUriString);

            base.OnActionExecuting(filterContext);
        }
        // GET: App
        public ActionResult Index()
        {
            ViewBag.Message = "Comisarios";
            ViewBag.baseUri = this._baseUri;
            return View();
        }
    }
}