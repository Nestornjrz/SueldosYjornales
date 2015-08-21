using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class PrestamosSimplesManagers {
        public List<PrestamoSimpleDto> ListadoPrestamo(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.PrestamosSimples
                    .Where(p => p.EmpleadoID == empleadoID)
                    .Select(s => new PrestamoSimpleDto() {
                        PrestamoSimpleID = s.PrestamoSimpleID,
                        EmpleadoID = s.EmpleadoID,
                        FechaPrestamo = s.FechaPrestamo,
                        Monto = s.Monto,
                        Cuotas = s.Cuotas,
                        Observacion = s.Observacion
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarPrestamo(PrestamoSimpleDto psDto, Guid userID) {
            if (psDto.PrestamoSimpleID > 0) {
                return EditarPrestamo(psDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var prestamoSimpleDb = new PrestamosSimple();
                prestamoSimpleDb.EmpleadoID = psDto.EmpleadoID;
                prestamoSimpleDb.FechaPrestamo = psDto.FechaPrestamo;
                prestamoSimpleDb.Monto = psDto.Monto;
                prestamoSimpleDb.Cuotas = psDto.Cuotas;
                prestamoSimpleDb.Observacion = psDto.Observacion;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                prestamoSimpleDb.UsuarioID = usuarioID;
                prestamoSimpleDb.MomentoCarga = DateTime.Now;

                context.PrestamosSimples.Add(prestamoSimpleDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                psDto.PrestamoSimpleID = prestamoSimpleDb.PrestamoSimpleID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el prestamo: " + psDto.PrestamoSimpleID,
                    ObjetoDto = psDto
                };
            }
        }

        private MensajeDto EditarPrestamo(PrestamoSimpleDto psDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var prestamoSimpleDb = context.PrestamosSimples
                 .Where(p => p.PrestamoSimpleID == psDto.PrestamoSimpleID)
                 .FirstOrDefault();
                if (prestamoSimpleDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el prestamo id : " + psDto.PrestamoSimpleID
                    };
                }
                prestamoSimpleDb.Monto = psDto.Monto;
                prestamoSimpleDb.Cuotas = psDto.Cuotas;
                prestamoSimpleDb.Observacion = psDto.Observacion;

                context.Entry(prestamoSimpleDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito el prestamo: " + psDto.PrestamoSimpleID,
                    ObjetoDto = psDto
                };
            }
        }

        public MensajeDto EliminarPrestamo(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var prestamoSimpleDb = context.PrestamosSimples
                    .Where(p => p.PrestamoSimpleID == id)
                    .FirstOrDefault();
                if (prestamoSimpleDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "No existe el prestamo id : " + id
                    };
                }
                context.PrestamosSimples.Remove(prestamoSimpleDb);
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el prestamo : " + id
                };
            }
        }
    }
}
