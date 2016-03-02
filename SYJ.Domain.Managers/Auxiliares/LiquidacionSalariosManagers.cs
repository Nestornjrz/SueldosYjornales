using SYJ.Application.Dto;
using SYJ.Domain.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SYJ.Application.Dto.Auxiliares;
using SYJ.Domain.Managers.Util;

namespace SYJ.Domain.Managers.Auxiliares {
    public class LiquidacionSalariosManagers {
        /// <summary>
        /// Esta informacion viene del cliente que envia este informacion desde un formulario
        /// atraves del web api
        /// </summary>
        private FormLiquidacionDto _FlDto { get; set; }
        private Guid _UserID { get; set; }
        private List<string> _Mensajes = new List<string>();
        private int _CantidadErrores = 0;
        private SueldosJornalesEntities _Context;
        private DbContextTransaction _DbContexTransaction;
        /// <summary>
        /// Este constructor se solicita el mes el año y los codigos de los empleados seleccionados para 
        /// generar la liquidacion de cada uno
        /// </summary>
        /// <param name="flDto">Es el que trae los datos de el mes y el año, ademas de los codigos
        /// de los empleados seleccionados </param>
        /// <param name="userID"></param>
        public LiquidacionSalariosManagers(FormLiquidacionDto flDto, Guid userID) {
            _FlDto = flDto;
            _UserID = userID;
        }
        public MensajeDto GenerarLiquidacionesSalarios() {
            foreach (var empleadoID in _FlDto.EmpleadosSeleccionados) {
                if (!YaSeGeneroLiquidacionParaEmpleadoSn(empleadoID)) {
                    _Mensajes.Add("###################################");
                    GenerarLiquidacionEmpleado(empleadoID);
                    _Mensajes.Add("###################################");
                }
            }
            return new MensajeDto() {
                Error = (_CantidadErrores > 0) ? true : false,
                MensajeDelProceso = "Proceso de liquidacion Terminado",
                ObjetoDto = _Mensajes,
                Valor = _CantidadErrores.ToString()
            };
        }
        public MensajeDto RecuperarDetalles() {
            using (var context = new SueldosJornalesEntities()) {
                List<MovEmpleadoDto> movimientos = new List<MovEmpleadoDto>();
                GetMovimientos(context, movimientos);
                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "Se recuperaron " + movimientos.Count() + " liquidaciones",
                    ObjetoDto = movimientos,
                    Valor = movimientos.Count().ToString()
                };
            }
        }

