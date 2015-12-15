using SYJ.Application.Dto.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SYJ.Domain.Managers;
using SYJ.Domain.Managers.Auxiliares;
using SYJ.Application.Dto;

namespace SueldosYjornales.Controllers.Api.Auxiliares
{
    public class LiquidacionAguinaldosController : ApiController
    {
        // GET: api/LiquidacionAguinaldos
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LiquidacionAguinaldos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LiquidacionAguinaldos
        public HttpResponseMessage Post(FormLiquidacionDto fldto) {
            LiquidacionAguinaldosManagers lsm = new LiquidacionAguinaldosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.GenerarLiquidacionesAguinaldos();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
        [HttpPost]
        [Route("api/LiquidacionAguinaldos/Detalles")]
        public HttpResponseMessage PostDetalles(FormLiquidacionDto fldto) {
            LiquidacionAguinaldosManagers lsm = new LiquidacionAguinaldosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.RecuperarDetallesPorMes();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
        [HttpPost]
        [Route("api/LiquidacionAguinaldos/ParaImprimir")]
        public HttpResponseMessage PostParaImprimir(FormLiquidacionDto fldto) {
            LiquidacionAguinaldosManagers lsm = new LiquidacionAguinaldosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.RecuperarDetallesParaImprimir();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }


        // PUT: api/LiquidacionAguinaldos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LiquidacionAguinaldos/5
        public void Delete(int id)
        {
        }
    }
}
