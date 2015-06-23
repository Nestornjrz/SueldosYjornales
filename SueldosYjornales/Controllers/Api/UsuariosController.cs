using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api {
    public class UsuariosController : ApiController {
        // GET: api/Usuarios
        public HttpResponseMessage Get() {
            UsuariosManagers um = new UsuariosManagers();
            List<UsuarioDto> listado = um.ListadoUsuarios();
            return Request.CreateResponse<List<UsuarioDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Usuarios/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/Usuarios
        public HttpResponseMessage Post(UsuarioDto uDto) {
            UsuariosManagers um = new UsuariosManagers();
            MensajeDto mensaje = um.CargarUsuarios(uDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Usuarios/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/Usuarios/5
        public HttpResponseMessage Delete(int id) {
            UsuariosManagers um = new UsuariosManagers();
            MensajeDto mensaje = um.EliminarUsuarios(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
