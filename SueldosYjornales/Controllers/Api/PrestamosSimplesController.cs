using Microsoft.AspNet.Identity;
using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api {
    [Authorize]
    public class PrestamosSimplesController : ApiController {
        // GET: api/PrestamosSimples
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/PrestamosSimples/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadoID(long empleadoID) {
            PrestamosSimplesManagers psm = new PrestamosSimplesManagers();
            List<PrestamoSimpleDto> listado = psm.ListadoPrestamo(empleadoID);
            return Request.CreateResponse<List<PrestamoSimpleDto>>(HttpStatusCode.OK, listado);
        }
        [HttpGet]
        [Route("api/PrestamosSimples/ByEmpleadoID/ConCuotas")]
        public HttpResponseMessage GetByEmpleadoIDConCuotas(long empleadoID) {
            PrestamosSimplesManagers psm = new PrestamosSimplesManagers();
            List<PrestamoSimpleDto> listado = psm.ListadoPrestamoConSusDebitos(empleadoID);
            return Request.CreateResponse<List<PrestamoSimpleDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/PrestamosSimples/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/PrestamosSimples
        public HttpResponseMessage Post(PrestamoSimpleDto psDto) {
            PrestamosSimplesManagers psm = new PrestamosSimplesManagers();
            MensajeDto mensaje = psm.CargarPrestamo(psDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/PrestamosSimples/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/PrestamosSimples/5
        public HttpResponseMessage Delete(int id) {
            PrestamosSimplesManagers psm = new PrestamosSimplesManagers();
            MensajeDto mensaje = psm.EliminarPrestamo(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
