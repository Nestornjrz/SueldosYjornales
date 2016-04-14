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
    public class InfoLiqSalariosController : ApiController
    {
        // GET: api/InfoLiqSalarios
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/InfoLiqSalarios/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/InfoLiqSalarios
        public HttpResponseMessage Post(LiquidacionSueldoFormDto lsfDto)
        {
            InfoLiqSalariosManagers lsm = new InfoLiqSalariosManagers();
            MensajeDto mensaje = lsm.ConsultarLiquidacionSalario(lsfDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/InfoLiqSalarios/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/InfoLiqSalarios/5
        public void Delete(int id)
        {
        }
    }
}
