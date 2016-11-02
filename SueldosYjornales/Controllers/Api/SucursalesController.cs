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
    [Authorize]
    public class SucursalesController : ApiController
    {
        // GET: api/Sucursales
        public HttpResponseMessage Get()
        {
            SucursalesManagers sm = new SucursalesManagers();
            List<SucursaleDto> listado = sm.ListadoSucursales();
            return Request.CreateResponse<List<SucursaleDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/Sucursales/SegunEmpresaID")]
        public HttpResponseMessage GetByEmpresa(int empresaID) {
            SucursalesManagers sm = new SucursalesManagers();
            List<SucursaleDto> listado = sm.ListadoSucursales(empresaID);
            return Request.CreateResponse<List<SucursaleDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Sucursales/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Sucursales
        public HttpResponseMessage Post(SucursaleDto sDto)
        {
            SucursalesManagers sm = new SucursalesManagers();
            MensajeDto mensaje = sm.CargarSucursal(sDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Sucursales/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sucursales/5
        public HttpResponseMessage Delete(int id)
        {
            SucursalesManagers sm = new SucursalesManagers();
            MensajeDto mensaje = sm.EliminarSucursal(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
