using SYJ.Application.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers.Util {
    public class AgregarModificar {
        public static MensajeDto Hacer(DbContext context, MensajeDto mensajeDto) {
            try {
                context.SaveChanges();
            } catch (DbUpdateException e) {
                var mensaje = MensajeAux(e);
                mensajeDto = new MensajeDto() {
                    Error = true,
                    MensajeDelProceso = mensaje
                };
            } catch (DbEntityValidationException e) {
                var mensajeEntity = e.EntityValidationErrors.First()
                    .ValidationErrors.First().ErrorMessage;
                var mensaje = MensajeAux(e);
                mensajeDto = new MensajeDto() {
                    Error = true,
                    MensajeDelProceso = mensaje + " " + mensajeEntity
                };
            } catch (Exception e) {
                var mensaje = MensajeAux(e);
                mensajeDto = new MensajeDto() {
                    Error = true,
                    MensajeDelProceso = mensaje
                };
            }
            return mensajeDto;
        }
        private static string MensajeAux(Exception e) {
            var mensaje = e.Message;
            if (e.InnerException != null) {
                if (mensaje != e.InnerException.Message) {
                    mensaje += ", " + e.InnerException.Message;
                }
                if (e.InnerException.InnerException != null) {
                    mensaje += ", " + e.InnerException.InnerException.Message;
                }
            }
            return mensaje;
        }
    }
}
