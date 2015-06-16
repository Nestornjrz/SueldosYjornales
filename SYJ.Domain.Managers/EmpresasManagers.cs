using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class EmpresasManagers {

        public List<EmpresaDto> ListadoEmpresas() {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.Empresas
                    .Select(s => new EmpresaDto() {
                        EmpresaID = s.EmpresaID,
                        Descripcion = s.Descripcion,
                        Abreviatura = s.Abreviatura,
                        NombreEmpresa = s.NombreEmpresa,
                        Ruc = s.Ruc
                    }).ToList();
                return listado;
            }
        }

        public MensajeDto CargarEmpresa(EmpresaDto eDto) {
            if (eDto.EmpresaID > 0) {
                return EditarEmpresa(eDto);
            }
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var empresaDb = new Empresa();
                empresaDb.Descripcion = eDto.Descripcion;
                empresaDb.Abreviatura = eDto.Abreviatura;
                empresaDb.NombreEmpresa = eDto.NombreEmpresa;
                empresaDb.Ruc = eDto.Ruc;

                context.Empresas.Add(empresaDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                eDto.EmpresaID = empresaDb.EmpresaID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo la empresa : " + eDto.EmpresaID,
                    ObjetoDto = eDto
                };
            }
        }

        private MensajeDto EditarEmpresa(EmpresaDto eDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var empresaDb = context.Empresas
                    .Where(e => e.EmpresaID == eDto.EmpresaID)
                    .First();
                empresaDb.Descripcion = eDto.Descripcion;
                empresaDb.Abreviatura = eDto.Abreviatura;
                empresaDb.NombreEmpresa = eDto.NombreEmpresa;
                empresaDb.Ruc = eDto.Ruc;

                context.Entry(empresaDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se Edito la empresa : " + eDto.EmpresaID,
                    ObjetoDto = eDto
                };
            }
        }

        public MensajeDto EliminarEmpresa(int id) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var empresaDb = context.Empresas
                   .Where(e => e.EmpresaID == id)
                   .First();
                context.Empresas.Remove(empresaDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se elimino la empresa : " + id
                };
            }
        }
    }
}
