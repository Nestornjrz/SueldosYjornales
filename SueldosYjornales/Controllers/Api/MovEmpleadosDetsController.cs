using SYJ.Application.Dto;
using SYJ.Application.Dto.Auxiliares;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api {
    [Authorize]
    public class MovEmpleadosDetsController : ApiController {
        // GET: api/MovEmpleadosDets
        [HttpGet]
        public HttpResponseMessage Get(DateTime fechaDesde,
            DateTime fechaHasta, long empleadoID) {
            var listado = MovEmpleadosDetsManagers.ListadoMovimientos(
                 fechaDesde,
                 fechaHasta,
                 empleadoID);
            return Request.CreateResponse<List<MovEmpleadoDetDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/MovEmpleadosDets/PorMes")]
        public HttpResponseMessage GetXmes(DateTime fechaDesde,
           DateTime fechaHasta, long empleadoID) {
            var listado = MovEmpleadosDetsManagers.ListadoMovimientos(
                 fechaDesde,
                 fechaHasta,
                 empleadoID);
            var listadoAgrupado = MovEmpleadosDetsManagers.MovimientosEmpleadoXmes(listado);
            return Request.CreateResponse<List<MovimientosEmpleadoXmesDto>>(HttpStatusCode.OK, listadoAgrupado);
        }

        // GET: api/MovEmpleadosDets/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/MovEmpleadosDets
        public void Post([FromBody]string value) {
        }

        // PUT: api/MovEmpleadosDets/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/MovEmpleadosDets/5
        public void Delete(int id) {
        }
    }
}
