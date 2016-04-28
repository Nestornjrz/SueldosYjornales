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
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();
            HistoricoIngresoSalidasManagers hism = new HistoricoIngresoSalidasManagers();

            int year = 2015;

            var empleados = em.ListadoEmpleados();
            List<EmpleadoYobreroDto> listado = new List<EmpleadoYobreroDto>();

            foreach (EmpleadoDto empleado in empleados) {
                //Se empieza a cargar el empleado
                EmpleadoYobreroDto eyoDto = new EmpleadoYobreroDto();
                eyoDto.EmpleadoID = empleado.EmpleadoID;
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
                    if (historicoDi.Direccion.Length > 100) {
                        eyoDto.Domicilio = historicoDi.Direccion.Substring(0, 100);
                    } else {
                        eyoDto.Domicilio = historicoDi.Direccion;
                    }
                } else {
                    eyoDto.Domicilio = "--";
                }
                //eyoDto.FechaNacMenor
                eyoDto.HijosMenores = empleado.CantidadHijos;
                //Se carga el cargo
                var salarioYcargo = hsm.SalarioYCargoActual(empleado.EmpleadoID);
                if (!salarioYcargo.Error) {
                    var historicoSalarioCargo = (HistoricoSalarioDto)salarioYcargo.ObjetoDto;
                    eyoDto.Cargo = historicoSalarioCargo.Cargo.NombreCargo;
                }
                eyoDto.Profesion = empleado.Profesione.NombreProfesion;
                //Se ve que ultima fecha de entrada tiene
                var ingresoEgreso = hism.UltimoIngreso(empleado.EmpleadoID);
                if (!ingresoEgreso.Error) {
                    var historicoIngresoEgreso = (HistoricoIngresoSalidaDto)ingresoEgreso.ObjetoDto;
                    eyoDto.FechaEntrada = historicoIngresoEgreso.FechaIngreso;
                    //Evitar que los que tienen fecha de ingreso superior al periodo que se esta calculando
                    //ingresen en la lista.
                    if (!(eyoDto.FechaEntrada.Value.Year <= year)) {
                        continue;
                    }
                    if (historicoIngresoEgreso.FechaSalida != null) {
                        //Evitar que se muestre la fecha de salida y el motivo si supera el periodo
                        //que se esta calculando.
                        if (historicoIngresoEgreso.FechaSalida.Value.Year <= year) {
                            eyoDto.FechaSalida = historicoIngresoEgreso.FechaSalida.Value;
                            eyoDto.MotivoSalida = historicoIngresoEgreso.MotivoSalida;
                        }
                    }
                }
                //eyoDto.HorarioTrabajo
                //eyoDto.MenorEscapa
                //eyoDto.MenorEsEscolar
                listado.Add(eyoDto);
            }
            return listado;
        }
    }
}
