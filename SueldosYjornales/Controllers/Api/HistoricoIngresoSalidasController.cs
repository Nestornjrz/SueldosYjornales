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
    public class HistoricoIngresoSalidasController : ApiController
    {
        // GET: api/HistoricoIngresoSalidas
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/HistoricoIngresoSalidas/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadoID(long empleadoID) {
            HistoricoIngresoSalidasManagers hism = new HistoricoIngresoSalidasManagers();
            List<HistoricoIngresoSalidaDto> listado = hism.ListadoHistoricoIngresoSalidas(empleadoID);
            return Request.CreateResponse<List<HistoricoIngresoSalidaDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/HistoricoIngresoSalidas/UltimoIngreso")]
        public HttpResponseMessage GetUltimoIngreso(long empleadoID) {
            HistoricoIngresoSalidasManagers hism = new HistoricoIngresoSalidasManagers();
            MensajeDto mensaje = hism.UltimoIngreso(empleadoID);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // GET: api/HistoricoIngresoSalidas/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/HistoricoIngresoSalidas
        public HttpResponseMessage Post(HistoricoIngresoSalidaDto hisDto)
        {
            HistoricoIngresoSalidasManagers hism = new HistoricoIngresoSalidasManagers();
            MensajeDto mensaje = hism.CargarHistoricoIngresoSalida(hisDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/HistoricoIngresoSalidas/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/HistoricoIngresoSalidas/5
        public HttpResponseMessage Delete(int id)
        {
            HistoricoIngresoSalidasManagers hism = new HistoricoIngresoSalidasManagers();
            MensajeDto mensaje = hism.EliminarHistoricoSalario(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
