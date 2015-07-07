﻿using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers {
    public class HistoricoDireccionesManagers {
        public List<HistoricoDireccioneDto> ListadoHistoricoDirecciones() {
            throw new NotImplementedException();
        }

        public MensajeDto CargarHistoricoDirecciones(HistoricoDireccioneDto hdDto, Guid userID) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                var historicoDireccioneDb = new HistoricoDireccione();
                historicoDireccioneDb.EmpleadoID = hdDto.EmpleadoID;
                historicoDireccioneDb.Direccion = hdDto.Direccion;
                historicoDireccioneDb.MomentoCarga = DateTime.Now;
                //Se recupera el usuarioID
                var usuarioID = context.Usuarios.Where(u => u.UserID == userID)
                    .First().UsuarioID;
                historicoDireccioneDb.UsuarioID = usuarioID;

                context.HistoricoDirecciones.Add(historicoDireccioneDb);

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) { return mensajeDto; }

                hdDto.HistoricoDireccionID = historicoDireccioneDb.HistoricoDireccionID;

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo la direccion : " + hdDto.HistoricoDireccionID,
                    ObjetoDto = hdDto
                };
            }
        }

        public MensajeDto EliminarHistoricoDireccion(int id) {
            throw new NotImplementedException();
        }

        public List<HistoricoDireccioneDto> ListadoHistoricoDirecciones(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var listado = context.HistoricoDirecciones
                    .Where(h => h.EmpleadoID == empleadoID)
                    .Select(s => new HistoricoDireccioneDto() {
                        HistoricoDireccionID = s.HistoricoDireccionID,
                        Direccion = s.Direccion
                    }).ToList();
                return listado;
            }
        }
    }
}