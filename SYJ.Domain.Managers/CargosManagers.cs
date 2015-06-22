using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class CargosManagers {
        public List<CargoDto> ListadoCargos() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Cargos
                    .Select(s => new CargoDto() {
                        CargoID = s.CargoID,
                        NombreCargo = s.NombreCargo,
                        Abreviatura = s.Abreviatura
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarCargos(CargoDto cDto) {
            if (cDto.CargoID > 0) {
                return EditarCargar(cDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var cargoDb = new Cargo();
                cargoDb.CargoID = cDto.CargoID;
                cargoDb.NombreCargo = cDto.NombreCargo;
                cargoDb.Abreviatura = cDto.Abreviatura;

                context.Cargos.Add(cargoDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                cDto.CargoID = cargoDb.CargoID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el cargo : " + cDto.CargoID,
                    ObjetoDto = cDto
                };
            }
        }

        private MensajeDto EditarCargar(CargoDto cDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var cargoDb = context.Cargos
                    .Where(c => c.CargoID == cDto.CargoID)
                    .FirstOrDefault();

                context.Entry(cargoDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito la empresa : " + cDto.CargoID,
                    ObjetoDto = cDto
                };
            }
        }

        public MensajeDto EliminarCargos(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var cargoDb = context.Cargos
                    .Where(c => c.CargoID == id)
                    .FirstOrDefault();

                if (cargoDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "La carga ID : " + id + " no existe en la base de datos"
                    };
                }
                context.Cargos.Remove(cargoDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino el cargo : " + id
                };
            }
        }
    }
}
