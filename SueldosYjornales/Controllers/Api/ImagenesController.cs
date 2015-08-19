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
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;

namespace SueldosYjornales.Controllers.Api {
    public class ImagenesController : ApiController {
        // GET: api/Imagenes
        [HttpGet]
        public HttpResponseMessage Get([FromUri] long empleadoID, [FromUri] int tipoImagenID) {
            ImagenesManagers im = new ImagenesManagers();
            MensajeDto mensaje = im.RecuperarImagen(empleadoID, tipoImagenID);
            if (mensaje.Error) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, mensaje.MensajeDelProceso);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            Image image = (Image)mensaje.ObjetoDto;
            MemoryStream memoryStream = new MemoryStream();
            if (mensaje.Valor == "jpg") {
                //Error arreglado en el jpg gracias al siguiente enlace
                //http://stackoverflow.com/questions/15571022/how-to-find-reason-for-generic-gdi-error-when-saving-an-image
                var imageJpg = new Bitmap(image);
                imageJpg.Save(memoryStream, ImageFormat.Jpeg);
                result.Content = new ByteArrayContent(memoryStream.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            } else if (mensaje.Valor == "png") {
                image.Save(memoryStream, ImageFormat.Png);
                result.Content = new ByteArrayContent(memoryStream.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            } else if (mensaje.Valor == "gif") {
                image.Save(memoryStream, ImageFormat.Gif);
                result.Content = new ByteArrayContent(memoryStream.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
            }
            return result;
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
