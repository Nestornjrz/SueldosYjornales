using SYJ.Application.Dto;
using SYJ.Application.Dto.Auxiliares;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers.Auxiliares {
    public class PrestamoSimMovManagers {
        public PrestamoSimMovDto GetPrestamoSimpleMov(long movEmpleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                PrestamoSimMovDto psmDto = new PrestamoSimMovDto();
                //Se carga el prestamo simple
                var prestamoSimpleDto = context.PrestamosSimples
                    .Where(p => p.MovEmpleadoID == movEmpleadoID)
                    .Select(s => new PrestamoSimpleDto() {
                        PrestamoSimpleID = s.PrestamoSimpleID,
                        Cuotas = s.Cuotas,
                        EmpleadoID = s.EmpleadoID,
                        Empleado = new EmpleadoDto() {
                            EmpleadoID = s.EmpleadoID,
                            Nombres = s.Empleado.Nombres,
                            Apellidos = s.Empleado.Apellidos
                        },
                        Fecha1erVencimiento = s.Fecha1erVencimiento,
                        Monto = s.Monto,
                        Observacion = s.Observacion
                    }).FirstOrDefault();
                if (prestamoSimpleDto == null) {
                    return psmDto;
                }
                psmDto.PrestamoSimple = prestamoSimpleDto;
                //Se carga los movimientos relacionados con el prestamo
                MovEmpleadoDto movimientoDto = new MovEmpleadoDto();
                movimientoDto = context.MovEmpleados
                    .Where(m => m.MovEmpleadoID == movEmpleadoID)
                    .Select(s => new MovEmpleadoDto() {
                        MovEmpleadoID = s.MovEmpleadoID,
                        FechaMovimiento = s.FechaMovimiento,
                        Descripcion = s.Descripcion
                    }).FirstOrDefault();
                ///Se carga el detalle
                if (movimientoDto != null) {
                    List<MovEmpleadoDetDto> movimientosDet = new List<MovEmpleadoDetDto>();
                    movimientosDet = context.MovEmpleadosDets
                       .Where(m => m.MovEmpleadoID == movEmpleadoID)
                       .Select(s => new MovEmpleadoDetDto() {
                           MovEmpleadoID = s.MovEmpleadoID,
                           MovEmpleadoDetID = s.MovEmpleadoDetID,
                           Credito = (s.DevCred == false) ? s.Monto : 0,
                           Devito = (s.DevCred == true) ? s.Monto : 0,
                           MesAplicacion = s.MesAplicacion,
                           Empleado = new EmpleadoDto() {
                               EmpleadoID = s.EmpleadoID,
                               Nombres = s.Empleado.Nombres,
                               Apellidos = s.Empleado.Apellidos
                           }
                       }).ToList();
                    movimientoDto.MovEmpleadosDets = movimientosDet;
                }

                psmDto.MovimientoEmpleado = movimientoDto;

                return psmDto;
            }
        }

        public MensajeDto RecuperarListadoPrestamosAgrupadoXsuc(MesYearEmpresaSucursalesDto myesDto) {
            using (var context = new SueldosJornalesEntities()) {
                List<PrestamoSimMovDto> listPrestamoSimMovDto = new List<PrestamoSimMovDto>();
                //Se busca el empleado segun las sucursales              
                var empleados = context.Empleados.Select(s => s.EmpleadoID).ToArray();
                foreach (long empleadoID in empleados) {
                    HistoricoSucursaleDto sucursalIDdelEmple = HistoricoSucursalesManagers
                        .getSucursalDelEmpleado(empleadoID, myesDto.Year, myesDto.Mes.MesID);
                    if (sucursalIDdelEmple == null) {
                        sucursalIDdelEmple = new HistoricoSucursaleDto() {
                            EmpleadoID = empleadoID,
                            Sucursal = new SucursaleDto() {
                                SucursalID = 0,
                                NombreSucursal = "Sin sucursal asignada hasta este fecha",
                                Descripcion = "Sin sucursal",
                                Abreviatura = "SS"
                            }
                        };
                        if (myesDto.Sucursales.Where(s => s.SucursalID == 0).Count() < 1) {
                            myesDto.Sucursales.Add(sucursalIDdelEmple.Sucursal);
                        }
                    }
                    if (myesDto.Sucursales.Where(s => s.SucursalID == sucursalIDdelEmple.Sucursal.SucursalID).Count() > 0) {
                        //Se recuperan los prestamos del empleado
                        PrestamosSimplesManagers psm = new PrestamosSimplesManagers();
                        List<PrestamoSimpleDto> prestamosDelEmpleado = psm
                            .ListadoPrestamo(empleadoID, myesDto.Year, myesDto.Mes.MesID);
                        foreach (PrestamoSimpleDto psDto in prestamosDelEmpleado) {
                            //Tengo que marcar los que no estan marcados
                            MarcarPrestamo(psDto);
                            //Se ve si el prestamo tiene relacionado movimientos, es decir si tiene cuotas generadas
                            psDto.Empleado.Sucursale = sucursalIDdelEmple.Sucursal;
                            if (psDto.MovEmpleadoID != null) {
                                var psDtoYmov = GetPrestamoSimpleMov((long)psDto.MovEmpleadoID);
                                psDtoYmov.PrestamoSimple.Empleado.Sucursale = sucursalIDdelEmple.Sucursal;
                                listPrestamoSimMovDto.Add(psDtoYmov);
                            } else {//Si no se genero movimiento para este prestamo
                                listPrestamoSimMovDto.Add(new PrestamoSimMovDto() {
                                    PrestamoSimple = psDto
                                });
                            }
                        }
                    }
                }
                List<PrestamosSimXsucDto> listadoXsucursal = AgruparXSucursal(listPrestamoSimMovDto, myesDto.Sucursales);
                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se recuperaron los prestamos ",
                    ObjetoDto = listadoXsucursal
                };
            }
        }
        public MensajeDto RecuperarListPrestamoAgruXsucResumen(MesYearEmpresaSucursalesDto myesDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensaje = RecuperarListadoPrestamosAgrupadoXsuc(myesDto);
                List<PrestamosSimXsucDto> listadoXsucursal = (List<PrestamosSimXsucDto>)mensaje.ObjetoDto;
                foreach (PrestamosSimXsucDto g_suc in listadoXsucursal) {
                    foreach (PrestamoSimMovDto ps_Mov in g_suc.PrestamoSimpleMovimiento) {
                        var prestamoS = ps_Mov.PrestamoSimple;
                        var movimiento = ps_Mov.MovimientoEmpleado;

                        List<MovEmpleadoDetDto> movDetallesResul = new List<MovEmpleadoDetDto>();
                        MovEmpleadoDetDto movDetalle = new MovEmpleadoDetDto();
                        movDetalle.Observacion = "Saldo";

                        int contador = 0;
                        foreach (MovEmpleadoDetDto mDet in movimiento.MovEmpleadosDets) {
                            contador++;
                            //Se ve si el detalle del movimiento tiene una liquidacion generada
                            int generoLaLiquidacionCant = 0;
                            if (mDet.MesAplicacion.Year <= myesDto.Year && mDet.MesAplicacion.Month <= myesDto.Mes.MesID) {
                                generoLaLiquidacionCant = context.MovEmpleadosDets
                                   .Where(m => m.EmpleadoID == mDet.Empleado.EmpleadoID &&
                                             m.MesAplicacion.Year == myesDto.Year &&
                                             m.MesAplicacion.Month == myesDto.Mes.MesID &&
                                             m.LiquidacionConceptoID == (int)LiquidacionSalariosManagers
                                                                        .LiquidacionConceptos.TotalPagado)
                                   .Count();

                            }
                            if (generoLaLiquidacionCant > 0) {
                                movDetallesResul.Add(mDet);
                            } else {
                                movDetalle.Devito += mDet.Devito;
                            }
                        }
                        if (movDetalle.Devito > 0) {
                            movDetallesResul.Add(movDetalle);
                        }
                        movimiento.MovEmpleadosDets = movDetallesResul;
                    }
                }
                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se realizo el resumen, incorporando el saldo",
                    ObjetoDto = listadoXsucursal
                };
            }
        }

        private void MarcarPrestamo(PrestamoSimpleDto psDto) {
            using (var context = new SueldosJornalesEntities()) {
                MensajeDto mensajeDto = null;
                if (psDto.MovEmpleadoID == null) {//Los que no estan marcados tienen este campo null
                    var prestamoSimple = context.PrestamosSimples
                   .Where(p => p.PrestamoSimpleID == psDto.PrestamoSimpleID)
                   .First();

                    prestamoSimple.GenerarDevitoSn = true;
                    context.Entry(prestamoSimple).State = System.Data.Entity.EntityState.Modified;
                    mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                    if (mensajeDto.Error != true) {
                        psDto.MovEmpleadoID = prestamoSimple.MovEmpleadoID;
                    }
                }
            }
        }

        private List<PrestamosSimXsucDto> AgruparXSucursal(
            List<PrestamoSimMovDto> listPrestamoSimMovDto,
            List<SucursaleDto> sucursales) {
            List<PrestamosSimXsucDto> listadoResul = new List<PrestamosSimXsucDto>();
            foreach (var suc in sucursales) {
                PrestamosSimXsucDto psxsDto = new PrestamosSimXsucDto();
                var lisPresSimMov = listPrestamoSimMovDto
                    .Where(l => l.PrestamoSimple.Empleado.Sucursale.SucursalID == suc.SucursalID)
                    .ToList();
                if (lisPresSimMov.Count > 0) {
                    psxsDto.PrestamoSimpleMovimiento = lisPresSimMov;
                    psxsDto.Sucursal = suc;
                    listadoResul.Add(psxsDto);
                }
            }
            return listadoResul;
        }


    }
}
