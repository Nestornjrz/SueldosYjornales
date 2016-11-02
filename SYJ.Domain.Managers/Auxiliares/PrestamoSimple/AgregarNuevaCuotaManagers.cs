using SYJ.Application.Dto;
using SYJ.Domain.Db;
using SYJ.Domain.Managers.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SYJ.Domain.Managers.Auxiliares.PrestamoSimple {
    public class AgregarNuevaCuotaManagers {
        #region CAMPOS PRIVADOS
        #region DATOS DE LA CUOTA
        /// <summary>
        /// Es el registro de la cuota a modificar de la base de datos        
        /// </summary>
        private MovEmpleadosDet _UltimaCuotaAmodificarDb;
        private PrestamosSimple _CabeceraPrestamoDb;
        //private decimal _MontoQueReemplazaElMontoDeLaCuota;
        //private decimal _MontoOriginalDeLaCuota;
        private decimal _MontoDeLaCuotaNueva;
        private MovEmpleadosDet _NuevaCuotaAgregadaDb;
        #endregion
        private List<Mensaje> _Mensajes = new List<Mensaje>();
        #region BASE DE DATOS
        private SueldosJornalesEntities _Context;
        private DbContextTransaction _DbContextTransaction;
        #endregion
        #endregion
        #region CONSTRUCTORES
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ultimaCuotaAmodificar">Es la cuota que va a ser modificada, gracias a este dato sabemos cual es</param>
        /// <param name="montoNuevo">Es el nuevo monto que el cliente quiere que tenga la ultima cuota</param>
        public AgregarNuevaCuotaManagers(SueldosJornalesEntities context, DbContextTransaction dbContextTransaction) {
            _Context = context;
            _DbContextTransaction = dbContextTransaction;
        }
        #endregion
        public MensajeDto AgregarCuota(MovEmpleadoDetDto ultimaCuotaAmodificar, Decimal montoNuevo) {
            if (!P0_CargarDatosBasicos(ultimaCuotaAmodificar, montoNuevo)) { return _Error_EnAgregarNuevaCuota(); };
            if (!P1_EsLaUltimaCuota()) { return _Error_EnAgregarNuevaCuota(); };
            if (!P2_CargarCabeceraPrestamo()) { return _Error_EnAgregarNuevaCuota(); };
            //if (!P3_ModificarMontoDeLaUltimaCuota()) { return _Error_EnAgregarNuevaCuota(); };
            if (!P4_AgregarUnaCuotaMas()) { return _Error_EnAgregarNuevaCuota(); };
            if (!P5_ModificarLaCantidadCuotaDelPrestamo()) { return _Error_EnAgregarNuevaCuota(); };
            return ProcesoExitoso();
        }

        #region AUXILIARES DEL PROCESO
        private MensajeDto ProcesoExitoso() {
            ///Fin exitoso del proceso si llega hasta aca
            //_DbContextTransaction.Commit(); Aqui no termina con COMMIT porque el objeto que llamo a este objeto realizara el commit
            List<string> m = new List<string>();
            foreach (var mensaje in _Mensajes) {
                if (mensaje.Error) {
                    m.Add("##====" + mensaje.Comentario + "====##");
                } else {
                    m.Add("OK -> " + mensaje.Comentario);
                }
            }
            return new MensajeDto() {
                Error = false,
                MensajeDelProceso = "Proceso de agregar nueva cuota terminado con exito",
                ObjetoDto = m
            };
        }
        private MensajeDto _Error_EnAgregarNuevaCuota() {
            _Mensajes.Add(new Mensaje() { Error = true, Comentario = "===== Error en el proceso =======" });
            _DbContextTransaction.Rollback();
            List<string> m = new List<string>();
            foreach (var mensaje in _Mensajes) {
                if (mensaje.Error) {
                    m.Add("##====" + mensaje.Comentario + "====##");
                } else {
                    m.Add("OK -> " + mensaje.Comentario);
                }
            }
            return new MensajeDto() {
                Error = true,
                MensajeDelProceso = "#ERROR# no se logro modificar la cuota del prestamo",
                ObjetoDto = m
            };
        }
        #endregion
        #region PASOS DEL PROCESO
        private bool P0_CargarDatosBasicos(MovEmpleadoDetDto ultimaCuotaAmodificar, Decimal montoNuevo) {
            //Se recupera la cuota a modificar
            var movEmpleadoDetDb = _Context.MovEmpleadosDets
                .Where(p => p.MovEmpleadoDetID == ultimaCuotaAmodificar.MovEmpleadoDetID)
                .FirstOrDefault();
            if (movEmpleadoDetDb == null) {
                _Mensajes.Add(new Mensaje() { Error = true, Comentario = "#Error: No pudo encontrarse la cuota a modificar en la base de datos" });
                return false;
            }
            //_MontoOriginalDeLaCuota = movEmpleadoDetDb.Monto;
            _MontoDeLaCuotaNueva = montoNuevo;
            _UltimaCuotaAmodificarDb = movEmpleadoDetDb;
            //_MontoQueReemplazaElMontoDeLaCuota = montoNuevo;

            _Mensajes.Add(new Mensaje() { Error = false, Comentario = "Ultima cuota a modificar encontrada en la base de datos" });
            return true;
        }
        private bool P1_EsLaUltimaCuota() {
            var ultimaCuotaDb = _Context.MovEmpleadosDets
                .Where(d => d.MovEmpleadoID == _UltimaCuotaAmodificarDb.MovEmpleadoID)
                .OrderByDescending(o => o.MesAplicacion).First();
            if (_UltimaCuotaAmodificarDb.MovEmpleadoDetID == ultimaCuotaDb.MovEmpleadoDetID) {
                _Mensajes.Add(new Mensaje() { Error = false, Comentario = "Efectivamente es la ultima cuota" });
                return true;
            } else {
                _Mensajes.Add(new Mensaje() { Error = true, Comentario = "#Error: No es la ultima cuota" });
                return false;
            }
        }
        /// <summary>
        /// La cabecera del prestamo simple es necesario pues se modificara la cantidad de cuota pero el monto
        /// total no
        /// </summary>
        /// <returns></returns>
        private bool P2_CargarCabeceraPrestamo() {
            var cabeceraPrestamoDb = _Context.PrestamosSimples
                .Where(p => p.MovEmpleadoID == _UltimaCuotaAmodificarDb.MovEmpleadoID).FirstOrDefault();
            if (cabeceraPrestamoDb == null) {
                _Mensajes.Add(new Mensaje() { Error = true, Comentario = "#Error: No existe el prestamo simple numero:" + _UltimaCuotaAmodificarDb.MovEmpleadoID });
                return false;
            } else {
                _Mensajes.Add(new Mensaje() { Error = false, Comentario = "Prestamo simple encontrado con exito" });
            }
            _CabeceraPrestamoDb = cabeceraPrestamoDb;
            //_CabeceraPrestamo.Empleado = new EmpleadoDto() {
            //    EmpleadoID = cabeceraPrestamoDb.EmpleadoID,
            //    Nombres = cabeceraPrestamoDb.Empleado.Nombres
            //};
            //_CabeceraPrestamo.Fecha1erVencimiento = cabeceraPrestamoDb.Fecha1erVencimiento;
            //_CabeceraPrestamo.Monto = cabeceraPrestamoDb.Monto;
            //_CabeceraPrestamo.Cuotas = cabeceraPrestamoDb.Cuotas;
            //_CabeceraPrestamo.Observacion = cabeceraPrestamoDb.Observacion;
            _Mensajes.Add(new Mensaje() { Error = false, Comentario = "Campo prestamos simple cargado con exito" });
            return true;
        }
        ///// <summary>
        ///// Modifica el monto del devito del prestamo, pues es lo que hace el usuario
        ///// modificar el monto de la ultima cuota pues el deudor no pudo pagar todo
        ///// </summary>
        ///// <returns></returns>
        //private bool P3_ModificarMontoDeLaUltimaCuota() {
        //    MensajeDto mensajeDto = null;
        //    var nombre = "(" + _UltimaCuotaAmodificarDb.Empleado.Nombres + " " + _UltimaCuotaAmodificarDb.Empleado.Apellidos + ")";
        //    _UltimaCuotaAmodificarDb.Monto = _MontoQueReemplazaElMontoDeLaCuota;
        //    _Context.Entry(_UltimaCuotaAmodificarDb).State = System.Data.Entity.EntityState.Modified;

        //    mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
        //    if (mensajeDto != null) {
        //        _Mensajes.Add(new Mensaje() {
        //            Error = true,
        //            Comentario = "#ERROR# al intentar cambiar el monto de la cuota " + nombre + " " + mensajeDto.MensajeDelProceso
        //        });
        //        return false;
        //    }
        //    _Mensajes.Add(new Mensaje() {
        //        Error = false,
        //        Comentario = "El monto de la cuota fue modificada "
        //    });
        //    return true;
        //}
        /// <summary>
        /// Agrega una cuota mas a la tabla MovEmpleadosDet
        /// </summary>
        /// <returns></returns>
        private bool P4_AgregarUnaCuotaMas() {
            MensajeDto mensajeDto = null;
            MovEmpleadosDet cuotaAagregar = new MovEmpleadosDet();
            cuotaAagregar.MovEmpleadoID = _UltimaCuotaAmodificarDb.MovEmpleadoID;
            cuotaAagregar.EmpleadoID = _UltimaCuotaAmodificarDb.EmpleadoID;
            cuotaAagregar.DevCred = _UltimaCuotaAmodificarDb.DevCred;
            cuotaAagregar.Monto = _MontoDeLaCuotaNueva;
            var mesSiguiente = _UltimaCuotaAmodificarDb.MesAplicacion.AddMonths(1);
            cuotaAagregar.MesAplicacion = new DateTime(mesSiguiente.Year, mesSiguiente.Month, 1);
            cuotaAagregar.LiquidacionConceptoID = (int)Liquidacion.Conceptos.Prestamo;
            _Context.MovEmpleadosDets.Add(cuotaAagregar);
            mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
            if (mensajeDto != null) {
                _Mensajes.Add(new Mensaje() {
                    Error = true,
                    Comentario = "#ERROR# al intentar insertar una nueva cuota: " + mensajeDto.MensajeDelProceso
                });
                return false;
            }
            _Mensajes.Add(new Mensaje() {
                Error = false,
                Comentario = "La cuota fue insertada con exito "
            });
            _NuevaCuotaAgregadaDb = cuotaAagregar;
            return true;
        }
        /// <summary>
        /// Modifica el valor del campo "Cuotas" de la tabla PrestamosSimple
        /// </summary>
        /// <returns></returns>
        private bool P5_ModificarLaCantidadCuotaDelPrestamo() {
            MensajeDto mensajeDto = null;
            var cantidadCuotasAntesDeActualizar = _CabeceraPrestamoDb.Cuotas;
            _CabeceraPrestamoDb.Cuotas += 1;
            _Context.Entry(_CabeceraPrestamoDb).State = System.Data.Entity.EntityState.Modified;

            mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
            if (mensajeDto != null) {
                _Mensajes.Add(new Mensaje() {
                    Error = true,
                    Comentario = "#ERROR# al intentar cambiar la cantidad de cuotas en la cabecera: " + " " + mensajeDto.MensajeDelProceso
                });
                return false;
            }
            _Mensajes.Add(new Mensaje() {
                Error = false,
                Comentario = "El valor de la cantidad cuotas del prestamo simple fue modificado de  " +
                cantidadCuotasAntesDeActualizar +
                " a " + _CabeceraPrestamoDb.Cuotas
            });
            return true;
        }
        #endregion
        #region CLASES
        class Mensaje {
            public bool Error { get; set; }
            public string Comentario { get; set; }
        }
        #endregion
    }
}
