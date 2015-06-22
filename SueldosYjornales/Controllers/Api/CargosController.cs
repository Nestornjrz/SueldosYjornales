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
    public class CargosController : ApiController
    {
        // GET: api/Cargos
        public HttpResponseMessage Get()
        {
            CargosManagers cm = new CargosManagers();
            List<CargoDto> listado = cm.ListadoCargos();
            return Request.CreateResponse<List<CargoDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Cargos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Cargos
        public HttpResponseMessage Post(CargoDto cDto)
        {
            CargosManagers cm = new CargosManagers();
            MensajeDto mensaje = cm.CargarCargos(cDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Cargos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Cargos/5
        public HttpResponseMessage Delete(int id)
        {
            CargosManagers cm = new CargosManagers();
            MensajeDto mensaje = cm.EliminarCargos(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
