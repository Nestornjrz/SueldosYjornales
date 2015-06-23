using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class ProfesionesManagers {
        public List<ProfesioneDto> ListadoProfesiones() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Profesiones
                    .Select(s => new ProfesioneDto() {
                        ProfesionID = s.ProfesionID,
                        NombreProfesion = s.NombreProfesion,
                        Descripcion = s.Descripcion,
                        Abreviatura = s.Abreviatura
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarProfesion(ProfesioneDto pDto) {
            if (pDto.ProfesionID > 0) {
                return EditarProfesion(pDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var profesioneDb = new Profesione();
                profesioneDb.NombreProfesion = pDto.NombreProfesion;
                profesioneDb.Abreviatura = pDto.Abreviatura;
                profesioneDb.Descripcion = pDto.Descripcion;

                context.Profesiones.Add(profesioneDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                pDto.ProfesionID = profesioneDb.ProfesionID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo la profesion : " + pDto.ProfesionID,
                    ObjetoDto = pDto
                };
            }
        }

        private MensajeDto EditarProfesion(ProfesioneDto pDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var profesioneDb = context.Profesiones
                    .Where(p => p.ProfesionID == pDto.ProfesionID)
                    .First();
                profesioneDb.NombreProfesion = pDto.NombreProfesion;
                profesioneDb.Abreviatura = pDto.Abreviatura;
                profesioneDb.Descripcion = pDto.Descripcion;

                context.Entry(profesioneDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito la profesion : " + pDto.ProfesionID,
                    ObjetoDto = pDto
                };

            }
        }

        public MensajeDto EliminarProfesion(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var profesioneDb = context.Profesiones
                    .Where(p => p.ProfesionID == id)
                    .FirstOrDefault();

                if (profesioneDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "La profesion ID : " + id + " no existe en la base de datos"                       
                    };
                }
                context.Profesiones.Remove(profesioneDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino la profesion : " + id
                };
            }
        }
    }
}
