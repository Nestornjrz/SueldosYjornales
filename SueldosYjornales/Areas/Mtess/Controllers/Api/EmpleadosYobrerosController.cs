using SYJ.Application.Dto.Mtess;
using SYJ.Domain.Managers.Mtess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Areas.Mtess.Controllers.Api
{
    public class EmpleadosYobrerosController : ApiController
    {
        // GET: api/EmpleadosYobreros
        public HttpResponseMessage Get()
        {
            EmpleadosYobrerosManagers eyom = new EmpleadosYobrerosManagers();
            var listado = eyom.ListadoEmpleados();
            return Request.CreateResponse<List<EmpleadoYobreroDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/EmpleadosYobreros/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/EmpleadosYobreros
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/EmpleadosYobreros/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EmpleadosYobreros/5
        public void Delete(int id)
        {
        }
    }
}
