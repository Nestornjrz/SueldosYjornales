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
    public class EmpresasController : ApiController
    {
        // GET: api/Empresas
        public HttpResponseMessage Get()
        {
            EmpresasManagers em = new EmpresasManagers();
            List<EmpresaDto> listado = em.ListadoEmpresas();
            return Request.CreateResponse<List<EmpresaDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Empresas/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Empresas
        public HttpResponseMessage Post(EmpresaDto eDto)
        {
            EmpresasManagers em = new EmpresasManagers();
            MensajeDto mensaje = em.CargarEmpresa(eDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Empresas/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Empresas/5
        public HttpResponseMessage Delete(int id)
        {
            EmpresasManagers em = new EmpresasManagers();
            MensajeDto mensaje = em.EliminarEmpresa(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
