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
    public class SueldosYjornalesController : ApiController {
        // GET: api/SueldosYjornales
        public HttpResponseMessage Get() {
            SueldosYjornalesManagers sym = new SueldosYjornalesManagers();
            var listado = sym.ListadoSueldosYjoranales();
            return Request.CreateResponse<List<SueldoYjornaleDto>>(HttpStatusCode.OK, listado);
        }
        [HttpGet]
        [Route("api/SueldosYjornales/GetFile")]
        public HttpResponseMessage GetFile() {
            SueldosYjornalesManagers sym = new SueldosYjornalesManagers();
            StringBuilder sb = new StringBuilder();
            var listado = sym.ListadoSueldosYjoranales();
            //Se inserta el titulo
            var linea_titulo = "NROPATRONAL,DOCUMENTO,FORMADPAGO,IMPORTEUNITARIO,H_ENE,S_ENE," +
                                "H_FEB,S_FEB," +
                                "H_MAR,S_MAR,"+
                                "H_ABR,S_ABR,"+
                                "H_MAY,S_MAY"+
                                "H_JUN,S_JUN"+
                                "H_JUL,S_JUL"+
                                "H_AGO,S_AGO"+
                                "H_SET,S_SET"+
                                "H_OCT,S_OCT"+
                                "H_NOV,S_NOV"+
                                "H_DIC,S_DIC"+
                                "H_50,S_50"+
                                "H_100,S_100"+
                                "AGUINALDO"+
                                "BENEFICIOS"+
                                "BONIFICACIONES"+
                                "VACACIONES"+
                                "TOTAL_H,TOTAL_S"+
                                "TOTAGENERAL";
            sb.AppendLine(linea_titulo);
            foreach (var item in listado) {
                var linea = item.NroPatronal + "," +
                            item.Documento + "," +
                            item.FormaDePago + "," +
                            item.ImporteUnitario + "," +
                            item.H_Ene + "," +
                            item.S_Ene + "," +
                            item.H_Feb + "," +
                            item.S_Feb + "," +
                            item.H_Mar + "," +
                            item.S_Mar + "," +
                            item.H_Abr + "," +
                            item.S_Abr + "," +
                            item.H_May + "," +
                            item.S_May + "," +
                            item.H_Jun + "," +
                            item.S_Jun + "," +
                            item.H_Jul + "," +
                            item.S_Jul + "," +
                            item.H_Ago + "," +
                            item.S_Ago + "," +
                            item.H_Set + "," +
                            item.S_Set + "," +
                            item.H_Oct + "," +
                            item.S_Oct + "," +
                            item.H_Nov + "," +
                            item.S_Nov + "," +
                            item.H_Dic + "," +
                            item.S_Dic + "," +
                            item.H_50 + "," +
                            item.S_50 + "," +
                            item.H_100 + "," +
                            item.S_100 + "," +
                            item.Aguinaldo + "," +
                            item.Beneficios + "," +
                            item.Bonificaciones + "," +
                            item.Vacaciones + "," +
                            item.Total_H + "," +
                            item.Total_S + "," +
                            item.TotalGeneral;
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
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "SueldosYjornales.cvs" };
            return result;
        }

        // GET: api/SueldosYjornales/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/SueldosYjornales
        public void Post([FromBody]string value) {
        }

        // PUT: api/SueldosYjornales/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/SueldosYjornales/5
        public void Delete(int id) {
        }
    }
}
