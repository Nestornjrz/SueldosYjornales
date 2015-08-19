using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api
{
    public class TipoImagenesController : ApiController
    {
        // GET: api/TipoImagenes
        public HttpResponseMessage Get()
        {
            TipoImagenesManagers tim = new TipoImagenesManagers();
            List<TipoImageneDto> listado = tim.ListadoTipoImagenes();
            return Request.CreateResponse<List<TipoImageneDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/TipoImagenes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TipoImagenes
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TipoImagenes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TipoImagenes/5
        public void Delete(int id)
        {
        }
    }
}
