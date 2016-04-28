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
    public class EmpleadosYobrerosController : ApiController {
        // GET: api/EmpleadosYobreros
        public HttpResponseMessage Get() {
            EmpleadosYobrerosManagers eyom = new EmpleadosYobrerosManagers();
            var listado = eyom.ListadoEmpleados();
            return Request.CreateResponse<List<EmpleadoYobreroDto>>(HttpStatusCode.OK, listado);
        }
        [HttpGet]
        [Route("api/EmpleadosYobreros/GetFile")]
        public HttpResponseMessage GetFile() {
            EmpleadosYobrerosManagers eyom = new EmpleadosYobrerosManagers();
            var listado = eyom.ListadoEmpleados();

            StringBuilder sb = new StringBuilder();
            foreach (var item in listado) {
                var fechaEntrada = "";
                if (item.FechaEntrada != null) {
                    fechaEntrada = item.FechaEntrada.Value.ToString("yyyy/MM/dd");
                }
                var fechaSalida = "";
                if (item.FechaSalida != null) {
                    fechaSalida = item.FechaSalida.Value.ToString("yyyy/MM/dd");
                }
                var linea = item.NroPatronal + "," +
                            item.Documento + "," +
                            item.Nombre + "," +
                            item.Apellido + "," +
                            item.Sexo + "," +
                            item.EstadoCivil + "," +
                            item.FechaNac.ToString("yyyy/MM/dd") + "," +
                            item.Nacionalidad + "," +
                            item.Domicilio + "," +
                            item.FechaNacMenor + "," +
                            item.HijosMenores + "," +
                            item.Cargo + "," +
                            item.Profesion + "," +
                            fechaEntrada + "," +
                            item.HorarioTrabajo + "," +
                            item.MenorEscapa + "," +
                            item.MenorEsEscolar + "," +
                            fechaSalida + "," +
                            item.MotivoSalida + "," +
                            item.Estado;
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
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "EmpleadosYobreros.cvs" };
            return result;          
        }

        // GET: api/EmpleadosYobreros/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/EmpleadosYobreros
        public void Post([FromBody]string value) {
        }

        // PUT: api/EmpleadosYobreros/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/EmpleadosYobreros/5
        public void Delete(int id) {
        }
    }
}
