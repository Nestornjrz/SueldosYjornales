using Microsoft.AspNet.Identity;
using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Admin.Api
{
    [Authorize]
    public class VacacionesController : ApiController
    {
        // GET: api/Vacaciones
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Vacaciones/5
        [HttpGet]
        [Route("api/Vacaciones/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadoID(int empleadoID)
        {
            VacacionesManagers vm = new VacacionesManagers();
            List<VacacioneDto> listado = vm.ListadoVacaciones(empleadoID);
            return Request.CreateResponse<List<VacacioneDto>>(HttpStatusCode.OK, listado);
        }

        // POST: api/Vacaciones
        public HttpResponseMessage Post(VacacioneDto vDto)
        {
            VacacionesManagers vm = new VacacionesManagers();
            MensajeDto mensaje = vm.CargarVacacion(vDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Vacaciones/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Vacaciones/5
        public HttpResponseMessage Delete(int id)
        {
            VacacionesManagers vm = new VacacionesManagers();
            MensajeDto mensaje = vm.EliminarVacacion(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
