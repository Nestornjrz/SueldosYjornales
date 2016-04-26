using SYJ.Application.Dto;
using SYJ.Application.Dto.Mtess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers.Mtess {
    public class EmpleadosYobrerosManagers {
        public List<EmpleadoYobreroDto> ListadoEmpleados() {
            EmpleadosManagers em = new EmpleadosManagers();
            HistoricoDireccionesManagers hdm = new HistoricoDireccionesManagers();
            var empleados = em.ListadoEmpleados();
            foreach (EmpleadoDto empleado in empleados) {
                //Se empieza a cargar el empleado
                EmpleadoYobreroDto eyoDto = new EmpleadoYobreroDto();
                eyoDto.NroPatronal = 77399;
                eyoDto.Documento = empleado.NroCedula.ToString();
                eyoDto.Nombre = empleado.Nombres.Split(' ')[0];
                eyoDto.Apellido = empleado.Apellidos.Split(' ')[0];
                eyoDto.Sexo = (empleado.Sexo.SexoID == 1) ? 'M' : 'F';
                //Se calcula la carga del estado civil
                switch (empleado.EstadoCivile.EstadoCivilID) {
                    case 1:
                        eyoDto.EstadoCivil = 'S';
                        break;
                    case 2:
                        eyoDto.EstadoCivil = 'C';
                        break;
                    case 3:
                        eyoDto.EstadoCivil = 'D';
                        break;
                    case 4:
                        eyoDto.EstadoCivil = 'V';
                        break;
                    default:
                        break;
                }
                eyoDto.FechaNac = empleado.FechaNacimiento;
                eyoDto.Nacionalidad = empleado.Nacionalidade.NombreNacionalidad;
                //Se calcula domicilio
                var direccionActual = hdm.DireccionaActual(empleado.EmpleadoID);
                if (!direccionActual.Error) {
                    var historicoDi = (HistoricoDireccioneDto)direccionActual.ObjetoDto;
                    eyoDto.Domicilio = historicoDi.Direccion;
                } else {
                    eyoDto.Domicilio = "--";
                }
                //eyoDto.FechaNacMenor
                eyoDto.HijosMenores = empleado.CantidadHijos;

                throw new NotImplementedException();
            }
        }
    }
}
