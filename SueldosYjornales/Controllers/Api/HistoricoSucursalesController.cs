using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace SueldosYjornales.Controllers.Api
{
    public class HistoricoSucursalesController : ApiController
    {
        // GET: api/HistoricoSucursales
        public HttpResponseMessage Get()
        {
            HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();
            List<HistoricoSucursaleDto> listado = hsm.ListadoHistoricoSucursales();
            return Request.CreateResponse<List<HistoricoSucursaleDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/HistoricoSucursales/ByEmpleadoID")]
        public HttpResponseMessage GetByEmpleadoID(long empleadoID) {
            HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();
            List<HistoricoSucursaleDto> listado = hsm.ListadoHistoricoSucursales(empleadoID);
            return Request.CreateResponse<List<HistoricoSucursaleDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/HistoricoSucursales/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/HistoricoSucursales
        public HttpResponseMessage Post(HistoricoSucursaleDto hsDto)
        {
            HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();
            MensajeDto mensaje = hsm.CargarHistoricoSucursal(hsDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/HistoricoSucursales/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/HistoricoSucursales/5
        public HttpResponseMessage Delete(int id)
        {
            HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();
            MensajeDto mensaje = hsm.EliminarHistoricoSucursal(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
