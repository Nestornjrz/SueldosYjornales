using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;

namespace Comisarios.Controllers.Api {
    [RoutePrefix("api/Empleados")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class EmpleadosController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(EmpleadoDto eDto) {
            EmpleadosManagers em = new EmpleadosManagers();
            MensajeDto mensaje = em.CargarEmpleado(eDto, Guid.NewGuid()); ;

            if (mensaje == null) {
                return NotFound();
            }
            return Ok(mensaje);
        }
    }
}
