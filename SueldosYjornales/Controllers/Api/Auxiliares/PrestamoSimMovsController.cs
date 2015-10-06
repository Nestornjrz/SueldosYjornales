using SYJ.Application.Dto;
using SYJ.Application.Dto.Auxiliares;
using SYJ.Domain.Managers.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api.Auxiliares
{
    public class PrestamoSimMovsController : ApiController
    {
        // GET: api/PrestamoSimMovs
        public HttpResponseMessage Get(long movEmpleadoID)
        {
            PrestamoSimMovManagers psmm = new PrestamoSimMovManagers();
            PrestamoSimMovDto psmDto = psmm.GetPrestamoSimpleMov(movEmpleadoID);
            return Request.CreateResponse(HttpStatusCode.OK, psmDto);
        }

        // GET: api/PrestamoSimMovs/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PrestamoSimMovs
        public void Post([FromBody]string value)
        {
        }

        [HttpPost]
        [Route("api/PrestamoSimMovs/ParaPlanillaPrestamos")]
        public HttpResponseMessage PostParaImprimir(MesYearEmpresaSucursalesDto myesDto) {
            PrestamoSimMovManagers psmm = new PrestamoSimMovManagers();
            MensajeDto mensaje = psmm.RecuperarListPrestamoAgruXsucResumen(myesDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
          
        }

        // PUT: api/PrestamoSimMovs/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PrestamoSimMovs/5
        public void Delete(int id)
        {
        }
    }
}
