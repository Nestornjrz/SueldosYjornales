using SYJ.Application.Dto;
using SYJ.Application.Dto.Auxiliares;
using SYJ.Domain.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers.Auxiliares {
    public class InfoLiqSalariosManagers {
        /// <summary>
        /// Este es una clase que se utilizaba para informa la liquidacion de salario pero 
        /// va a quedar en desusho
        /// </summary>
        /// <param name="lsfDto"></param>
        /// <returns></returns>
        public MensajeDto ConsultarLiquidacionSalario(LiquidacionSueldoFormDto lsfDto) {
            using (var context = new SueldosJornalesEntities()) {
                var empleados = GetListadoEmpleados(context);
                var empleadosSegunSucursal = getEmpleadosSegunSucursal(lsfDto, empleados);
                string cargoMensaje = CargarCargo(empleadosSegunSucursal);
                //Se prepara el listado de la liquidacion de salarios Dto
                List<LiquidacionSalarioDto> listLsDto = new List<LiquidacionSalarioDto>();
                empleadosSegunSucursal.ForEach(delegate(EmpleadoDto e) {
                    LiquidacionSalarioDto lsDto = new LiquidacionSalarioDto();
                    //-----Recuperando el empleado-----
                    lsDto.Empleado = e;
                    lsDto.DiasTrabajados = 30;
                    string asingarSalarioMensaje = AsignarSalarioBase(e, lsDto);
                    //-----sub total ingresos------
                    lsDto.SubTotalIngresos = lsDto.SalarioBase;
                    //-----comisiones--------------
                    ComisionesManagers cm = new ComisionesManagers();
                    List<ComisioneDto> listadoComision = cm.ListadoSegunMesYanosYempleado(e.EmpleadoID, lsfDto.Mes.MesID, lsfDto.Year);
                    lsDto.Comisiones = listadoComision.Sum(s => s.MontoComision);
                    //----total ingresos-----------
                    lsDto.TotalIngreso = lsDto.SubTotalIngresos + lsDto.Comisiones;
                    //----descuento IPS------------
                    lsDto.DescIPS = (lsDto.SubTotalIngresos / 100) * 9;
                    //----descuento otros----------
                    lsDto.DescOtros = 0;
                    //----total descuento----------
                    lsDto.TotalDescuentos = lsDto.DescIPS + lsDto.DescOtros;
                    //----neto a cobrar------------
                    lsDto.NetoAcobrar = lsDto.TotalIngreso - lsDto.TotalDescuentos;
                    //----periodo------------------
                    var periodo = new DateTime(lsfDto.Year, lsfDto.Mes.MesID, 1);
                    var ultimoDiaPeriodo = new DateTime(lsfDto.Year, lsfDto.Mes.MesID, DateTime.DaysInMonth(lsfDto.Year, lsfDto.Mes.MesID));
                    lsDto.Periodo = periodo;
                    lsDto.UltimoDiaPeriodo = ultimoDiaPeriodo;
                    //-----mensaje de los calculos
                    lsDto.MensajeCalculos = asingarSalarioMensaje + cargoMensaje;
                    listLsDto.Add(lsDto);
                });
                throw new NotImplementedException();
                //return new MensajeDto() {
                //    Error = false,
                //    MensajeDelProceso = "Se cargo el listado de salarios segun la sucursal, el mes y el año seleccionado : ",
                //    ObjetoDto = listLsDto
                //};
            }
        }

        private static string CargarCargo(List<EmpleadoDto> empleadosSegunSucursal) {
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();
            MensajeDto mensajeCargo = null;
            string mensaje = "";
            empleadosSegunSucursal.ForEach(delegate(EmpleadoDto e) {
                mensajeCargo = hsm.SalarioYCargoActual(e.EmpleadoID);
                if (mensajeCargo.Error) {
                    mensaje += mensajeCargo.MensajeDelProceso;
                } else {
                    var hsDto = (HistoricoSalarioDto)mensajeCargo.ObjetoDto;
                    e.Cargo = hsDto.Cargo;
                }
            });
            return mensaje;
        }

        private static string AsignarSalarioBase(EmpleadoDto e, LiquidacionSalarioDto lsDto) {
            //-----salario base------------
            string mensajeString = "ok";
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();

            MensajeDto mensaje = hsm.SalarioYCargoActual(e.EmpleadoID);
            if (mensaje.Error) {
                lsDto.SalarioBase = 0;
                return mensaje.MensajeDelProceso;
            }
            lsDto.SalarioBase = decimal.Parse(mensaje.Valor);
            return mensajeString;
        }

        private static List<EmpleadoDto> getEmpleadosSegunSucursal(LiquidacionSueldoFormDto lsfDto, List<EmpleadoDto> empleados) {
            using (var context = new SueldosJornalesEntities()) {
                List<EmpleadoDto> empleadoSegunSucursal = new List<EmpleadoDto>();
                //Filtrando empleados por la ultima sucursal donde trabajo
                HistoricoSucursalesManagers sm = new HistoricoSucursalesManagers();
                empleados.ForEach(delegate(EmpleadoDto e) {
                    int sucursalID = int.Parse(sm.UltimoSucursales(e.EmpleadoID).Valor);
                    if (lsfDto.Sucursale.SucursalID == sucursalID) {
                        var sucursalDto = context.Sucursales
                            .Where(s => s.SucursalID == sucursalID)
                            .Select(s => new SucursaleDto() {
                                SucursalID = s.SucursalID,
                                NombreSucursal = s.NombreSucursal
                            }).First();
                        e.Sucursale = sucursalDto;

                        empleadoSegunSucursal.Add(e);
                    }
                });
                return empleadoSegunSucursal;
            }
        }

        private static List<EmpleadoDto> GetListadoEmpleados(SueldosJornalesEntities context) {
            var empleados = context.Empleados
                .Select(e => new EmpleadoDto() {
                    EmpleadoID = e.EmpleadoID,
                    Nombres = e.Nombres,
                    Apellidos = e.Apellidos,
                    NroCedula = e.NroCedula
                })
                .ToList();
            return empleados;
        }
    }
}
