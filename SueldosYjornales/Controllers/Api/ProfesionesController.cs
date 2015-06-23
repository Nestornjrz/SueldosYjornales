using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api {
    public class ProfesionesController : ApiController {
        // GET: api/Profesiones
        public HttpResponseMessage Get() {
            ProfesionesManagers pm = new ProfesionesManagers();
            List<ProfesioneDto> listado = pm.ListadoProfesiones();
            return Request.CreateResponse<List<ProfesioneDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Profesiones/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/Profesiones
        public HttpResponseMessage Post(ProfesioneDto pDto) {
            ProfesionesManagers pm = new ProfesionesManagers();
            MensajeDto mensaje = pm.CargarProfesion(pDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Profesiones/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/Profesiones/5
        public HttpResponseMessage Delete(int id) {
            ProfesionesManagers pm = new ProfesionesManagers();
            MensajeDto mensaje = pm.EliminarProfesion(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
