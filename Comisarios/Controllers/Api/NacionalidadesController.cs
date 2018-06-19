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
    [RoutePrefix("api/Nacionalidades")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class NacionalidadesController : ApiController {
        [HttpGet]
        public IHttpActionResult GetProfesiones() {
            NacionalidadesManagers nm = new NacionalidadesManagers();
            List<NacionalidadeDto> listado = nm.ListadoNacionalidades();

            if (listado == null) {
                return NotFound();
            }
            return Ok(listado);
        }
    }
}
