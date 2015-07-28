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
    public class ComisionesController : ApiController
    {
        // GET: api/Comisiones
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/Comisiones/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadoID(long empleadoID) {
            ComisionesManagers cm = new ComisionesManagers();
            List<ComisioneDto> listado = cm.ListadoHistoricoHorarios(empleadoID);
            return Request.CreateResponse<List<ComisioneDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Comisiones/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Comisiones
        public HttpResponseMessage Post(ComisioneDto cDto)
        {
            ComisionesManagers cm = new ComisionesManagers();
            MensajeDto mensaje = cm.CargarComision(cDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Comisiones/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Comisiones/5
        public HttpResponseMessage Delete(int id)
        {
            ComisionesManagers cm = new ComisionesManagers();
            MensajeDto mensaje = cm.EliminarComision(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
