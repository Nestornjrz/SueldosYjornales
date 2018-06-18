using SYJ.Application.Dto;
using SYJ.Application.Dto.Mtess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SYJ.Domain.Managers.Mtess {
    public class ResumenesGeneralesManagers {
        public List<ResumenGeneralDto> ListadoResumenGeneral() {
            EmpleadosManagers em = new EmpleadosManagers();
            SueldosYjornalesManagers syjm = new SueldosYjornalesManagers();
            List<SueldoYjornaleDto> listadoSueldoYjornales = syjm.ListadoSueldosYjoranales();
            EmpleadosYobrerosManagers eyom = new EmpleadosYobrerosManagers();
            var listadoEmplMtess = eyom.ListadoEmpleadosFormatoMtess();

            var year = 2017;
            List<EmpleadoDto> empleados = em.ListadoEmpleados();

            var empleadosActivos = new List<EmpleadoDto>();
            var empleadosSalidos = new List<EmpleadoDto>();
            // var empleadosSalidosTotales = new List<EmpleadoDto>();
            //foreach (var syjDto in listadoSueldoYjornales)
            //{
            //    var empleado = empleados.Where(e => e.EmpleadoID == syjDto.EmpleadoID).First();
            //    if (HistoricoIngresoSalidasManagers.EmpleadoTrabajaTodaviaEnLaEmpresa(syjDto.EmpleadoID, 12, year))
            //    {
            //        empleadosActivos.Add(empleado);
            //    }
            //    else
            //    {
            //        empleadosSalidos.Add(empleado);
            //    }
            //    //Se calcula la cantidad de empleados salidos en el año, los que incluyen los que se les considera como activos
            //    //porque salieron en el mes activo (2 parametro del metodo EmpleadoTrabajaTodaviaEnLaEmpresa)
            //    if (!HistoricoIngresoSalidasManagers.EmpleadoTrabajaTodaviaEnLaEmpresa(syjDto.EmpleadoID, 12, year, false))
            //    {
            //        empleadosSalidosTotales.Add(empleado);
            //    }
            //}
            foreach (var empMtess in listadoEmplMtess) {
                var empleado = empleados.Where(e => e.EmpleadoID == empMtess.EmpleadoID).First();
                if (empMtess.FechaSalida == null) {
                    empleadosActivos.Add(empleado);
                } else {
                    if (empMtess.FechaSalida.Value.Month == 12) {// Si salio en diciembre se considera activo
                        empleadosActivos.Add(empleado);
                    } else {
                        empleadosSalidos.Add(empleado);
                    }
                }
            }
            List<EmpleadoDto> empleadosActivosYsalidos = new List<EmpleadoDto>();
            empleadosActivosYsalidos.AddRange(empleadosActivos);
            empleadosActivosYsalidos.AddRange(empleadosSalidos);


            var listado = new List<ResumenGeneralDto>();
            //Orden 1 - 1 CANTIDAD  O NUMERO DE TRABAJADORES
            ResumenGeneralDto rgDto_1 = new ResumenGeneralDto();
            rgDto_1.NroPatronal = 77399;
            rgDto_1.Anho = year;
            rgDto_1.SupJefesVarones = CantidadSupJefes(empleadosActivosYsalidos, 1);//Masculino
            rgDto_1.SupJefesMujeres = CantidadSupJefes(empleadosActivosYsalidos, 2);//Femenino
            rgDto_1.EmpleadosVarones = CantidadEmpleados(empleadosActivosYsalidos, 1);//Masculino
            rgDto_1.EmpleadosMujeres = CantidadEmpleados(empleadosActivosYsalidos, 2);//Femenino

            rgDto_1.Orden = 1;
            listado.Add(rgDto_1);

            //Orden 2 -  HORAS TRABAJADAS
            ResumenGeneralDto rgDto_2 = new ResumenGeneralDto();
            rgDto_2.NroPatronal = 77399;
            rgDto_2.Anho = year;
            rgDto_2.SupJefesVarones = CantHorasTrabajadasSupJefes(empleadosActivosYsalidos, listadoSueldoYjornales, 1);//Masculino
            rgDto_2.SupJefesMujeres = CantHorasTrabajadasSupJefes(empleadosActivosYsalidos, listadoSueldoYjornales, 2);//Femenino
            rgDto_2.EmpleadosVarones = CantHorasTrabajadasEmpleados(empleadosActivosYsalidos, listadoSueldoYjornales, 1);//Masculino
            rgDto_2.EmpleadosMujeres = CantHorasTrabajadasEmpleados(empleadosActivosYsalidos, listadoSueldoYjornales, 2);//Femenino

            rgDto_2.Orden = 2;
            listado.Add(rgDto_2);
            //Orden 3 -  SUELDOS O JORNALES
            ResumenGeneralDto rgDto_3 = new ResumenGeneralDto();
            rgDto_3.NroPatronal = 77399;
            rgDto_3.Anho = year;
            rgDto_3.SupJefesVarones = CantSueldosYjornalesSupJefes(empleadosActivosYsalidos, listadoSueldoYjornales, 1);//Masculino
            rgDto_3.SupJefesMujeres = CantSueldosYjornalesSupJefes(empleadosActivosYsalidos, listadoSueldoYjornales, 2);//Femenino
            rgDto_3.EmpleadosVarones = CantSueldosYJornalesEmpleados(empleadosActivosYsalidos, listadoSueldoYjornales, 1);//Masculino
            rgDto_3.EmpleadosMujeres = CantSueldosYJornalesEmpleados(empleadosActivosYsalidos, listadoSueldoYjornales, 2);//Femenino

            rgDto_3.Orden = 3;
            listado.Add(rgDto_3);
            //Orden 4 -  CANTIDAD DE INGRESOS
            ResumenGeneralDto rgDto_4 = new ResumenGeneralDto();
            rgDto_4.NroPatronal = 77399;
            rgDto_4.Anho = year;
            rgDto_4.SupJefesVarones = CantidadIngresoSupJefesYear(empleadosActivosYsalidos, listadoEmplMtess, year, 1);//Masculino
            rgDto_4.SupJefesMujeres = CantidadIngresoSupJefesYear(empleadosActivosYsalidos, listadoEmplMtess, year, 2);//Femenino
            rgDto_4.EmpleadosVarones = CantIngresoEmpleados(empleadosActivosYsalidos, listadoEmplMtess, year, 1);//Masculino
            rgDto_4.EmpleadosMujeres = CantIngresoEmpleados(empleadosActivosYsalidos, listadoEmplMtess, year, 2);//Femenino

            rgDto_4.Orden = 4;
            listado.Add(rgDto_4);
            //Orden 5 - CANTIDAD DE EGRESOS
            ResumenGeneralDto rgDto_5 = new ResumenGeneralDto();
            rgDto_5.NroPatronal = 77399;
            rgDto_5.Anho = year;
            rgDto_5.SupJefesVarones = CantidadEgresoSupJefesYear(empleadosActivosYsalidos, listadoEmplMtess, year, 1);//Masculino          
            rgDto_5.SupJefesMujeres = CantidadEgresoSupJefesYear(empleadosActivosYsalidos, listadoEmplMtess, year, 2);//Femenino          
            rgDto_5.EmpleadosVarones = CantEgresoEmpleados(empleadosActivosYsalidos, listadoEmplMtess, year, 1);//Masculino          
            rgDto_5.EmpleadosMujeres = CantEgresoEmpleados(empleadosActivosYsalidos, listadoEmplMtess, year, 2);//Femenino          

            rgDto_5.Orden = 5;
            listado.Add(rgDto_5);

            return listado;
        }

        private int CantEgresoEmpleados(List<EmpleadoDto> empleadosSalidos, List<EmpleadoYobreroDto> listadoEmplMtess, int year, int sexoID) {
            listadoEmplMtess = listadoEmplMtess.Where(w=>w.FechaSalida != null).ToList();


            int cantidad = 0;
            foreach (var empleado in empleadosSalidos) {
                if (empleado.Sexo.SexoID == sexoID && empleado.Cargo.CargoID != 2) {//No sea Gerente Administrativo
                    cantidad += listadoEmplMtess.Where(w => w.FechaSalida.Value.Year == year && w.EmpleadoID == empleado.EmpleadoID).Count();
                    //var mensajeUltimoIngreso = hism.UltimoIngreso(empleado.EmpleadoID,new DateTime(year, 12, 31));
                    //if (!mensajeUltimoIngreso.Error) {
                    //    var hisIngresoSalidaDto = (HistoricoIngresoSalidaDto)mensajeUltimoIngreso.ObjetoDto;
                    //    var fechaSalida = hisIngresoSalidaDto.FechaSalida;
                    //    if (fechaSalida != null) {
                    //        if (fechaSalida.Value.Year == year) {
                    //            cantidad++;
                    //        }
                    //    }
                    //}
                }
            }
            return cantidad;
        }



        private int CantIngresoEmpleados(List<EmpleadoDto> empleados, List<EmpleadoYobreroDto> listadoEmplMtess, int year, int sexoID) {
            HistoricoIngresoSalidasManagers hism = new HistoricoIngresoSalidasManagers();
            int cantidad = 0;
            foreach (var empleado in empleados) {
                if (empleado.Sexo.SexoID == sexoID && empleado.Cargo.CargoID != 2) {//No sea Gerente Administrativo
                    cantidad += listadoEmplMtess.Where(w => w.FechaEntrada.Value.Year == year && w.EmpleadoID == empleado.EmpleadoID).Count();
                    //var mensajeUltimoIngreso = hism.UltimoIngreso(empleado.EmpleadoID,new DateTime(year,12,31));
                    //if (!mensajeUltimoIngreso.Error) {
                    //    var hisIngresoSalidaDto = (HistoricoIngresoSalidaDto)mensajeUltimoIngreso.ObjetoDto;
                    //    var fechaIngreso = hisIngresoSalidaDto.FechaIngreso;
                    //    if (fechaIngreso.Year == year) {
                    //        cantidad++;
                    //    }
                    //}
                }
            }
            return cantidad;
        }

        private int CantidadIngresoSupJefesYear(List<EmpleadoDto> empleados, List<EmpleadoYobreroDto> listadoEmplMtess, int year, int sexoID) {
            //HistoricoIngresoSalidasManagers hism = new HistoricoIngresoSalidasManagers();
            int cantidad = 0;
            foreach (var empleado in empleados) {
                if (empleado.Sexo.SexoID == sexoID && empleado.Cargo.CargoID == 2) {//Gerente Administrativo
                    cantidad += listadoEmplMtess.Where(w => w.FechaEntrada.Value.Year == year && w.EmpleadoID == empleado.EmpleadoID).Count();
                    //var mensajeUltimoIngreso = hism.UltimoIngreso(empleado.EmpleadoID, new DateTime(year, 12, 31));
                    //if (!mensajeUltimoIngreso.Error) {
                    //    var hisIngresoSalidaDto = (HistoricoIngresoSalidaDto)mensajeUltimoIngreso.ObjetoDto;
                    //    var fechaIngreso = hisIngresoSalidaDto.FechaIngreso;
                    //    if (fechaIngreso.Year == year) {
                    //        cantidad++;
                    //    }
                    //}
                }
            }
            return cantidad;
        }
        private int CantidadEgresoSupJefesYear(List<EmpleadoDto> empleados, List<EmpleadoYobreroDto> listadoEmplMtess, int year, int sexoID) {
            listadoEmplMtess = listadoEmplMtess.Where(w => w.FechaSalida != null).ToList();
            int cantidad = 0;
            foreach (var empleado in empleados) {
                if (empleado.Sexo.SexoID == sexoID && empleado.Cargo.CargoID == 2) {//Gerente Administrativo
                    cantidad += listadoEmplMtess.Where(w => w.FechaSalida.Value.Year == year && w.EmpleadoID == empleado.EmpleadoID).Count();                   
                    //var mensajeUltimoIngreso = hism.UltimoIngreso(empleado.EmpleadoID, new DateTime(year, 12, 31));
                    //if (!mensajeUltimoIngreso.Error) {
                    //    var hisIngresoSalidaDto = (HistoricoIngresoSalidaDto)mensajeUltimoIngreso.ObjetoDto;
                    //    var fechaIngreso = hisIngresoSalidaDto.FechaIngreso;
                    //    if (fechaIngreso.Year == year) {
                    //        cantidad++;
                    //    }
                    //}
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
                        totalSueldos += (int)empleSueldoYjornal.Total_S + (int)empleSueldoYjornal.Aguinaldo; 
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
                        totalSueldos += (int)empleSueldoYjornal.Total_S + (int)empleSueldoYjornal.Aguinaldo;
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
