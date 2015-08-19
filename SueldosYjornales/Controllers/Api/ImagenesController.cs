using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace SueldosYjornales.Controllers.Api {
    public class ImagenesController : ApiController {
        // GET: api/Imagenes
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Imagenes/5
        public string Get(int id) {
            return "value";
        }

        // POST: api/Imagenes
        public HttpResponseMessage Post() {
            ImagenesManagers im = new ImagenesManagers();
            MensajeDto mensaje = null;
            var request = HttpContext.Current.Request;
            //Se recupera las variables enviadas desde el formulario 
            var empleadoID = request["empleadoID"];
            var tipoImagenID = request["tipoImagenID"];
            if (request.Files.Count > 0) {
                foreach (string file in request.Files) {
                    var postedFile = request.Files[file];
                    using (var binaryReader = new BinaryReader(postedFile.InputStream)) {
                        byte[] fileData = binaryReader.ReadBytes(postedFile.ContentLength);

                        mensaje = im.guardarImagen(long.Parse(empleadoID), int.Parse(tipoImagenID), fileData, postedFile.FileName, Guid.Parse(User.Identity.GetUserId()));
                    }
                }
                return Request.CreateResponse(HttpStatusCode.Created, mensaje);
            } else {
                mensaje = new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "No se envio ningun archivo"
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, mensaje);
            }

            //throw new NotImplementedException();
        }

        // PUT: api/Imagenes/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE: api/Imagenes/5
        public void Delete(int id) {
        }
    }
}
