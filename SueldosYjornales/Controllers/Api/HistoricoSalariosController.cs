using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api
{
    public class HistoricoSalariosController : ApiController
    {
        // GET: api/HistoricoSalarios
        public HttpResponseMessage Get()
        {
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();
            List<HistoricoSalarioDto> listado = hsm.ListadoHistoricoSalarios();
            return Request.CreateResponse<List<HistoricoSalarioDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/HistoricoSalarios/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadosID(long empleadoID) {
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();
            List<HistoricoSalarioDto> listado = hsm.ListadoHistoricoSalarios(empleadoID);
            return Request.CreateResponse<List<HistoricoSalarioDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/HistoricoSalarios/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/HistoricoSalarios
        public HttpResponseMessage Post(HistoricoSalarioDto hsDto)
        {
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();
            MensajeDto mensaje = hsm.CargarHistoricoSalario(hsDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/HistoricoSalarios/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/HistoricoSalarios/5
        public HttpResponseMessage Delete(int id)
        {
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();
            MensajeDto mensaje = hsm.EliminarHistoricoSalario(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
