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
    public class EstadosCivilesController : ApiController
    {
        // GET: api/EstadosCiviles
        public HttpResponseMessage Get()
        {
            EstadosCivilesManagers ecm = new EstadosCivilesManagers();
            List<EstadoCivileDto> listado = ecm.ListadoEstadosCiviles();
            return Request.CreateResponse<List<EstadoCivileDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/EstadosCiviles/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/EstadosCiviles
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/EstadosCiviles/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EstadosCiviles/5
        public void Delete(int id)
        {
        }
    }
}
