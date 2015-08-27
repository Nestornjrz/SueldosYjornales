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

        private bool YaSeGeneroLiquidacionParaEmpleadoSn(int empleadoID) {
            using (var context = new SueldosJornalesEntities()) {
                var mesAplicacion = new DateTime(_FlDto.Year, _FlDto.Mes.MesID, 1);
                var movi = context.MovEmpleadosDets
                    .Where(m => m.DevCred == DevCred.Devito &&
                    m.LiquidacionConceptoID == (int)LiquidacionConceptos.TotalPagado &&
                    m.MesAplicacion == mesAplicacion &&
                    m.EmpleadoID  == empleadoID);
                if (movi.Count() > 0) {
                    var empleado = context.Empleados
                       .Where(e => e.EmpleadoID == empleadoID)
                       .Select(s => new EmpleadoDto() {
                           EmpleadoID = s.EmpleadoID,
                           Nombres = s.Nombres,
                           Apellidos = s.Apellidos
                       }).First();
                    var nombre = "(" + empleado.Nombres + " " + empleado.Apellidos + ") ";
                    _Mensajes.Add(nombre +
                        " ya fue generado su liquidacion mes " +
                        _FlDto.Mes.MesID + "/" + _FlDto.Year +
                        " Liquidacion numero: " + movi.First().MovEmpleadoID
                        );
                    return true;
                }
                return false;
            }
        }

        private void GenerarLiquidacionEmpleado(int empleadoID) {
            DatosEmpleado de = new DatosEmpleado();
            using (var context = new SueldosJornalesEntities()) {
                de.Empleado = context.Empleados.Where(e => e.EmpleadoID == empleadoID)
                    .Select(s => new EmpleadoDto() {
                        EmpleadoID = s.EmpleadoID,
                        Nombres = s.Nombres,
                        Apellidos = s.Apellidos
                    }).First();
                ///Se marcan primero los prestamos pues esta marca genera los devitos
                ///por prestamos en la tabla MovEmpleados/Dets que luego se va a consultar
                ///mas adelante en este mismo metodo
                MarcarPrestamos(de, context);
            }
            using (var context = new SueldosJornalesEntities()) {
                _Context = context;
                using (var dbContextTransaction = context.Database.BeginTransaction()) {
                    _DbContexTransaction = dbContextTransaction;
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
            movEmpleado.Descripcion = "Liquidacion de salario de " + nombre;
            movEmpleado.UsuarioID = _Context.Usuarios.Where(u => u.UserID == _UserID).First().UsuarioID;
            movEmpleado.MomentoCarga = DateTime.Now;

            _Context.MovEmpleados.Add(movEmpleado);
            mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
            if (mensajeDto != null) {
                _Mensajes.Add("#ERROR# en la generacion de la cabecera del movimiento para " + nombre + " " + mensajeDto.MensajeDelProceso);
                _CantidadErrores += 1;
                _DbContexTransaction.Rollback();
                return;
            }
            _Mensajes.Add("Se cargo correctamente la cabecera de movimiento para " + nombre);

            //SE CARGAN LOS DETALLES (Tabla MovEmpleadosDets) ---------------------------------
            ///Se carga el Sueldo Base
            var movEmpleadoDet = new MovEmpleadosDet();
            CargarDatosComunes(de, movEmpleado, movEmpleadoDet);
            movEmpleadoDet.LiquidacionConceptoID = (int)LiquidacionConceptos.SueldoBase;
            movEmpleadoDet.DevCred = DevCred.Credito;
            movEmpleadoDet.Monto = de.SueldoBase;
            if (!SeCargoMovEmpleadosDetsSn(movEmpleadoDet, mensajeDto, de, "Sueldo Base")) { return; };
            _Mensajes.Add("Se Cargo correctamente el Sueldo Base de " + nombre);

            ///Carga de comisiones
            movEmpleadoDet = new MovEmpleadosDet();
            CargarDatosComunes(de, movEmpleado, movEmpleadoDet);
            movEmpleadoDet.LiquidacionConceptoID = (int)LiquidacionConceptos.Comision;
            movEmpleadoDet.DevCred = DevCred.Credito;
            movEmpleadoDet.Monto = de.Comisiones.Sum();
            if (!SeCargoMovEmpleadosDetsSn(movEmpleadoDet, mensajeDto, de, "Comisiones")) { return; };
            _Mensajes.Add("Se Cargo correctamente las comisiones de " + nombre);

            ///Carga de anticipos
            movEmpleadoDet = new MovEmpleadosDet();
            CargarDatosComunes(de, movEmpleado, movEmpleadoDet);
            movEmpleadoDet.LiquidacionConceptoID = (int)LiquidacionConceptos.Anticipo;
            movEmpleadoDet.DevCred = DevCred.Devito;
            movEmpleadoDet.Monto = de.Anticipos.Sum();
            if (!SeCargoMovEmpleadosDetsSn(movEmpleadoDet, mensajeDto, de, "Anticipos")) { return; };
            _Mensajes.Add("Se Cargo correctamente los anticipos de " + nombre);

            ///Prestamos
            /// LOS PRESTAMOS YA ESTAN CARGADOS POR MEDIO DE TRIGGERS DISPARADOS DESDE LA TABLA PrestamosSimples           

            ///Se carga el TOTAL PAGADO
            /// Se recupera el prestamo del mesAplicacion
            var mesAplicacion = new DateTime(_FlDto.Year, _FlDto.Mes.MesID, 1);
            var devCred = DevCred.Devito;
            int liquidacionConcepto = (int)LiquidacionConceptos.Prestamo;
            decimal prestamos = 0;
            var prestamosDb = _Context.MovEmpleadosDets
                .Where(md => md.MesAplicacion == mesAplicacion &&
                       md.LiquidacionConceptoID == liquidacionConcepto &&
                       md.DevCred == devCred);
            if (prestamosDb.Count() > 0) {
                prestamos = prestamosDb.Sum(s => s.Monto);
            }

            var totalCredito = de.SueldoBase + de.Comisiones.Sum();
            var totalDevito = de.Anticipos.Sum() + prestamos;

            var totalPagado = totalCredito - totalDevito;

            movEmpleadoDet = new MovEmpleadosDet();
            CargarDatosComunes(de, movEmpleado, movEmpleadoDet);
            movEmpleadoDet.LiquidacionConceptoID = (int)LiquidacionConceptos.TotalPagado;
            movEmpleadoDet.DevCred = DevCred.Devito;
            movEmpleadoDet.Monto = totalPagado;
            if (!SeCargoMovEmpleadosDetsSn(movEmpleadoDet, mensajeDto, de, "Total Pagado")) { return; };
            _Mensajes.Add("Se Cargo correctamente el total pagado de " + nombre);
        }

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

        private void MarcarPrestamos(DatosEmpleado de, SueldosJornalesEntities context) {
            var nombre = "(" + de.Empleado.Nombres + " " + de.Empleado.Apellidos + ") ";
            var prestamosSimples = context.PrestamosSimples
                .Where(p => p.EmpleadoID == de.Empleado.EmpleadoID)
                .Select(s => new PrestamoSimpleDto() {
                    PrestamoSimpleID = s.PrestamoSimpleID,
                    EmpleadoID = s.EmpleadoID,
                    Cuotas = s.Cuotas,
                    Fecha1erVencimiento = s.Fecha1erVencimiento,
                    Monto = s.Monto,
                    Observacion = s.Observacion
                }).ToList();

            var resul = new List<decimal>();
            foreach (var item in prestamosSimples) {
                var cantidad = context.PrestamosSimples
                    .Where(p => p.PrestamoSimpleID == item.PrestamoSimpleID)
                    .Count();
                if (cantidad > 0) {
                    continue;
                }

                //Se marca el prestamo para que genere los devitos
                //No hace falta cargarlos pues este proceso dispara triggers y estos cargan
                //los devitos de los prestamos
                MensajeDto mensajeDto = null;
                var prestamoSimple = context.PrestamosSimples
                    .Where(p => p.PrestamoSimpleID == item.PrestamoSimpleID).First();

                prestamoSimple.GenerarDevitoSn = true;
                context.Entry(prestamoSimple).State = System.Data.Entity.EntityState.Modified;

                mensajeDto = AgregarModificar.Hacer(context, mensajeDto);
                if (mensajeDto != null) {
                    _Mensajes.Add("#ERROR# en la marca del prestamo simple de " + nombre + " " + mensajeDto.MensajeDelProceso);
                    return;
                }
            }
            _Mensajes.Add(nombre + "Devitos de prestamos generadors correctamente");
        }

        private List<decimal> RecuperarAnticipos(DatosEmpleado de) {
            var nombre = "(" + de.Empleado.Nombres + " " + de.Empleado.Apellidos + ") ";
            var anticipos = _Context.Anticipos
                .Where(a => a.EmpleadoID == de.Empleado.EmpleadoID)
                .Select(s => new AnticipoDto() {
                    AnticipoID = s.AnticipoID,
                    EmpleadoID = s.EmpleadoID,
                    FechaAnticipo = s.FechaAnticipo,
                    MontoAnticipo = s.MontoAnticipo,
                    Observacion = s.Observacion
                }).ToList();
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
                }
                resul.Add(item.MontoAnticipo);
            }
            _Mensajes.Add(nombre + "Devitos de anticipos generadors correctamente");
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
            //Se marca las comisiones
            List<decimal> resul = new List<decimal>();
            foreach (ComisioneDto item in listado) {
                MensajeDto mensajeDto = null;
                var comisionDb = _Context.Comisiones
                    .Where(c => c.ComisionID == item.ComisionID).First();
                comisionDb.CreditosGeneradoSn = true;

                _Context.Entry(comisionDb).State = System.Data.Entity.EntityState.Modified;
                mensajeDto = AgregarModificar.Hacer(_Context, mensajeDto);
                if (mensajeDto != null) {
                    _Mensajes.Add("#ERROR# en la marca en la comision de " + nombre + " " + mensajeDto.MensajeDelProceso);
                    _DbContexTransaction.Rollback();
                    return resul;
                }
                resul.Add(item.MontoComision);
            }
            _Mensajes.Add(nombre + "Creditos de Comisiones generadors correctamente");
            return resul;
        }

        private decimal RecuperarSueldo(DatosEmpleado de) {
            var nombre = "(" + de.Empleado.Nombres + " " + de.Empleado.Apellidos + ") ";

            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();
            MensajeDto mensaje = hsm.SalarioActual(de.Empleado.EmpleadoID);
            if (mensaje.Error) {
                _Mensajes.Add(nombre + mensaje.MensajeDelProceso);
                _CantidadErrores += 1;
                return 0;
            }
            _Mensajes.Add(nombre + mensaje.MensajeDelProceso);
            return decimal.Parse(mensaje.Valor);
        }

        class DatosEmpleado {
            public EmpleadoDto Empleado { get; set; }
            public decimal SueldoBase { get; set; }
            public List<decimal> Comisiones { get; set; }
            public List<decimal> Anticipos { get; set; }
            ///Los prestamos ya se generan automanticamente mediante un trigger
            /// en la tabla de PrestamosSimples
            //public List<decimal> Prestamos { get; set; }
        }
        static class DevCred {
            public static bool Devito = true;
            public static bool Credito = false;
        }
        enum LiquidacionConceptos {
            SueldoBase = 1,
            Comision = 2,
            Anticipo = 3,
            Prestamo = 4,
            TotalPagado = 5
        }
    }
}
