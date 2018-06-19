using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Comisarios.Controllers.Api {
    [RoutePrefix("api/Profesiones")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class ProfesionesController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetProfesiones() {
            ProfesionesManagers pm = new ProfesionesManagers();
            List<ProfesioneDto> listado = pm.ListadoProfesiones();

            if (listado == null) {
                return NotFound();
            }
            return Ok(listado);
        }
    }
}
