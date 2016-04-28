using SYJ.Application.Dto.Mtess;
using SYJ.Domain.Managers.Mtess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace SueldosYjornales.Areas.Mtess.Controllers.Api {
    public class ResumenesGeneralesController : ApiController {
        // GET: api/ResumenesGenerales
        public HttpResponseMessage Get() {
            ResumenesGeneralesManagers rgm = new ResumenesGeneralesManagers();
            var listado = rgm.ListadoResumenGeneral();
            return Request.CreateResponse<List<ResumenGeneralDto>>(HttpStatusCode.OK, listado);
        }
        [HttpGet]
        [Route("api/ResumenesGenerales/GetFile")]
        public HttpResponseMessage GetFile() {
            ResumenesGeneralesManagers rgm = new ResumenesGeneralesManagers();
            var listado = rgm.ListadoResumenGeneral();
            StringBuilder sb = new StringBuilder();
            foreach (var item in listado) {
                var linea = item.NroPatronal + "," +
                            item.Anho + "," +
                            item.SupJefesVarones + "," +
                            item.SupJefesMujeres + "," +
                            item.EmpleadosVarones + "," +
                            item.EmpleadosMujeres + "," +
                            item.ObrerosVarones + "," +
                            item.ObrerosMujeres + "," +
                            item.MenoresVarones + "," +
                            item.MenoresMujeres + "," +
                            item.Orden;
                sb.AppendLine(linea);
            }
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(sb.ToString());
            writer.Flush();
            stream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "ResumenesGenerales.cvs" };
            return result;
        }

        // GET: api/ResumenesGenerales/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/ResumenesGenerales
        public void Post([FromBody]string value) {
        }

        // PUT: api/ResumenesGenerales/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/ResumenesGenerales/5
        public void Delete(int id) {
        }
    }
}
