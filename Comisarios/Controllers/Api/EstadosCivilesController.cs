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
    [RoutePrefix("api/EstadosCiviles")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class EstadosCivilesController : ApiController {
        [HttpGet]
        public IHttpActionResult GetProfesiones() {
            EstadosCivilesManagers ecm = new EstadosCivilesManagers();
            List<EstadoCivileDto> listado = ecm.ListadoEstadosCiviles();
            if (listado == null) {
                return NotFound();
            }
            return Ok(listado);
        }
    }
}
