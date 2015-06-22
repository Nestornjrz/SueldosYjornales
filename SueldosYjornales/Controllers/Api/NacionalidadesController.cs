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
    public class NacionalidadesController : ApiController
    {
        // GET: api/Nacionalidades
        public HttpResponseMessage Get()
        {
            NacionalidadesManagers nm = new NacionalidadesManagers();
            List<NacionalidadeDto> listado = nm.ListadoNacionalidades();
            return Request.CreateResponse<List<NacionalidadeDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Nacionalidades/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Nacionalidades
        public HttpResponseMessage Post(NacionalidadeDto nDto)
        {
            NacionalidadesManagers nm = new NacionalidadesManagers();
            MensajeDto mensaje = nm.CargarNacionalidad(nDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Nacionalidades/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Nacionalidades/5
        public HttpResponseMessage Delete(int id)
        {
            NacionalidadesManagers nm = new NacionalidadesManagers();
            MensajeDto mensaje = nm.EliminarNacionalidad(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