        public MensajeDto RecuperarDetallesParaImprimir() {
            using (var context = new SueldosJornalesEntities()) {
                List<MovEmpleadoDto> movimientos = new List<MovEmpleadoDto>();
                GetMovimientos(context, movimientos);
                //Se prepara el listado de la liquidacion de salarios Dto
                List<LiquidacionSalarioDto> listLsDto = new List<LiquidacionSalarioDto>();
                foreach (var itemMov in movimientos) {
                    LiquidacionSalarioDto lsDto = new LiquidacionSalarioDto();
                    //--Datos del empleado
                    lsDto.Empleado = itemMov.MovEmpleadosDets.First().Empleado;
                    lsDto.DiasTrabajados = 30;

                    //Se calcula su sueldo segun la fecha de salida si es que cae en el mes que se esta calculando
                    var finMesFechaSeleccionada = new DateTime(_FlDto.Year, _FlDto.Mes.MesID, DateTime.DaysInMonth(_FlDto.Year, _FlDto.Mes.MesID));
                    var fechaSalida = HistoricoIngresoSalidasManagers.fechaSalida(lsDto.Empleado.EmpleadoID, finMesFechaSeleccionada);
                    if (fechaSalida != null) {
                        if (fechaSalida.Value.Year == _FlDto.Year && fechaSalida.Value.Month == _FlDto.Mes.MesID) {
                            int diaSalida = fechaSalida.Value.Day;
                            lsDto.DiasTrabajados = diaSalida;
                        }
                    }

                    lsDto.SalarioBase = itemMov.MovEmpleadosDets
                        .Where(md => md.LiquidacionConcepto.LiquidacionConceptoID == (int)Liquidacion.Conceptos.SueldoBase)
                        .First().Credito;
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
                    MovEmpleadoDetDto comisionesObj = itemMov.MovEmpleadosDets
                        .Where(md => md.LiquidacionConcepto.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Comision)
                        .FirstOrDefault();
                    if (comisionesObj != null) {
                        lsDto.Comisiones = comisionesObj.Credito;
                    }
                    //-----Total ingresos-----
                    lsDto.TotalIngreso = lsDto.SubTotalIngresos + lsDto.Comisiones;
                    //----descuento IPS------------
                    var ipsObj = itemMov.MovEmpleadosDets
                        .Where(md => md.LiquidacionConcepto.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Ips)
                        .FirstOrDefault();
                    if (ipsObj != null) {
                        lsDto.DescIPS = ipsObj.Devito;
                    }
                    //----anticipos y prestamos OTROS DESCUENTOS------
                    var anticiposObj = itemMov.MovEmpleadosDets
                        .Where(md => md.LiquidacionConcepto.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Anticipo)
                        .FirstOrDefault();
                    decimal anticipos = 0;
                    if (anticiposObj != null) {
                        anticipos = anticiposObj.Devito;
                    }
                    var prestamosObj = itemMov.MovEmpleadosDets
                        .Where(md => md.LiquidacionConcepto.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Prestamo);
                    decimal prestamos = 0;
                    if (prestamosObj != null) {
                        prestamos = prestamosObj.Sum(p=>p.Devito);
                    }

                    lsDto.DescOtros = prestamos; //el anticipo no se coloca con los descuentos por pedido del jefe
                    //----total descuento----------
                    lsDto.TotalDescuentos = lsDto.DescIPS + lsDto.DescOtros;
                    //----neto a cobrar------------
                    lsDto.NetoAcobrar = lsDto.TotalIngreso - lsDto.TotalDescuentos;
                    //----periodo------------------
                    var mesAplicacion = itemMov.MovEmpleadosDets
                        .Where(md => md.LiquidacionConcepto.LiquidacionConceptoID == (int)Liquidacion.Conceptos.SueldoBase)
                        .First().MesAplicacion;

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
                    MensajeDelProceso = "Se cargo el listado de salarios segun la sucursal, el mes y el año seleccionado : ",
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
        #region Metodos privados
        /// <summary>
        /// Este metodo solo muestra los movimientos que tengan cualquier concepto 
        /// sin embargo si el unico concepto que tiene es el de prestamo no muestra pues
        /// significa que tiene devitos por prestamo pero ningun movimiento mas para ese mes
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
                                m.MesAplicacion.Month == mesAplicacion.Month);

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
                        Devito = (s.DevCred == Liquidacion.DevCred.Devito) ? s.Monto : 0,
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
        private bool YaSeGeneroLiquidacionParaEmpleadoSn(long empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var mesAplicacion = new DateTime(_FlDto.Year, _FlDto.Mes.MesID, 1);
                var movi = context.MovEmpleadosDets
                    .Where(m => m.DevCred == Liquidacion.DevCred.Devito &&
                    m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.TotalPagado &&
                    m.MesAplicacion == mesAplicacion &&
                    m.EmpleadoID == empleadoID);

                var empleado = context.Empleados
                       .Where(e => e.EmpleadoID == empleadoID)
                       .Select(s => new EmpleadoDto() {
                           EmpleadoID = s.EmpleadoID,
                           Nombres = s.Nombres,
                           Apellidos = s.Apellidos
                       }).First();
                var nombre = "(" + empleado.Nombres + " " + empleado.Apellidos + ") ";
                if (movi.Count() > 0) {
                    _Mensajes.Add(nombre +
                        " ya fue generado su liquidacion mes " +
                        _FlDto.Mes.MesID + "/" + _FlDto.Year +
                        " Liquidacion numero: " + movi.First().MovEmpleadoID
                        );
                    return true;
                } else {
                    _Mensajes.Add(nombre + "---->> Liquidacion aun no generada");
                }
                return false;
            }
        }
        private void GenerarLiquidacionEmpleado(long empleadoID) {
            DatosEmpleado de = new DatosEmpleado();
            using (var context = new SueldosJornalesEntities()) {
                de.Empleado = context.Empleados.Where(e => e.EmpleadoID == empleadoID)
                    .Select(s => new EmpleadoDto() {
                        EmpleadoID = s.EmpleadoID,
                        Nombres = s.Nombres,
                        Apellidos = s.Apellidos
                    }).First();
            }
            using (var context = new SueldosJornalesEntities()) {
                _Context = context;
                using (var dbContextTransaction = context.Database.BeginTransaction()) {
                    _DbContexTransaction = dbContextTransaction;
                    ///Se marcan primero los prestamos pues esta marca genera los devitos
                    ///por prestamos en la tabla MovEmpleados/Dets que luego se va a consultar
                    ///mas adelante en este mismo metodo
                    MarcarPrestamos(de, context);
                    if (_CantidadErrores > 0) { return; }
                    //Creditos
                    de.SueldoBase = RecuperarSueldo(de);
                    de.Comisiones = RecuperarComisiones(de);
                    //Devitos                    
                    de.Anticipos = RecuperarAnticipos(de);

                    //Se realiza la liquidacion
                    if (_CantidadErrores == 0) {
                        RealizarLiquidacionSalario(de);
                    }
                    if (_CantidadErrores == 0) {
                        _DbContexTransaction.Commit();
                    }
                }
            }
            //throw new NotImplementedException();
        }
        private void RealizarLiquidacionSalario(DatosEmpleado de) {
            var nombre = "(" + de.Empleado.Nombres + " " + de.Empleado.Apellidos + ") ";
            MensajeDto mensajeDto = null;
            //SE CARGA LA CABECERA DEL MOVIMIENTO (Tabla MovEmpleados)-------------------------
            var movEmpleado = new MovEmpleado();
            movEmpleado.FechaMovimiento = DateTime.Now;
            movEmpleado.Descripcion = "Liquidacion de salario de " + nombre + " mes: " + _FlDto.Mes.MesID + "/" + _FlDto.Year;
            movEmpleado.UsuarioID = _Context.Usuarios.Where(u => u.UserID == _UserID).First().UsuarioID;
            movEmpleado.MomentoCarga = DateTime.Now;

            _Context.MovEmpleados.Add(movEmpleado);
            mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
            if (mensajeDto != null) {
                _Mensajes.Add("#ERROR# en la generacion de la cabecera del movimiento para " + nombre + " " + mensajeDto.MensajeDelProceso);
                _CantidadErrores += 1;
                _DbContexTransaction.Rollback();
                return;
            } else {
                _Mensajes.Add("Se cargo correctamente la cabecera de movimiento para " + nombre);
            }

            //SE CARGAN LOS DETALLES (Tabla MovEmpleadosDets) ---------------------------------
            #region Se carga el Sueldo Base
            var movEmpleadoDet = new MovEmpleadosDet();
            CargarDatosComunes(de, movEmpleado, movEmpleadoDet);
            movEmpleadoDet.LiquidacionConceptoID = (int)Liquidacion.Conceptos.SueldoBase;
            movEmpleadoDet.DevCred = Liquidacion.DevCred.Credito;
            movEmpleadoDet.Monto = de.SueldoBase;
            //Se calcula su sueldo segun la fecha de salida si es que cae en el mes que se esta calculando
            var finMesFechaSeleccionada = new DateTime(_FlDto.Year, _FlDto.Mes.MesID, DateTime.DaysInMonth(_FlDto.Year, _FlDto.Mes.MesID));           
            var fechaSalida = HistoricoIngresoSalidasManagers.fechaSalida(de.Empleado.EmpleadoID,finMesFechaSeleccionada);
            if (fechaSalida != null) {
                if (fechaSalida.Value.Year == _FlDto.Year && fechaSalida.Value.Month == _FlDto.Mes.MesID) {
                    int diaSalida = fechaSalida.Value.Day;
                    var sueldoPorDia = movEmpleadoDet.Monto / 30;
                    movEmpleadoDet.Monto = sueldoPorDia * diaSalida;
                    //Se carga de nuevo, el sueldo base modificado para que se calcule bien el ips
                    de.SueldoBase = movEmpleadoDet.Monto;
                }
            }
            if (!SeCargoMovEmpleadosDetsSn(movEmpleadoDet, mensajeDto, de, "Sueldo Base")) { return; };
            _Mensajes.Add("Se Cargo correctamente el Sueldo Base de " + nombre);
            #endregion
            ///Carga de comisiones
            movEmpleadoDet = new MovEmpleadosDet();
            CargarDatosComunes(de, movEmpleado, movEmpleadoDet);
            movEmpleadoDet.LiquidacionConceptoID = (int)Liquidacion.Conceptos.Comision;
            movEmpleadoDet.DevCred = Liquidacion.DevCred.Credito;
            movEmpleadoDet.Monto = de.Comisiones.Sum();
            if (movEmpleadoDet.Monto > 0) {
                if (!SeCargoMovEmpleadosDetsSn(movEmpleadoDet, mensajeDto, de, "Comisiones")) { return; };
                _Mensajes.Add("Se Cargo correctamente las comisiones de " + nombre);
            }

            ///Carga de anticipos
            movEmpleadoDet = new MovEmpleadosDet();
            CargarDatosComunes(de, movEmpleado, movEmpleadoDet);
            movEmpleadoDet.LiquidacionConceptoID = (int)Liquidacion.Conceptos.Anticipo;
            movEmpleadoDet.DevCred = Liquidacion.DevCred.Devito;
            movEmpleadoDet.Monto = de.Anticipos.Sum();
            if (movEmpleadoDet.Monto > 0) {
                if (!SeCargoMovEmpleadosDetsSn(movEmpleadoDet, mensajeDto, de, "Anticipos")) { return; };
                _Mensajes.Add("Se Cargo correctamente los anticipos de " + nombre);
            }

            ///Prestamos
            /// LOS PRESTAMOS YA ESTAN CARGADOS POR MEDIO DE TRIGGERS DISPARADOS DESDE LA TABLA PrestamosSimples           

            ///IPS
            ///Se calcula dentro del objeto DatosEmpleado, OJO cargar primero el sueldo base y la comision
            //Se calcula si tiene ips o no
            if (de.Empleado.TieneIpsSn) {
                movEmpleadoDet = new MovEmpleadosDet();
                CargarDatosComunes(de, movEmpleado, movEmpleadoDet);
                movEmpleadoDet.LiquidacionConceptoID = (int)Liquidacion.Conceptos.Ips;
                movEmpleadoDet.DevCred = Liquidacion.DevCred.Devito;
                movEmpleadoDet.Monto = de.Ips;
                if (movEmpleadoDet.Monto > 0) {
                    if (!SeCargoMovEmpleadosDetsSn(movEmpleadoDet, mensajeDto, de, "I.P.S.")) { return; };
                    _Mensajes.Add("Se Cargo correctamente el IPS de " + nombre);
                }
            }

            ///Se carga el TOTAL PAGADO
            /// Se recupera el prestamo del mesAplicacion
            var mesAplicacion = new DateTime(_FlDto.Year, _FlDto.Mes.MesID, 1);
            var devCred = Liquidacion.DevCred.Devito;
            int liquidacionConcepto = (int)Liquidacion.Conceptos.Prestamo;
            decimal prestamos = 0;
            var prestamosDb = _Context.MovEmpleadosDets
                .Where(md => md.MesAplicacion.Year == mesAplicacion.Year &&
                             md.MesAplicacion.Month == mesAplicacion.Month &&
                       md.LiquidacionConceptoID == liquidacionConcepto &&
                       md.DevCred == devCred &&
                       md.EmpleadoID == de.Empleado.EmpleadoID);
            if (prestamosDb.Count() > 0) {
                prestamos = prestamosDb.Sum(s => s.Monto);
            }

            var totalCredito = de.SueldoBase + de.Comisiones.Sum();
            var totalDevito = de.Anticipos.Sum() + prestamos + de.Ips;

            var totalPagado = totalCredito - totalDevito;

            movEmpleadoDet = new MovEmpleadosDet();
            CargarDatosComunes(de, movEmpleado, movEmpleadoDet);
            movEmpleadoDet.LiquidacionConceptoID = (int)Liquidacion.Conceptos.TotalPagado;
            movEmpleadoDet.DevCred = Liquidacion.DevCred.Devito;
            movEmpleadoDet.Monto = totalPagado;
            if (!SeCargoMovEmpleadosDetsSn(movEmpleadoDet, mensajeDto, de, "Total Pagado")) { return; };
            _Mensajes.Add("Se Cargo correctamente el total pagado de " + nombre);
        }
        private void MarcarPrestamos(DatosEmpleado de, SueldosJornalesEntities context) {
            var nombre = "(" + de.Empleado.Nombres + " " + de.Empleado.Apellidos + ") ";
            var prestamosSimples = context.PrestamosSimples
                .Where(p => p.EmpleadoID == de.Empleado.EmpleadoID &&
                            p.Fecha1erVencimiento.Year == _FlDto.Year &&
                            p.Fecha1erVencimiento.Month == _FlDto.Mes.MesID)
                .Select(s => new PrestamoSimpleDto() {
                    PrestamoSimpleID = s.PrestamoSimpleID,
                    EmpleadoID = s.EmpleadoID,
                    Cuotas = s.Cuotas,
                    Fecha1erVencimiento = s.Fecha1erVencimiento,
                    Monto = s.Monto,
                    Observacion = s.Observacion
                }).ToList();

            var resul = new List<decimal>();
            if (prestamosSimples.Count() < 1) {
                _Mensajes.Add(nombre + " No posee prestamos");
            }
            foreach (var item in prestamosSimples) {
                //Se marca el prestamo para que genere los devitos
                //No hace falta cargarlos pues este proceso dispara triggers y estos cargan
                //los devitos de los prestamos
                MensajeDto mensajeDto = null;
                var prestamoSimple = context.PrestamosSimples
                    .Where(p => p.PrestamoSimpleID == item.PrestamoSimpleID).First();

                if (prestamoSimple.GenerarDevitoSn == false || prestamoSimple.GenerarDevitoSn == null) {//Solo si hay una verdadera actualizacion
                    prestamoSimple.GenerarDevitoSn = true;
                    context.Entry(prestamoSimple).State = System.Data.Entity.EntityState.Modified;
                    mensajeDto = AgregarModificar.Hacer(context, mensajeDto);

                    if (mensajeDto != null) {
                        _Mensajes.Add("#ERROR# en la marca del prestamo simple de " + nombre + " " + mensajeDto.MensajeDelProceso);
                        _CantidadErrores++;
                        return;
                    } else {
                        _Mensajes.Add(nombre + "Devitos para el prestamo " + item.PrestamoSimpleID +
                            " generados correctamente");
                    }
                } else {
                    _Mensajes.Add(nombre + "Los devitos para el prestamo simple numero " + item.PrestamoSimpleID +
                          " ya fueron generados");
                }
            }
        }
        private List<decimal> RecuperarAnticipos(DatosEmpleado de) {
            var nombre = "(" + de.Empleado.Nombres + " " + de.Empleado.Apellidos + ") ";
            var anticipos = _Context.Anticipos
                .Where(a => a.EmpleadoID == de.Empleado.EmpleadoID &&
                            a.FechaAnticipo.Year == _FlDto.Year &&
                            a.FechaAnticipo.Month == _FlDto.Mes.MesID)
                .Select(s => new AnticipoDto() {
                    AnticipoID = s.AnticipoID,
                    EmpleadoID = s.EmpleadoID,
                    FechaAnticipo = s.FechaAnticipo,
                    MontoAnticipo = s.MontoAnticipo,
                    Observacion = s.Observacion
                }).ToList();
            if (anticipos.Count() < 1) {
                _Mensajes.Add(nombre + " No posee anticipos");
            }
            List<decimal> resul = new List<decimal>();
            foreach (AnticipoDto item in anticipos) {
                MensajeDto mensajeDto = null;
                var anticipoDb = _Context.Anticipos
                    .Where(a => a.AnticipoID == item.AnticipoID).First();

                anticipoDb.DevitoGeneradoSn = true;
                _Context.Entry(anticipoDb).State = System.Data.Entity.EntityState.Modified;

                mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
                if (mensajeDto != null) {
                    _Mensajes.Add("#ERROR# en la marca del anticipo de " + nombre + " " + mensajeDto.MensajeDelProceso);
                    _DbContexTransaction.Rollback();
                    return resul;
                } else {
                    _Mensajes.Add(nombre + "Devitos de anticipos generadors correctamente");
                }
                resul.Add(item.MontoAnticipo);
            }
            return resul;
        }
        private List<decimal> RecuperarComisiones(DatosEmpleado de) {
            var nombre = "(" + de.Empleado.Nombres + " " + de.Empleado.Apellidos + ") ";
            var listado = _Context.Comisiones
                 .Where(c => c.EmpleadoID == de.Empleado.EmpleadoID &&
                             c.FechaComision.Month == _FlDto.Mes.MesID &&
                             c.FechaComision.Year == _FlDto.Year)
                 .Select(s => new ComisioneDto() {
                     ComisionID = s.ComisionID,
                     EmpleadoID = s.EmpleadoID,
                     FechaComision = s.FechaComision,
                     MontoComision = s.MontoComision,
                     Observacion = s.Observacion
                 }).ToList();
            if (listado.Count() < 1) {
                _Mensajes.Add(nombre + " No posee comisiones");
            }
            //Se marca las comisiones
            List<decimal> resul = new List<decimal>();
            foreach (ComisioneDto item in listado) {
                MensajeDto mensajeDto = null;
                var comisionDb = _Context.Comisiones
                    .Where(c => c.ComisionID == item.ComisionID).First();
                comisionDb.CreditoGeneradoSn = true;

                _Context.Entry(comisionDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
                if (mensajeDto != null) {
                    _Mensajes.Add("#ERROR# en la marca en la comision de " + nombre + " " + mensajeDto.MensajeDelProceso);
                    _DbContexTransaction.Rollback();
                } else {
                    _Mensajes.Add(nombre + "Creditos de Comisiones generadors correctamente");
                    resul.Add(item.MontoComision);
                }
            }
            return resul;
        }
        /// <summary>
        /// Aqui se recupera el sueldo, el cargo y tambien si tiene o no IPS
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        private decimal RecuperarSueldo(DatosEmpleado de) {
            var nombre = "(" + de.Empleado.Nombres + " " + de.Empleado.Apellidos + ") ";

            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();            
            var finMesFechaSeleccionada = new DateTime(_FlDto.Year,_FlDto.Mes.MesID,DateTime.DaysInMonth(_FlDto.Year,_FlDto.Mes.MesID));           
            MensajeDto mensaje = hsm.SalarioYCargo(de.Empleado.EmpleadoID, finMesFechaSeleccionada);
            if (mensaje.Error) {
                _Mensajes.Add("#ERROR# " + nombre + mensaje.MensajeDelProceso);
                _CantidadErrores += 1;
                return 0;
            }
            _Mensajes.Add(nombre + mensaje.MensajeDelProceso);
            var historicoSalarioDto = (HistoricoSalarioDto)mensaje.ObjetoDto;
            de.Empleado.TieneIpsSn = historicoSalarioDto.Ips_Sn;
            return decimal.Parse(mensaje.Valor);
        }
        #endregion
        #region AuxiliaresEnLaCargaDeSalario
        private bool SeCargoMovEmpleadosDetsSn(MovEmpleadosDet movEmpleadoDet, MensajeDto mensajeDto, DatosEmpleado de, string descripcion) {
            var nombre = "(" + de.Empleado.Nombres + " " + de.Empleado.Apellidos + ") ";
            _Context.MovEmpleadosDets.Add(movEmpleadoDet);
            mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
            if (mensajeDto != null) {
                _Mensajes.Add(descripcion + nombre + " " + mensajeDto.MensajeDelProceso);
                _CantidadErrores += 1;
                _DbContexTransaction.Rollback();
                return false;
            }
            return true;
        }

        private void CargarDatosComunes(DatosEmpleado de, MovEmpleado movEmpleado, MovEmpleadosDet movEmpleadoDet) {
            movEmpleadoDet.MovEmpleadoID = movEmpleado.MovEmpleadoID;
            movEmpleadoDet.EmpleadoID = de.Empleado.EmpleadoID;
            movEmpleadoDet.MesAplicacion = new DateTime(_FlDto.Year, _FlDto.Mes.MesID, 1);
        }
        #endregion
        #region CLASES Y ENUM
        class DatosEmpleado {
            public EmpleadoDto Empleado { get; set; }
            public decimal SueldoBase { get; set; }
            /// <summary>
            /// La solisitud hecha a esta propiedad deveria realizarse una vez cargados
            /// el sueldo base y las comisiones
            /// </summary>
            public decimal Ips {
                get {
                    return ((this.SueldoBase + this.Comisiones.Sum()) / 100) * 9;
                }
            }
            public List<decimal> Comisiones { get; set; }
            public List<decimal> Anticipos { get; set; }
            ///Los prestamos ya se generan automanticamente mediante un trigger
            /// en la tabla de PrestamosSimples
            //public List<decimal> Prestamos { get; set; }
        }

        #endregion


        /// <summary>
        /// Esta metodo estatico es utilizado para eliminar el movimiento y los detalles del movimiento
        /// para que despues pueda recrearse de nuevo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MensajeDto EliminarMovimiento(int id) {
            using (var context = new SueldosJornalesEntities()) {
                using (var dbContextTransaction = context.Database.BeginTransaction()) {
                    MensajeDto mensajeDto = null;
                    //Se eliminar los detalles
                    var detalles = context.MovEmpleadosDets.Where(m => m.MovEmpleadoID == id);
                    context.MovEmpleadosDets.RemoveRange(detalles);
                    mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                    if (mensajeDto != null) { return mensajeDto; }
                    //Se elimina la cabecera del movimiento
                    var cabecera = context.MovEmpleados.Where(m => m.MovEmpleadoID == id).First();
                    context.MovEmpleados.Remove(cabecera);
                    mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                    if (mensajeDto != null) { return mensajeDto; }

                    dbContextTransaction.Commit();

                    return new MensajeDto() {
                        Error = false,
                        MensajeDelProceso = "Se elimino la liquidacion numero : " + id
                    };
                }
            }
        }
    }
}
