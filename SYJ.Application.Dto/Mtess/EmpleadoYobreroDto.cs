using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto.Mtess {
    public class EmpleadoYobreroDto {
        #region SOLO PARA REFERENCIA
        public long EmpleadoID { get; set; }      
        #endregion
        /// <summary>
        /// A NROPATRONAL 32 ENTERO NÚMERO PATRONAL ASIGNADO POR EL MTESS
        /// </summary>
        public int NroPatronal{ get; set; }
        /// <summary>
        /// B DOCUMENTO 15 CARACTER CI DE IDENTIDAD POLICIAL, PASAPORT, ETC
        /// </summary>
        public string Documento { get; set; }
        /// <summary>
        /// C NOMBRE 50 CARACTER NOMBRES DE PILA
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// D APELLIDO 50 CARACTER APELLIDOS
        /// </summary>
        public string Apellido { get; set; }
        /// <summary>
        /// E SEXO 1 CARACTER M PARA MÁSCULINO Y F PARA FEMENINO
        /// </summary>
        public char Sexo { get; set; }
        /// <summary>
        /// F ESTADOCIVIL 1 CARACTER S PARA SOLTERO, C PARA CASADO, D PARA DIVORCIADOS, V PARA VIUDAS.
        /// </summary>
        public char EstadoCivil { get; set; }
        /// <summary>
        /// G FECHANAC FECHA FECHA EN FORMATO YYYY-MM-DD PREFERIBLEMENTE
        /// </summary>
        public DateTime FechaNac { get; set; }
        /// <summary>
        /// H NACIONALIDAD 20 CARACTER NACIONALIDAD DEL EMPLEADO
        /// </summary>
        public string Nacionalidad { get; set; }
        /// <summary>
        /// I DOMICILIO 100 CARACTER DIRECCIÓN DEL EMPLEADO
        /// </summary>
        public string Domicilio { get; set; }
        /// <summary>
        /// J FECHANACMENOR FECHA FECHA EN FORMATO YYYY-MM-DD PREFERIBLEMENTE
        /// </summary>
        public DateTime? FechaNacMenor { get; set; }
        /// <summary>
        /// K HIJOSMENORES 32 ENTERO CANTIDAD DE HIJOS MENORES DEL EMPLEADO
        /// </summary>
        public int HijosMenores { get; set; }
        /// <summary>
        /// L CARGO 100 CARACTER OCUPACIÓN REAL DEL EMPLEADO
        /// </summary>
        public string Cargo { get; set; }
        /// <summary>
        /// M PROFESION 100 CARACTER PROFESIÓN DEL EMPLEADO
        /// </summary>
        public string Profesion { get; set; }
        /// <summary>
        /// N FECHAENTRADA FECHA FECHA EN FORMATO YYYY-MM-DD PREFERIBLEMENTE
        /// </summary>
        public DateTime? FechaEntrada { get; set; }
        /// <summary>
        /// O HORARIOTRABAJO 20 CARACTER COMPLETAR HORARIO DE TRABAJO - COMPLETAR SÓLO SI EL TRABAJADOR
        /// ES MENOR
        /// </summary>
        public string HorarioTrabajo { get; set; }
        /// <summary>
        /// P MENORESCAPA 20 CARACTER FECHA DE INICIO DE TRABAJO DEL MENOR CERTIFICADO POR LA CODENI -
        /// COMPLETAR SÓLO SI EL TRABAJADOR ES MENOR
        /// </summary>
        public string MenorEscapa { get; set; }
        /// <summary>
        /// Q MENORESESCOLAR 20 CARACTER SITUACIÓN ESCOLAR DEL MENOR
        /// </summary>
        public string MenorEsEscolar { get; set; }
        /// <summary>
        /// R FECHASALIDA FECHA FECHA EN FORMATO YYYY-MM-DD PREFERIBLEMENTE
        /// </summary>
        public DateTime? FechaSalida { get; set; }
        /// <summary>
        /// S MOTIVOSALIDA 100 CARACTER COMPLETAR OBLIGATORIAMENTE SI EXISTE FECHA DE SALIDA
        /// </summary>
        public string MotivoSalida { get; set; }
        /// <summary>
        /// T ESTADO 1 CARACTER NO COMPLETAR
        /// </summary>
        public char Estado { get; set; }
    }
}
