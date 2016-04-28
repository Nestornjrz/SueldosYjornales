using SYJ.Application.Dto.Mtess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYJ.Application.Dto;

namespace SYJ.Domain.Managers.Mtess {
    public class ResumenesGeneralesManagers {
        public List<ResumenGeneralDto> ListadoResumenGeneral() {
            EmpleadosManagers em = new EmpleadosManagers();
            SueldosYjornalesManagers syjm = new SueldosYjornalesManagers();
            List<SueldoYjornaleDto> listadoSueldoYjornales = syjm.ListadoSueldosYjoranales();

            var year = 2015;
            var empleados = em.ListadoEmpleados();
            var empleadosActivos = new List<EmpleadoDto>();
            var empleadosSalidos = new List<EmpleadoDto>();
            foreach (var empleado in empleados) {
                if (HistoricoIngresoSalidasManagers.EmpleadoTrabajaTodaviaEnLaEmpresa(empleado.EmpleadoID, 12, year)) {
                    empleadosActivos.Add(empleado);
                } else {
                    empleadosSalidos.Add(empleado);
                }
            }

            var listado = new List<ResumenGeneralDto>();
            //Orden 1 - 1 CANTIDAD  O NUMERO DE TRABAJADORES
            ResumenGeneralDto rgDto_1 = new ResumenGeneralDto();
            rgDto_1.NroPatronal = 77399;
            rgDto_1.Anho = year;
            rgDto_1.SupJefesVarones = CantidadSupJefes(empleadosActivos, 1);//Masculino
            rgDto_1.SupJefesMujeres = CantidadSupJefes(empleadosActivos, 2);//Femenino
            rgDto_1.EmpleadosVarones = CantidadEmpleados(empleadosActivos, 1);//Masculino
            rgDto_1.EmpleadosMujeres = CantidadEmpleados(empleadosActivos, 2);//Femenino

            rgDto_1.Orden = 1;
            listado.Add(rgDto_1);

            //Orden 2 -  HORAS TRABAJADAS
            ResumenGeneralDto rgDto_2 = new ResumenGeneralDto();
            rgDto_2.NroPatronal = 77399;
            rgDto_2.Anho = year;
            rgDto_2.SupJefesVarones = CantHorasTrabajadasSupJefes(empleadosActivos, listadoSueldoYjornales, 1);//Masculino
            rgDto_2.SupJefesMujeres = CantHorasTrabajadasSupJefes(empleadosActivos, listadoSueldoYjornales, 2);//Femenino
            rgDto_2.EmpleadosVarones = CantHorasTrabajadasEmpleados(empleadosActivos, listadoSueldoYjornales, 1);//Masculino
            rgDto_2.EmpleadosMujeres = CantHorasTrabajadasEmpleados(empleadosActivos, listadoSueldoYjornales, 2);//Femenino

            rgDto_2.Orden = 2;
            listado.Add(rgDto_2);
            //Orden 3 -  SUELDOS O JORNALES
            ResumenGeneralDto rgDto_3 = new ResumenGeneralDto();
            rgDto_3.NroPatronal = 77399;
            rgDto_3.Anho = year;
            rgDto_3.SupJefesVarones = CantSueldosYjornalesSupJefes(empleadosActivos, listadoSueldoYjornales, 1);//Masculino
            rgDto_3.SupJefesMujeres = CantSueldosYjornalesSupJefes(empleadosActivos, listadoSueldoYjornales, 2);//Femenino
            rgDto_3.EmpleadosVarones = CantSueldosYJornalesEmpleados(empleadosActivos, listadoSueldoYjornales, 1);//Masculino
            rgDto_3.EmpleadosMujeres = CantSueldosYJornalesEmpleados(empleadosActivos, listadoSueldoYjornales, 2);//Femenino

            rgDto_3.Orden = 3;
            listado.Add(rgDto_3);
            //Orden 4 -  CANTIDAD DE INGRESOS
            ResumenGeneralDto rgDto_4 = new ResumenGeneralDto();
            rgDto_4.NroPatronal = 77399;
            rgDto_4.Anho = year;
            rgDto_4.SupJefesVarones = CantidadIngreEgreSupJefesYear(empleadosActivos, year, 1);//Masculino
            rgDto_4.SupJefesMujeres = CantidadIngreEgreSupJefesYear(empleadosActivos, year, 2);//Femenino
            rgDto_4.EmpleadosVarones = CantIngreEgreEmpleados(empleadosActivos, year, 1);//Masculino
            rgDto_4.EmpleadosMujeres = CantIngreEgreEmpleados(empleadosActivos, year, 2);//Femenino

            rgDto_4.Orden = 4;
            listado.Add(rgDto_4);
            //Orden 5 - CANTIDAD DE EGRESOS
            ResumenGeneralDto rgDto_5 = new ResumenGeneralDto();
            rgDto_5.NroPatronal = 77399;
            rgDto_5.Anho = year;
            rgDto_5.SupJefesVarones = CantidadIngreEgreSupJefesYear(empleadosSalidos, year, 1);//Masculino
            rgDto_5.SupJefesMujeres = CantidadIngreEgreSupJefesYear(empleadosSalidos, year, 2);//Femenino
            rgDto_5.EmpleadosVarones = CantIngreEgreEmpleados(empleadosSalidos, year, 1);//Masculino
            rgDto_5.EmpleadosMujeres = CantIngreEgreEmpleados(empleadosSalidos, year, 2);//Femenino

            rgDto_5.Orden = 5;
            listado.Add(rgDto_5);

            return listado;
        }

      

