using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace SueldosYjornales.Controllers.Api {
    public class HistoricoDireccionesController : ApiController {
        // GET: api/HistoricoDirecciones
        public HttpResponseMessage Get() {
            HistoricoDireccionesManagers hdm = new HistoricoDireccionesManagers();
            List<HistoricoDireccioneDto> listado = hdm.ListadoHistoricoDirecciones();
            return Request.CreateResponse<List<HistoricoDireccioneDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/HistoricoDirecciones/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadosID(long empleadoID) {
            HistoricoDireccionesManagers hdm = new HistoricoDireccionesManagers();
            List<HistoricoDireccioneDto> listado = hdm.ListadoHistoricoDirecciones(empleadoID);
            return Request.CreateResponse<List<HistoricoDireccioneDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/HistoricoDirecciones/DireccionActual")]
        public HttpResponseMessage GetDireccionActual(long empleadoID) {
            HistoricoDireccionesManagers hdm = new HistoricoDireccionesManagers();
            MensajeDto mensaje = hdm.DireccionaActual(empleadoID);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // GET: api/HistoricoDirecciones/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/HistoricoDirecciones
        public HttpResponseMessage Post(HistoricoDireccioneDto hdDto) {
            HistoricoDireccionesManagers hdm = new HistoricoDireccionesManagers();
            MensajeDto mensaje = hdm.CargarHistoricoDirecciones(hdDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/HistoricoDirecciones/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/HistoricoDirecciones/5
        public HttpResponseMessage Delete(int id) {
            HistoricoDireccionesManagers hdm = new HistoricoDireccionesManagers();
            MensajeDto mensaje = hdm.EliminarHistoricoDireccion(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
