using SYJ.Application.Dto;
using SYJ.Domain.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers.Auxiliares {
    public class LiquidacionSalariosManagers {
        public MensajeDto GenerarLiquidacionSalario(int[] empleadosSeleccionados, Guid userID) {
            MensajeDto mensaje = null;
            foreach (var empleadoID in empleadosSeleccionados) {
                mensaje = GenerarLiquidacionEmpleado(empleadoID, userID);

            }
            throw new NotImplementedException();
        }

        private MensajeDto GenerarLiquidacionEmpleado(int empleadoID, Guid userID) {
            //Creditos
            decimal sueldoBase = RecuperarSueldo(empleadoID, userID);
            decimal[] comisiones = RecuperarComisiones(empleadoID, userID);
            //Devitos
            decimal[] anticipos = RecuperarAnticipos(empleadoID, userID);

            throw new NotImplementedException();
        }

        private decimal[] RecuperarAnticipos(int empleadoID, Guid userID) {
            throw new NotImplementedException();
        }

        private decimal[] RecuperarComisiones(int empleadoID, Guid userID) {
            throw new NotImplementedException();
        }

        private decimal RecuperarSueldo(int empleadoID, Guid userID) {
            throw new NotImplementedException();
        }
    }
}
