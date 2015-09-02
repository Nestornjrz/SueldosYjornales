using SYJ.Application.Dto;
using SYJ.Domain.Managers.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api.Auxiliares
{
    public class ModificarPrestamosController : ApiController
    {
        // GET: api/ModificarPrestamos
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ModificarPrestamos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ModificarPrestamos
        public HttpResponseMessage Post(MovEmpleadoDetDto meDto)
        {
            ModificarPrestamosManagers mpm = new ModificarPrestamosManagers(meDto);
            MensajeDto mensaje = mpm.ModificarPrestamo();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/ModificarPrestamos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ModificarPrestamos/5
        public void Delete(int id)
        {
        }
    }
}
