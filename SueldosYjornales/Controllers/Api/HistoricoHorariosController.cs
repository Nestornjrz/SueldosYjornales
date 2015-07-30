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
    public class HistoricoHorariosController : ApiController
    {
        // GET: api/HistoricoHorarios
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/HistoricoHorarios/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadoID(long empleadoID) {
            HistoricoHorariosManagers hhm = new HistoricoHorariosManagers();
            List<HistoricoHorarioDto> listado = hhm.ListadoHistoricoHorarios(empleadoID);
            return Request.CreateResponse<List<HistoricoHorarioDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/HistoricoHorarios/UltimoHorario")]
        public HttpResponseMessage GetUltimoHorario(long empleadoID) {
            HistoricoHorariosManagers hhm = new HistoricoHorariosManagers();
            MensajeDto mensaje = hhm.UltimoHorario(empleadoID);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // GET: api/HistoricoHorarios/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/HistoricoHorarios
        public HttpResponseMessage Post(HistoricoHorarioDto hhDto)
        {
            HistoricoHorariosManagers hhm = new HistoricoHorariosManagers();
            MensajeDto mensaje = hhm.CargarHistoricoHorario(hhDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/HistoricoHorarios/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/HistoricoHorarios/5
        public HttpResponseMessage Delete(int id)
        {
            HistoricoHorariosManagers hhm = new HistoricoHorariosManagers();
            MensajeDto mensaje = hhm.EliminarHistoricoHorario(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
