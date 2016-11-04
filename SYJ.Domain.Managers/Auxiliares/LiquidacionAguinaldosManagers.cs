using SYJ.Application.Dto;
using SYJ.Application.Dto.Auxiliares;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers.Auxiliares {
    public class LiquidacionAguinaldosManagers {
        #region CAMPOS PRIVADOS
        private FormLiquidacionDto _FlDto { get; set; }
        private Guid _UserID { get; set; }
        private List<Mensaje> _Mensajes = new List<Mensaje>();
        private SueldosJornalesEntities _Context;
        private DbContextTransaction _DbContexTransaction;
        #region Datos necesarios cargados durante el proceso
        private MovEmpleado _Cabecera = null;
        #endregion
        #endregion
        public LiquidacionAguinaldosManagers(FormLiquidacionDto flDto, Guid userID) {
            _FlDto = flDto;
            _UserID = userID;
        }

        public MensajeDto GenerarLiquidacionesAguinaldos() {
            //Se recorre empleado por empleado
            foreach (var empleadoID in _FlDto.EmpleadosSeleccionados) {

                _Mensajes.Add(new Mensaje() {
                    MensajeDeError = false,
                    MensajeString = "####################################"
                });
                GenerarLiquidacion(empleadoID);
                _Mensajes.Add(new Mensaje() {
                    MensajeDeError = false,
                    MensajeString = "####################################"
                });

            }
            return new MensajeDto() {
                Error = false,
                MensajeDelProceso = "Proceso terminado",
                ObjetoDto = _Mensajes
            };
        }
        public MensajeDto RecuperarDetallesPorMes() {
            using (_Context = new SueldosJornalesEntities()) {
                List<SueldoEmpleadoPorMes> listado = new List<SueldoEmpleadoPorMes>();
                HistoricoSucursalesManagers hsm = new HistoricoSucursalesManagers();

                foreach (var empleadoID in _FlDto.EmpleadosSeleccionados) {
                    var hs = hsm.UltimoSucursales(empleadoID);
                    HistoricoSucursaleDto h = (HistoricoSucursaleDto)hs.ObjetoDto;
                    var sueldoEmpPorMes = new SueldoEmpleadoPorMes();
                    //Se recupera el empleado y se carga
                    var empleadoDb = RecuperarEmpleado(empleadoID);
                    sueldoEmpPorMes.Empleado = new EmpleadoDto() {
                        EmpleadoID = empleadoDb.EmpleadoID,
                        Nombres = empleadoDb.Nombres,
                        Apellidos = empleadoDb.Apellidos,
                        Sucursale = h.Sucursal
                    };
                    //Se recupera los sueldos por mes
                    var sueldosPagados = RecuperarSueldosCobrados(empleadoID);
                    //Se cargan los meses
                    sueldoEmpPorMes.Meses = new List<Mes>();
                    for (int i = 1; i <= 12; i++) {
                        Mes mes = new Mes();
                        mes.Numero = i;
                        mes.Monto = sueldosPagados
                            .Where(s => s.MesAplicacion.Month == i).Sum(s => s.Monto);
                        sueldoEmpPorMes.Meses.Add(mes);
                    }
                    //Se calcula el aguinaldo
                    sueldoEmpPorMes.Aguinaldo = RecuperarAguinaldoDelAno(empleadoID);
                    //Se calcula el total cobrado
                    sueldoEmpPorMes.TotalCobrado = sueldoEmpPorMes.Meses.Sum(s => s.Monto);
                    //Se carga la cabecera
                    sueldoEmpPorMes.MovEmpleadoID = _Context.MovEmpleadosDets
                       .Where(m =>
                       m.EmpleadoID == empleadoID &&
                       m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.TotalPagado &&
                       m.MesAplicacion.Year == _FlDto.Year)
                       .First().MovEmpleadoID;
                    //Se carga en la coleccion
                    listado.Add(sueldoEmpPorMes);
                }
                #region TOTALES
                var totales = new SueldoEmpleadoPorMes();
                totales.Meses = new List<Mes>();
                totales.Empleado = new EmpleadoDto() {
                    EmpleadoID = 0,
                    Nombres = "TOTALES"
                };
                for (int i = 1; i <= 12; i++) {
                    Mes mes = new Mes();
                    decimal monto = 0;
                    foreach (SueldoEmpleadoPorMes s in listado) {
                        monto += s.Meses.Where(w => w.Numero == i).Sum(t => t.Monto);
                    }
                    mes.Monto = monto;
                    totales.Meses.Add(mes);
                }
                totales.Aguinaldo = listado.Sum(l => l.Aguinaldo);
                totales.TotalCobrado = listado.Sum(l=>l.TotalCobrado);

                listado.Add(totales);
                #endregion

                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Datos de liquidacion por mes recuperado",
                    ObjetoDto = listado
                };
            }
        }

        #region METODOS PRIVADOS PARA GENERAR DETALLES
        private decimal RecuperarAguinaldoDelAno(long empleadoID) {
            //Se carga los meses que cobro su sueldo del año 
            var aguinaldoMov = _Context.MovEmpleadosDets
                .Where(m =>
                m.EmpleadoID == empleadoID &&
                m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Aguinaldo &&
                m.MesAplicacion.Year == _FlDto.Year)
                .FirstOrDefault();
            decimal aguinaldo = 0;
            if (aguinaldoMov != null) {
                aguinaldo = aguinaldoMov.Monto;
            }
            return aguinaldo;
        }
        #endregion
        #region METODOS PRIVADOS UTILIZADOS PARA GENERAR AGUINALDOS
        private void GenerarLiquidacion(long empleadoID) {
            using (_Context = new SueldosJornalesEntities()) {
                using (_DbContexTransaction = _Context.Database.BeginTransaction()) {
                    var empleado = RecuperarEmpleado(empleadoID);

                    if (!CargarCabeceraDeMovimiento(empleado)) { return; };
                    if (!CargarElDetalle(empleado)) { return; };
                    _DbContexTransaction.Commit();
                }
            }
        }
        private bool CargarElDetalle(Empleado empleado) {
            MensajeDto mensajeDto = null;
            MovEmpleadosDet detalle = new MovEmpleadosDet();
            detalle.MovEmpleadoID = _Cabecera.MovEmpleadoID;
            detalle.EmpleadoID = empleado.EmpleadoID;
            detalle.DevCred = Liquidacion.DevCred.Devito;
            detalle.MesAplicacion = new DateTime(_FlDto.Year, 12, 1);
            detalle.LiquidacionConceptoID = (int)Liquidacion.Conceptos.Aguinaldo;
            var sueldosPagados = RecuperarSueldosCobrados(empleado.EmpleadoID);
            //Se calcula el aguinaldo
            detalle.Monto = sueldosPagados.Sum(s => s.Monto) / 12;
            //Se carga los datos
            MovEmpleadosDet movAguinaldoDb = null;
            if (YaSeGeneroLiquidacionAguinaldoSn(empleado.EmpleadoID, out movAguinaldoDb)) {
                movAguinaldoDb.Monto = sueldosPagados.Sum(s => s.Monto) / 12;
                _Context.Entry(movAguinaldoDb).State = System.Data.Entity.EntityState.Modified;
            } else {
                _Context.MovEmpleadosDets.Add(detalle);
            }
            mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
            //Manejo de errores
            if (mensajeDto != null) {
                _Mensajes.Add(new Mensaje() {
                    MensajeDeError = true,
                    MensajeString = "#ERROR# en la generacion del detalle para " + empleado.Nombres + empleado.Apellidos +
                    mensajeDto.MensajeDelProceso
                });
                _DbContexTransaction.Rollback();
                return false;
            } else {
                _Mensajes.Add(new Mensaje() {
                    MensajeDeError = false,
                    MensajeString = "Detalle generada CORRECTAMENTE: " + empleado.Nombres + " " + empleado.Apellidos
                });
                return true;
            }
        }
        private List<MovEmpleadosDet> RecuperarSueldosCobrados(long empleadoID) {
            //Se carga los meses que cobro su sueldo del año 
            var sueldosPagados = _Context.MovEmpleadosDets
                .Where(m =>
                m.EmpleadoID == empleadoID &&
                m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.SueldoBase &&
                m.MesAplicacion.Year == _FlDto.Year)
                .ToList();
            var comisionesCobradas = _Context.MovEmpleadosDets
               .Where(m =>
               m.EmpleadoID == empleadoID &&
               m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Comision &&
               m.MesAplicacion.Year == _FlDto.Year)
               .ToList();
            sueldosPagados.AddRange(comisionesCobradas);
            return sueldosPagados;
        }
        private Empleado RecuperarEmpleado(long empleadoID) {
            //Se recupera los datos del empleado
            var empleado = _Context.Empleados
                .Where(e => e.EmpleadoID == empleadoID).First();
            return empleado;
        }
        /// <summary>
        /// Carga la cabecera del movimiento
        /// </summary>
        /// <param name="empleado"></param>
        /// <returns></returns>
        private bool CargarCabeceraDeMovimiento(Empleado empleado) {
            MensajeDto mensajeDto = null;
            //Se carga la cabecera del movimiento
            var movimiento = new MovEmpleado();
            movimiento.FechaMovimiento = DateTime.Now;
            movimiento.Descripcion = "Liquidacion Aguinaldo " +
                empleado.Nombres + " " + empleado.Apellidos +
                " año: " + _FlDto.Year;
            var usuarioDb = _Context.Usuarios.Where(u => u.UserID == _UserID).FirstOrDefault();
            if (usuarioDb == null) {
                _Mensajes.Add(new Mensaje() {
                    MensajeDeError = true,
                    MensajeString = "#ERROR# de logeo. "
                });
                _DbContexTransaction.Rollback();
                return false;
            }
            movimiento.UsuarioID = usuarioDb.UsuarioID;

            movimiento.MomentoCarga = DateTime.Now;
            //Se carga los datos
            _Context.MovEmpleados.Add(movimiento);
            mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
            //Manejo de errores
            if (mensajeDto != null) {
                _Mensajes.Add(new Mensaje() {
                    MensajeDeError = true,
                    MensajeString = "#ERROR# en la generacion de la cabecera movimiento para " + empleado.Nombres + empleado.Apellidos +
                    mensajeDto.MensajeDelProceso
                });
                _DbContexTransaction.Rollback();
                return false;
            } else {
                _Cabecera = movimiento;
                _Mensajes.Add(new Mensaje() {
                    MensajeDeError = false,
                    MensajeString = "Cabecera generada CORRECTAMENTE: " + empleado.Nombres + " " + empleado.Apellidos
                });
                return true;
            }
        }
        /// <summary>
        /// Evalua si se genero el aguinaldo para el mes correspondiente
        /// </summary>
        /// <param name="empleadoID"></param>
        /// <returns>Retona verdadero si ys se genero la liquidacion</returns>
        private bool YaSeGeneroLiquidacionAguinaldoSn(long empleadoID,
            out MovEmpleadosDet movEmpleado) {
            movEmpleado = null;
            using (var context = new SueldosJornalesEntities()) {
                //Se recupera el movimiento de liquidacion de aguinaldo
                var movDet = context.MovEmpleadosDets
                   .Where(m =>
                   m.DevCred == Liquidacion.DevCred.Devito &&
                   m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Aguinaldo &&
                   m.MesAplicacion.Year == _FlDto.Year &&
                   m.EmpleadoID == empleadoID);
                //Se busca los datos del empleado para informarlo
                var empleado = context.Empleados
                      .Where(e => e.EmpleadoID == empleadoID)
                      .Select(s => new EmpleadoDto() {
                          EmpleadoID = s.EmpleadoID,
                          Nombres = s.Nombres,
                          Apellidos = s.Apellidos
                      }).First();
                var nombre = "(" + empleado.Nombres + " " + empleado.Apellidos + ") ";

                if (movDet.Count() > 0) {//Si ya se genero aguinaldo para este empleado
                    movEmpleado = movDet.First();
                    _Mensajes.Add(new Mensaje() {
                        MensajeDeError = false,
                        MensajeString = nombre + " Ya fue generado la liquidacion. " + " mes: " + _FlDto.Mes.MesID + "/" + _FlDto.Year
                    });
                    return true;
                } else {//Si todavia no se genero                   
                    _Mensajes.Add(new Mensaje() {
                        MensajeDeError = false,
                        MensajeString = nombre + "----->>Liquidacion aun no generada"
                    });
                    return false;
                }
            }
        }
        #endregion
        #region CLASES INTERNAS
        class SueldoEmpleadoPorMes {
            public long MovEmpleadoID { get; set; }
            public EmpleadoDto Empleado { get; set; }
            public List<Mes> Meses { get; set; }
            public decimal Aguinaldo { get; set; }
            public decimal TotalCobrado { get; set; }
        }
        class Mes {
            public int Numero { get; set; }
            public decimal Monto { get; set; }
        }
        class Mensaje {
            public bool MensajeDeError { get; set; }
            public string MensajeString { get; set; }
        }
        #endregion
        /// <summary>
        /// Se recuperan solo los aguinaldos
        /// </summary>
        /// <returns></returns>
        public MensajeDto RecuperarDetallesParaImprimir() {
            using (var context = new SueldosJornalesEntities()) {
                List<MovEmpleadoDto> movimientos = new List<MovEmpleadoDto>();
                GetMovimientos(context, movimientos);
                List<LiquidacionSalarioDto> listLsDto = new List<LiquidacionSalarioDto>();
                foreach (var itemMov in movimientos) {
                    LiquidacionSalarioDto lsDto = new LiquidacionSalarioDto();
                    //--Datos del empleado
                    lsDto.Empleado = itemMov.MovEmpleadosDets.First().Empleado;
                    lsDto.DiasTrabajados = 30;
                    lsDto.SalarioBase = itemMov.MovEmpleadosDets
                        .Where(md => md.LiquidacionConcepto.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Aguinaldo)
                        .First().Debito;//Aqui solo se tiene en cuenta el aguinaldo
                    lsDto.SubTotalIngresos = lsDto.SalarioBase;
                    ///Sucursal
                    var hsm = new HistoricoSucursalesManagers();
                    MensajeDto mensajeSuc = hsm.UltimoSucursales(lsDto.Empleado.EmpleadoID);
                    if (!mensajeSuc.Error) {
                        var historicoSucursale = (HistoricoSucursaleDto)mensajeSuc.ObjetoDto;
                        lsDto.Empleado.Sucursale = historicoSucursale.Sucursal;
                    }
                    ///Cargo
                    HistoricoSalariosManagers hSalariom = new HistoricoSalariosManagers();
                    MensajeDto mensajeSalario = hSalariom.SalarioYCargoActual(lsDto.Empleado.EmpleadoID);
                    if (!mensajeSalario.Error) {
                        var historicoSalarioDto = (HistoricoSalarioDto)mensajeSalario.ObjetoDto;
                        lsDto.Empleado.Cargo = historicoSalarioDto.Cargo;
                    }
                    //----Comisiones-----
                    //No se tiene en cuenta las comisiones
                    lsDto.Comisiones = 0;

                    //-----Total ingresos-----
                    lsDto.TotalIngreso = lsDto.SubTotalIngresos + lsDto.Comisiones;
                    //----descuento IPS------------
                    //NO se tiene en cuenta el ips
                    lsDto.DescIPS = 0;
                    
                    //----anticipos y prestamos OTROS DESCUENTOS------
                    //No se tiene en cuenta los anticipos
                    decimal anticipos = 0;
                    //No se tiene en cuenta los prestamos
                    decimal prestamos = 0;                   

                    lsDto.DescOtros = prestamos; //el anticipo no se coloca con los descuentos por pedido del jefe
                    //----total descuento----------
                    lsDto.TotalDescuentos = lsDto.DescIPS + lsDto.DescOtros;
                    //----neto a cobrar------------
                    lsDto.NetoAcobrar = lsDto.TotalIngreso - lsDto.TotalDescuentos;
                    //----periodo------------------
                    var mesAplicacion = new DateTime(_FlDto.Year,12,1);//Es solo para aguinaldo en diciembre

                    var periodo = mesAplicacion;
                    var ultimoDiaPeriodo = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    lsDto.Periodo = periodo;
                    lsDto.UltimoDiaPeriodo = ultimoDiaPeriodo;
                    //-----mensaje de los calculos
                    lsDto.MensajeCalculos = "Recuperacion Terminada";
                    listLsDto.Add(lsDto);
                }
                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se cargo el listado de aguinaldos segun la sucursal, el mes y el año seleccionado : ",
                    ObjetoDto = listLsDto
                };
            }
        }
        public MensajeDto RecuperarDetallesSubtotalesPorSuc(List<LiquidacionSalarioDto> liquidacionSalarios) {
            using (var context = new SueldosJornalesEntities()) {
                var liqSalariosResul = new List<LiquidacionSalarioDto>();
                var sucursales = context.Sucursales.ToList();

                int contador = 0;
                sucursales.ForEach(delegate(Sucursale s) {
                    //Se lista todos los empleados de usa sola sucursal
                    var liquidacionesSuc = liquidacionSalarios
                        .Where(l => l.Empleado.Sucursale.SucursalID == s.SucursalID)
                        .OrderByDescending(l => l.SalarioBase)
                        .ToList();
                    //Numerar las liquidaciones, es una fila por empleado
                    liquidacionesSuc.ForEach(delegate(LiquidacionSalarioDto ls) {
                        ls.NroItem = ++contador;
                    });
                    if (liquidacionesSuc.Count() > 0) {
                        liqSalariosResul.AddRange(liquidacionesSuc);
                        //Se le carga un subtotal
                        var liqSalario = new LiquidacionSalarioDto();
                        liqSalario.Empleado = new EmpleadoDto();
                        liqSalario.Empleado.EmpleadoID = 0;
                        liqSalario.Empleado.Nombres = "Subtotal " + s.NombreSucursal;
                        liqSalario.SalarioBase = liquidacionesSuc.Sum(l => l.SalarioBase);
                        liqSalario.DescIPS = liquidacionesSuc.Sum(l => l.DescIPS);
                        liqSalario.DescOtros = liquidacionesSuc.Sum(l => l.DescOtros);
                        liqSalario.NetoAcobrar = liquidacionesSuc.Sum(l => l.NetoAcobrar);
                        liqSalariosResul.Add(liqSalario);
                    }
                });
                //Se agrega el gran total
                if (liqSalariosResul.Count > 0) {
                    var liqSalario = new LiquidacionSalarioDto();
                    liqSalario.Empleado = new EmpleadoDto();
                    liqSalario.Empleado.EmpleadoID = 0;
                    liqSalario.Empleado.Nombres = "Gran total ";

                    liqSalario.SalarioBase = liqSalariosResul
                        .Where(l => l.Empleado.EmpleadoID != 0)
                        .Sum(l => l.SalarioBase);
                    liqSalario.DescIPS = liqSalariosResul
                            .Where(l => l.Empleado.EmpleadoID != 0)
                        .Sum(l => l.DescIPS);
                    liqSalario.DescOtros = liqSalariosResul
                            .Where(l => l.Empleado.EmpleadoID != 0)
                        .Sum(l => l.DescOtros);
                    liqSalario.NetoAcobrar = liqSalariosResul
                            .Where(l => l.Empleado.EmpleadoID != 0)
                        .Sum(l => l.NetoAcobrar);
                    liqSalariosResul.Add(liqSalario);
                }
                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se subtotalizo por sucursal el listado: ",
                    ObjetoDto = liqSalariosResul
                };
            }
        }
        /// <summary>
        /// Solo se recuperan los datos de aguinaldos de los empleados
        /// </summary>
        /// <param name="context"></param>
        /// <param name="movimientos"></param>
        private void GetMovimientos(SueldosJornalesEntities context, List<MovEmpleadoDto> movimientos) {
            foreach (var empleadoID in _FlDto.EmpleadosSeleccionados) {
                var movimiento = new MovEmpleadoDto();
                //Se cargan los detalles
                var mesAplicacion = new DateTime(_FlDto.Year, _FlDto.Mes.MesID, 1);
                var movEmpleadosDetsDb = context.MovEmpleadosDets
                    .Where(m => m.EmpleadoID == empleadoID &&
                                m.MesAplicacion.Year == mesAplicacion.Year &&
                                m.MesAplicacion.Month == 12 &&
                                m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Aguinaldo);

                movimiento.MovEmpleadosDets = movEmpleadosDetsDb
                    .Select(s => new MovEmpleadoDetDto() {
                        MovEmpleadoDetID = s.MovEmpleadoDetID,
                        MovEmpleadoID = s.MovEmpleadoID,
                        Empleado = new EmpleadoDto() {
                            EmpleadoID = empleadoID,
                            Nombres = s.Empleado.Nombres,
                            Apellidos = s.Empleado.Apellidos,
                            NroCedula = s.Empleado.NroCedula
                        },
                        Debito = (s.DevCred == Liquidacion.DevCred.Devito) ? s.Monto : 0,
                        Credito = (s.DevCred == Liquidacion.DevCred.Credito) ? s.Monto : 0,
                        MesAplicacion = s.MesAplicacion,
                        LiquidacionConcepto = new LiquidacionConceptoDto() {
                            LiquidacionConceptoID = s.LiquidacionConceptoID,
                            NombreConcepto = s.LiquidacionConcepto.NombreConcepto
                        }
                    }).ToList();
                if (movimiento.MovEmpleadosDets.Count() > 0) {
                    //Se carga la cabecera
                    var movEmpleadoDetDb = movEmpleadosDetsDb
                        .Where(m => m.LiquidacionConceptoID != (int)Liquidacion.Conceptos.Prestamo)
                        .FirstOrDefault();
                    if (movEmpleadoDetDb != null) {
                        var movEmpleadoDb = movEmpleadoDetDb.MovEmpleado;
                        movimiento.MovEmpleadoID = movEmpleadoDb.MovEmpleadoID;
                        movimiento.FechaMovimiento = movEmpleadoDb.FechaMovimiento;
                        movimiento.Descripcion = movEmpleadoDb.Descripcion;
                        //Se carga el listado
                        movimientos.Add(movimiento);
                    }

                }
            }
        }
    }
}
