using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Auxiliares.PrestamoSimple;
using SYJ.Domain.Managers.Util;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SYJ.Domain.Managers.Auxiliares {
    /// <summary>
    /// Esta clase maneja la modificacion de una cuota, dentro de la liquidacion de salario
    /// </summary>
    public class ModificarPrestamosManagers {
        private SueldosJornalesEntities _Context;
        private DbContextTransaction _DbContexTransaction;
        /// <summary>
        /// Representa la fila del detalle del prestamo a modificar, tiene especificamente
        /// la informacion del prestamo en cuestion que esta queriendo modificarse
        /// </summary>
        private MovEmpleadoDetDto _MovEmpleadoDet { get; set; }
        private List<string> _Mensajes = new List<string>();
        private int _CantidadErrores = 0;
        private CompartirDatos _CompartirDatos = new CompartirDatos();

        public ModificarPrestamosManagers(MovEmpleadoDetDto meDto) {
            _MovEmpleadoDet = meDto;
        }
        public MensajeDto ModificarPrestamo() {
            using (var context = new SueldosJornalesEntities()) {
                _Context = context;
                using (var dbContextTransaction = context.Database.BeginTransaction()) {
                    _DbContexTransaction = dbContextTransaction;
                    if (EvitarQueSeModifiqueSiYaSeGeneroLiquidacionMesSiguiente()) { return new MensajeDto { Error = true, ObjetoDto = _Mensajes }; }
                    if (!RecuperarElPrestamoSimple()) { return new MensajeDto { Error = true, ObjetoDto = _Mensajes }; }
                    CargarDatosBasicosAutilizar();
                    if (!AveriguarMontoPrestamoDelDb()) { return new MensajeDto { Error = true, ObjetoDto = _Mensajes }; }
                    if (ElMontoPrestamoDbDelClienteSuperaLimite()) { return new MensajeDto { Error = true, ObjetoDto = _Mensajes }; }
                    if (!SonIgualesLosMontosDeLaCabeceraYdetalle()) { return new MensajeDto { Error = true, ObjetoDto = _Mensajes }; }
                    ModificarMontoPrestamoDelDb();
                    _CompartirDatos.CalcularDifMontoPrestamo();
                    if (!ModificarElTotalPagado()) {
                        return Error_ModificarPrestamosSinRollback();
                    }
                    if (!RegularizarCuotasFuturasDelPrestamo()) {
                        return Error_ModificarPrestamosSinRollback();
                    }
                    VerificarSiquedaCuadradoElMovimiento();
                    if (_CantidadErrores < 1) {
                        _DbContexTransaction.Commit();
                        return new MensajeDto() {
                            Error = false,
                            MensajeDelProceso = "Se logro modificar la cuota del prestamo",
                            ObjetoDto = _Mensajes
                        };
                    } else {
                        return Error_ModificarPrestamos();
                    }
                }
            }
        }

        private bool SonIgualesLosMontosDeLaCabeceraYdetalle() {
            ///Se recupera el prestamo
            var prestamoObjDb = _Context.PrestamosSimples
                .Where(p => p.MovEmpleadoID == _MovEmpleadoDet.MovEmpleadoID)
                .FirstOrDefault();
            decimal montoPrestamo = 0;
            if (prestamoObjDb != null) {
                montoPrestamo = prestamoObjDb.Monto;
            }
            //Se recupera los detalles del prestamo
            var detallesPrestamoDb = _Context.MovEmpleadosDets
                .Where(d => d.MovEmpleadoID == _MovEmpleadoDet.MovEmpleadoID &&
                          d.DevCred == true)//Devito
                .ToList();
            decimal totalMontoDetalle = 0;
            if (detallesPrestamoDb.Count > 0) {
                totalMontoDetalle = detallesPrestamoDb.Sum(d => d.Monto);
            }
            if (montoPrestamo == totalMontoDetalle) {
                _Mensajes.Add("Los montos son iguales");
                return true;
            } else {
                _Mensajes.Add("#ERROR# El monto total del prestamo (" + montoPrestamo.ToString("N0") + ") y la suma de sus detalles " +
                     totalMontoDetalle.ToString("N0") + " no coinciden");
                return false;
            }
        }

        private MensajeDto Error_ModificarPrestamos() {
            _Mensajes.Add("===== Error en el proceso =======");
            _DbContexTransaction.Rollback();
            return new MensajeDto() {
                Error = true,
                MensajeDelProceso = "#ERROR# no se logro modificar la cuota del prestamo",
                ObjetoDto = _Mensajes
            };
        }
        private MensajeDto Error_ModificarPrestamosSinRollback() {
            _Mensajes.Add("===== Error en el proceso =======");
            return new MensajeDto() {
                Error = true,
                MensajeDelProceso = "#ERROR# no se logro modificar la cuota del prestamo",
                ObjetoDto = _Mensajes
            };
        }

        private bool ElMontoPrestamoDbDelClienteSuperaLimite() {
            //Se calcula lo que falta pagar todavia
            var movEmpleadosDets = _Context.MovEmpleadosDets
              .Where(m => m.MesAplicacion >= _MovEmpleadoDet.MesAplicacion &&
                          m.EmpleadoID == _MovEmpleadoDet.Empleado.EmpleadoID &&
                          m.MovEmpleadoID == _MovEmpleadoDet.MovEmpleadoID)
              .ToList();
            var totalDeudaApagarDelPrestamo = movEmpleadosDets.Sum(s => s.Monto);
            if (_CompartirDatos.MontoCuotaPrestamoDelCliente > totalDeudaApagarDelPrestamo) {
                _Mensajes.Add("#ERROR# El monto de la cuota no puede superar el total de lo adeudado por el prestamo. Total de deuda del prestamo (lo que falta): " + totalDeudaApagarDelPrestamo.ToString("N0"));
                _CantidadErrores++;
                return true;
            }
            return false;
        }

        private bool VerificarSiquedaCuadradoElMovimiento() {
            var nombre = "(" + _MovEmpleadoDet.Empleado.Nombres + " " + _MovEmpleadoDet.Empleado.Apellidos + ")";
            var movEmpleadosDets = _Context.MovEmpleadosDets
                .Where(m => m.MesAplicacion.Month == _MovEmpleadoDet.MesAplicacion.Month &&
                            m.MesAplicacion.Year == _MovEmpleadoDet.MesAplicacion.Year &&
                            m.EmpleadoID == _MovEmpleadoDet.Empleado.EmpleadoID)
                .ToList();
            var creditos = movEmpleadosDets
                .Where(m => m.DevCred == Liquidacion.DevCred.Credito)
                .Sum(m => m.Monto);
            var devitos = movEmpleadosDets
                .Where(m => m.DevCred == Liquidacion.DevCred.Devito)
                .Sum(m => m.Monto);
            if ((devitos - creditos) != 0) {
                _Mensajes.Add("#ERROR#  " + nombre + " Los movimientos de esta liquidacion NO ESTAN CUADRADOS: La diferencia es de: " + (devitos - creditos));
                _CantidadErrores += 1;
                _DbContexTransaction.Rollback();
                return false;
            }
            return true;
        }

        private bool EvitarQueSeModifiqueSiYaSeGeneroLiquidacionMesSiguiente() {
            var nombre = "(" + _MovEmpleadoDet.Empleado.Nombres + " " + _MovEmpleadoDet.Empleado.Apellidos + ")";
            var mesSiguiente = _MovEmpleadoDet.MesAplicacion.AddMonths(1);
            var movEmpleadoDetSiguiente = _Context.MovEmpleadosDets
                          .Where(m => m.MesAplicacion.Year == mesSiguiente.Year &&
                                    m.MesAplicacion.Month == mesSiguiente.Month &&
                                    m.EmpleadoID == _MovEmpleadoDet.Empleado.EmpleadoID &&
                                    m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.TotalPagado)
                          .FirstOrDefault();
            if (movEmpleadoDetSiguiente != null) {
                _Mensajes.Add("#ERROR# No se puede modificar pues ya existe para el mes siguiente, UNA LIQUIDACION GENERADA " + nombre);
                _CantidadErrores += 1;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Carga la propiedad privada _CompartirDatos.MontoCuotaPrestamoDelCliente que es el nuevo
        /// monto de la cuota del prestamo
        /// </summary>
        private void CargarDatosBasicosAutilizar() {
            _CompartirDatos.MontoCuotaPrestamoDelCliente = _MovEmpleadoDet.Debito;
        }

        private bool RegularizarCuotasFuturasDelPrestamo() {
            var nombre = "(" + _MovEmpleadoDet.Empleado.Nombres + " " + _MovEmpleadoDet.Empleado.Apellidos + ")";
            MensajeDto mensajeDto = null;
            var mesSiguiente = _MovEmpleadoDet.MesAplicacion.AddMonths(1);
            var movEmpleadoDetSiguiente = _Context.MovEmpleadosDets
                .Where(m => m.MesAplicacion.Month == mesSiguiente.Month &&
                          m.MesAplicacion.Year == mesSiguiente.Year &&
                          m.EmpleadoID == _MovEmpleadoDet.Empleado.EmpleadoID &&
                          m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.Prestamo)
                .FirstOrDefault();
            if (movEmpleadoDetSiguiente != null) {
                movEmpleadoDetSiguiente.Monto += _CompartirDatos.DifMontoPrestamo;
                _Context.Entry(movEmpleadoDetSiguiente).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
                if (mensajeDto != null) {
                    _Mensajes.Add("#ERROR# en la modificacion del monto del prestamo " + nombre + " " + mensajeDto.MensajeDelProceso);
                    _DbContexTransaction.Rollback();
                    return false;
                } else {
                    _Mensajes.Add("Se logro modificar el monto de la cuota posterior a la que se modifico, quedando en " + movEmpleadoDetSiguiente.Monto);
                    return true;
                }
            } else {
                AgregarNuevaCuota();
                return true;
            }
        }

        private void AgregarNuevaCuota() {
            AgregarNuevaCuotaManagers addNuevaCuota = new AgregarNuevaCuotaManagers(_Context, _DbContexTransaction);
            var mensaje = addNuevaCuota.AgregarCuota(_MovEmpleadoDet, _CompartirDatos.DifMontoPrestamo);
            _Mensajes.AddRange((List<string>)mensaje.ObjetoDto);
        }
        /// <summary>
        /// Se modifica el Total pagado de esta liquidacion devido a que cambio el monto de la cuota del prestamo
        /// </summary>
        private bool ModificarElTotalPagado() {
            MensajeDto mensajeDto = null;
            var nombre = "(" + _MovEmpleadoDet.Empleado.Nombres + " " + _MovEmpleadoDet.Empleado.Apellidos + ")";

            var totalPagadoDetDb = _Context.MovEmpleadosDets
                .Where(m => m.MesAplicacion.Month == _MovEmpleadoDet.MesAplicacion.Month &&
                            m.MesAplicacion.Year == _MovEmpleadoDet.MesAplicacion.Year &&
                            m.EmpleadoID == _MovEmpleadoDet.Empleado.EmpleadoID &&
                            m.LiquidacionConceptoID == (int)Liquidacion.Conceptos.TotalPagado)
             .FirstOrDefault();
            if (totalPagadoDetDb == null) {
                _Mensajes.Add("#ERROR# no se encontro el detalle del prestamo actual");
                _CantidadErrores += 1;
                _DbContexTransaction.Rollback();
                return false;
            }
            var totalPagado = totalPagadoDetDb.Monto;
            var totalPagadoModificado = totalPagado + _CompartirDatos.DifMontoPrestamo;

            totalPagadoDetDb.Monto = totalPagadoModificado;
            _Context.Entry(totalPagadoDetDb).State = System.Data.Entity.EntityState.Modified;
            mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);

            if (mensajeDto != null) {
                _Mensajes.Add("#ERROR# no se pudo modificar el total pagado " + nombre + " " + mensajeDto.MensajeDelProceso);
                _DbContexTransaction.Rollback();
                return false;
            }
            _Mensajes.Add("Se modifico el total Pagado " + nombre);
            return true;
        }

        private bool AveriguarMontoPrestamoDelDb() {
            var movEmpleadoDetDb = _Context.MovEmpleadosDets
                .Where(m => m.MovEmpleadoDetID == _MovEmpleadoDet.MovEmpleadoDetID)
                .FirstOrDefault();
            if (movEmpleadoDetDb == null) {
                _Mensajes.Add("#ERROR# no se encontro el detalle del prestamo actual");
                _CantidadErrores += 1;
                _DbContexTransaction.Rollback();
                return false;
            }
            _Mensajes.Add("Se recupero el monto del prestamo actual");
            _CompartirDatos.MontoCuotaPrestamoDelDb = movEmpleadoDetDb.Monto;
            return true;
        }

        private void ModificarMontoPrestamoDelDb() {
            var nombre = "(" + _MovEmpleadoDet.Empleado.Nombres + " " + _MovEmpleadoDet.Empleado.Apellidos + ")";
            MensajeDto mensajeDto = null;
            //Se modifica el monto de la cuota
            var movEmpleadoDet = _Context.MovEmpleadosDets
                .Where(m => m.MovEmpleadoDetID == _MovEmpleadoDet.MovEmpleadoDetID)
                .First();
            movEmpleadoDet.Monto = _CompartirDatos.MontoCuotaPrestamoDelCliente;
            _Context.Entry(movEmpleadoDet).State = System.Data.Entity.EntityState.Modified;

            mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
            if (mensajeDto != null) {
                _Mensajes.Add("#ERROR# al intentar cambiar el monto del prestamo " + nombre + " " + mensajeDto.MensajeDelProceso);
                _DbContexTransaction.Rollback();
                return;
            }
            _Mensajes.Add("Monto del prestamo modificado correctamente " + nombre);
        }
        /// <summary>
        /// Carga la propiedad _CompartirDatos.PrestamoSimple
        /// </summary>
        /// <returns></returns>
        private bool RecuperarElPrestamoSimple() {
            var prestamoSimpleDb = _Context.PrestamosSimples
                .Where(p => p.MovEmpleadoID == _MovEmpleadoDet.MovEmpleadoID)
                .FirstOrDefault();
            if (prestamoSimpleDb == null) {
                _Mensajes.Add("#ERROR# No existe el prestamo con movEmpleadoID " + _MovEmpleadoDet.MovEmpleadoID);
                _CantidadErrores += 1;
                _DbContexTransaction.Rollback();
                return false;
            }
            _CompartirDatos.PrestamoSimple = new PrestamoSimpleDto() {
                Monto = prestamoSimpleDb.Monto,
                Cuotas = prestamoSimpleDb.Cuotas,
                Fecha1erVencimiento = prestamoSimpleDb.Fecha1erVencimiento,
                PrestamoSimpleID = prestamoSimpleDb.PrestamoSimpleID,
                EmpleadoID = prestamoSimpleDb.EmpleadoID,
                Observacion = prestamoSimpleDb.Observacion
            };
            _Mensajes.Add("Se recupero los datos del prestamo simple");
            return true;
        }

        class CompartirDatos {
            public PrestamoSimpleDto PrestamoSimple { get; set; }
            /// <summary>
            /// Es el monto del prestamo (1 cuota) que se esta cambiando, es decir el que 
            /// viene de la base de datos y se va a modificar
            /// </summary>
            public decimal MontoCuotaPrestamoDelDb { get; set; }
            /// <summary>
            /// Es el monto del prestamo que viene del cliente, es decir el que el cliente quiere que sea el nuevo monto de la cuota prestamo
            /// </summary>
            public decimal MontoCuotaPrestamoDelCliente { get; set; }
            /// <summary>
            /// Es la diferencia entre el monto MontoPrestamoDelDb y el MontoPrestamoDelCliente
            /// </summary>
            public decimal DifMontoPrestamo { get; private set; }
            /// <summary>
            /// Es la diferencia entre el monto que esta en la base de datos y lo que el cliente modifico
            /// </summary>
            public void CalcularDifMontoPrestamo() {
                DifMontoPrestamo = MontoCuotaPrestamoDelDb - MontoCuotaPrestamoDelCliente;
            }
        }
    }
}