        private int CantIngreEgreEmpleados(List<EmpleadoDto> empleados, int year, int sexoID) {
            HistoricoIngresoSalidasManagers hism = new HistoricoIngresoSalidasManagers();
            int cantidad = 0;
            foreach (var empleado in empleados) {
                if (empleado.Sexo.SexoID == sexoID && empleado.Cargo.CargoID != 2) {//No sea Gerente Administrativo
                    var mensajeUltimoIngreso = hism.UltimoIngreso(empleado.EmpleadoID);
                    if (!mensajeUltimoIngreso.Error) {
                        var hisIngresoSalidaDto = (HistoricoIngresoSalidaDto)mensajeUltimoIngreso.ObjetoDto;
                        var fechaIngreso = hisIngresoSalidaDto.FechaIngreso;
                        if (fechaIngreso.Year == year) {
                            cantidad++;
                        }
                    }
                }
            }
            return cantidad;
        }

        private int CantidadIngreEgreSupJefesYear(List<EmpleadoDto> empleados, int year, int sexoID) {
            HistoricoIngresoSalidasManagers hism = new HistoricoIngresoSalidasManagers();
            int cantidad = 0;
            foreach (var empleado in empleados) {
                if (empleado.Sexo.SexoID == sexoID && empleado.Cargo.CargoID == 2) {//Gerente Administrativo
                    var mensajeUltimoIngreso = hism.UltimoIngreso(empleado.EmpleadoID);
                    if (!mensajeUltimoIngreso.Error) {
                        var hisIngresoSalidaDto = (HistoricoIngresoSalidaDto)mensajeUltimoIngreso.ObjetoDto;
                        var fechaIngreso = hisIngresoSalidaDto.FechaIngreso;
                        if (fechaIngreso.Year == year) {
                            cantidad++;
                        }
                    }
                }
            }
            return cantidad;
        }

        private int CantSueldosYJornalesEmpleados(List<EmpleadoDto> empleadosActivos,
            List<SueldoYjornaleDto> listadoSueldoYjornales,
            int sexoID) {
            int totalSueldos = 0;
            foreach (var empleado in empleadosActivos) {
                if (empleado.Cargo.CargoID != 2 && //No es Gerente Administrativo
                    empleado.Sexo.SexoID == sexoID) {
                    var empleSueldoYjornal = listadoSueldoYjornales.Where(l => l.EmpleadoID == empleado.EmpleadoID)
                         .FirstOrDefault();
                    if (empleSueldoYjornal != null) {
                        totalSueldos += (int)empleSueldoYjornal.Total_S;
                    }
                }
            }
            return totalSueldos;
        }

        private int CantSueldosYjornalesSupJefes(List<EmpleadoDto> empleadosActivos,
            List<SueldoYjornaleDto> listadoSueldoYjornales,
            int sexoID) {
            int totalSueldos = 0;
            foreach (var empleado in empleadosActivos) {
                if (empleado.Cargo.CargoID == 2 && //Gerente general
                    empleado.Sexo.SexoID == sexoID) {
                    var empleSueldoYjornal = listadoSueldoYjornales.Where(l => l.EmpleadoID == empleado.EmpleadoID)
                         .FirstOrDefault();
                    if (empleSueldoYjornal != null) {
                        totalSueldos += (int)empleSueldoYjornal.Total_S;
                    }
                }
            }
            return totalSueldos;
        }

        private int CantHorasTrabajadasEmpleados(List<EmpleadoDto> empleadosActivos,
            List<SueldoYjornaleDto> listadoSueldoYjornales,
            int sexoID) {
            int cantidad = 0;
            foreach (var empleado in empleadosActivos) {
                if (empleado.Cargo.CargoID != 2 && //No sea Gerente general
                    empleado.Sexo.SexoID == sexoID) {
                    var empleSueldoYjornal = listadoSueldoYjornales.Where(l => l.EmpleadoID == empleado.EmpleadoID)
                         .FirstOrDefault();
                    if (empleSueldoYjornal != null) {
                        cantidad += empleSueldoYjornal.Total_H;
                    }
                }
            }
            return cantidad;
        }

        private int CantHorasTrabajadasSupJefes(List<EmpleadoDto> empleadosActivos,
            List<SueldoYjornaleDto> listadoSueldoYjornales, int sexoID) {
            int cantidad = 0;
            foreach (var empleado in empleadosActivos) {
                if (empleado.Cargo.CargoID == 2 && //Gerente general
                    empleado.Sexo.SexoID == sexoID) {
                    var empleSueldoYjornal = listadoSueldoYjornales.Where(l => l.EmpleadoID == empleado.EmpleadoID)
                         .FirstOrDefault();
                    if (empleSueldoYjornal != null) {
                        cantidad += empleSueldoYjornal.Total_H;
                    }
                }
            }
            return cantidad;
        }

        private int CantidadEmpleados(List<EmpleadoDto> empleadosActivos, int sexoID) {
            int cantidad = 0;
            foreach (var empleado in empleadosActivos) {
                if (empleado.Cargo.CargoID != 2 && //No sea Gerente general
                    empleado.Sexo.SexoID == sexoID) {
                    cantidad++;
                }
            }
            return cantidad;
        }

        private int CantidadSupJefes(List<EmpleadoDto> empleadosActivos, int sexoID) {
            int cantidad = 0;
            foreach (var empleado in empleadosActivos) {
                if (empleado.Cargo.CargoID == 2 && //Gerente general
                    empleado.Sexo.SexoID == sexoID) {
                    cantidad++;
                }
            }
            return cantidad;
        }
    }
}
