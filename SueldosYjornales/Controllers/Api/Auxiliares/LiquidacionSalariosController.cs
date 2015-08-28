using SYJ.Application.Dto;
using SYJ.Domain.Managers.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SYJ.Application.Dto.Auxiliares;

namespace SueldosYjornales.Controllers.Api.Auxiliares
{
    public class LiquidacionSalariosController : ApiController
    {
        // GET: api/LiquidacionSalarios
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LiquidacionSalarios/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LiquidacionSalarios
        public HttpResponseMessage Post(FormLiquidacionDto fldto)
        {
            LiquidacionSalariosManagers lsm = new LiquidacionSalariosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.GenerarLiquidacionesSalarios();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // POST: api/LiquidacionSalarios/Detalles
        [HttpPost]
        [Route("api/LiquidacionSalarios/Detalles")]
        public HttpResponseMessage PostDetalles(FormLiquidacionDto fldto) {
            LiquidacionSalariosManagers lsm = new LiquidacionSalariosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.RecuperarDetalles();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // POST: api/LiquidacionSalarios/ParaImprimir
        [HttpPost]
        [Route("api/LiquidacionSalarios/ParaImprimir")]
        public HttpResponseMessage PostParaImprimir(FormLiquidacionDto fldto) {
            LiquidacionSalariosManagers lsm = new LiquidacionSalariosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.RecuperarDetallesParaImprimir();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/LiquidacionSalarios/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LiquidacionSalarios/5
        public void Delete(int id)
        {
        }
    }
}
