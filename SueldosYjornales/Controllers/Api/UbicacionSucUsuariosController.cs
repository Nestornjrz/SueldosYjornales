using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace SueldosYjornales.Controllers.Api {
    [Authorize]
    public class UbicacionSucUsuariosController : ApiController {
        // GET: api/UbicacionSucUsuarios
        public HttpResponseMessage GetUbicacionSucUsuario() {
            UbicacionSucUsuariosManagers usum = new UbicacionSucUsuariosManagers();
            MensajeDto mensaje = usum.RecuperarUbicacionSucUsuario(Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // GET: api/UbicacionSucUsuarios/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/UbicacionSucUsuarios
        public HttpResponseMessage Post(UbicacionSucUsuarioDto usuDto) {
            UbicacionSucUsuariosManagers usum = new UbicacionSucUsuariosManagers();
            MensajeDto mensaje = usum.CargarUbicacionSucUsuario(usuDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/UbicacionSucUsuarios/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/UbicacionSucUsuarios/5
        public void Delete(int id) {
        }
    }
}
