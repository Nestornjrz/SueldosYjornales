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
    public class AnticiposController : ApiController
    {
        // GET: api/Anticipos
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/Anticipos/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadoID(long empleadoID) {
            AnticiposManagers am = new AnticiposManagers();
            List<AnticipoDto> listado = am.ListadoAnticipos(empleadoID);
            return Request.CreateResponse<List<AnticipoDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Anticipos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Anticipos
        public HttpResponseMessage Post(AnticipoDto aDto)
        {
            AnticiposManagers am = new AnticiposManagers();
            MensajeDto mensaje = am.CargarAnticipo(aDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Anticipos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Anticipos/5
        public HttpResponseMessage Delete(int id)
        {
            AnticiposManagers am = new AnticiposManagers();
            MensajeDto mensaje = am.EliminarAnticipo(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
