using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace SueldosYjornales.Controllers.Api
{
    [Authorize]
    public class HistoricoTelefonosController : ApiController
    {
        // GET: api/HistoricoTelefonos
        public HttpResponseMessage Get()
        {
            HistoricoTelefonosManagers htm = new HistoricoTelefonosManagers();
            List<HistoricoTelefonoDto> listado = htm.ListadoHistoricoTelefonos();
            return Request.CreateResponse<List<HistoricoTelefonoDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/HistoricoTelefonos/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadoID(long empleadoID) {
            HistoricoTelefonosManagers htm = new HistoricoTelefonosManagers();
            List<HistoricoTelefonoDto> listado = htm.ListadoHistoricoTelefonos(empleadoID);
            return Request.CreateResponse<List<HistoricoTelefonoDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/HistoricoTelefonos/UltimoTelefono")]
        public HttpResponseMessage GetUltimoTelefono(long empleadoID) {
            HistoricoTelefonosManagers htm = new HistoricoTelefonosManagers();
            MensajeDto mensaje = htm.UltimoTelefono(empleadoID);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // GET: api/HistoricoTelefonos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/HistoricoTelefonos
        public HttpResponseMessage Post(HistoricoTelefonoDto htDto)
        {
            HistoricoTelefonosManagers htm = new HistoricoTelefonosManagers();
            MensajeDto mensaje = htm.CargarHistoricoTelefono(htDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/HistoricoTelefonos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/HistoricoTelefonos/5
        public HttpResponseMessage Delete(int id)
        {
            HistoricoTelefonosManagers htm = new HistoricoTelefonosManagers();
            MensajeDto mensaje = htm.EliminarHistoricoTelefono(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
