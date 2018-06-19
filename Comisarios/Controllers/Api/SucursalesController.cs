using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Comisarios.Controllers.Api {
    [RoutePrefix("api/Sucursales")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class SucursalesController : ApiController {
        [HttpGet]
        public IHttpActionResult GetProfesiones() {
            SucursalesManagers sm = new SucursalesManagers();
            List<SucursaleDto> listado = sm.ListadoSucursales();

            if (listado == null) {
                return NotFound();
            }
            return Ok(listado);
        }
    }
}
