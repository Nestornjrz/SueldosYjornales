using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto.Mtess {
    public class SueldoYjornaleDto {
        /// <summary>
        /// A NROPATRONAL 32 ENTERO CORRESPONDE AL NÚMERO PATRONAL DEL MINISTERIO DE TRABAJO
        /// </summary>
        public int NroPatronal { get; set; }
        /// <summary>
        /// B DOCUMENTO 15 CARACTER CORRESPONDE AL NÚMERO DE LA CÉDULA DE IDENTIDAD POLICIAL. ESTE 
        /// CAMPO SÓLO ACEPTA VALORES NUMÉRICOS.
        /// </summary>
        public string Documento { get; set; }
        /// <summary>
        /// C FORMADEPAGO 1 CARACTER LA FORMA DE PAGO ESTABLECIDA PUEDE SER J PARA JORNALES (26 DÍAS) O M PARA
        /// MENSUALES(30 DÍAS).
        /// </summary>
        public char FormaDePago { get; set; }
        /// <summary>
        /// D IMPORTEUNITARIO 32 ENTERO EL IMPORTE UNITARIO EQUIVALE AL SUELDO PERCIBIDO POR UN DÍA DE TRABAJO Y
        /// SE SERÁ EL COCIENTE ENTRE EL SALARIO Y LA FORMA DE PAGO.
        /// </summary>
        public decimal ImporteUnitario { get; set; }
        /// <summary>
        /// E H_ENE 32 ENTERO TOTAL DE HORAS TRABAJADAS EN ENERO, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_Ene { get; set; }
        /// <summary>
        /// F S_ENE 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN ENERO, SIN CONTAR 
        /// REMUNERACIÓN POR HORAS EXTRAS.
        /// </summary>
        public decimal S_Ene { get; set; }
        /// <summary>
        /// G H_FEB 32 ENTERO TOTAL DE HORAS TRABAJADAS EN FEBRERO, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_Feb { get; set; }
        /// <summary>
        /// H S_FEB 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN FEBRERO, SIN CONTAR
        /// REMUNERACIÓN POR HORAS EXTRAS.
        /// </summary>
        public decimal S_Feb { get; set; }
        /// <summary>
        /// I H_MAR 32 ENTERO TOTAL DE HORAS TRABAJADAS EN MARZO, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_Mar { get; set; }
        /// <summary>
        /// J S_MAR 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN MARZO, SIN CONTAR 
        /// REMUNERACIÓN POR HORAS EXTRAS.
        /// </summary>
        public decimal S_Mar { get; set; }
        /// <summary>
        /// K H_ABR 32 ENTERO TOTAL DE HORAS TRABAJADAS EN ABRIL, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_Abr { get; set; }
        /// <summary>
        /// L S_ABR 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN ABRIL, SIN CONTAR REMUNERACIÓN
        /// POR HORAS EXTRAS.
        /// </summary>
        public decimal S_Abr { get; set; }
        /// <summary>
        /// M H_MAY 32 ENTERO TOTAL DE HORAS TRABAJADAS EN MAYO, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_May { get; set; }
        /// <summary>
        /// N S_MAY 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN MAYO, SIN CONTAR 
        /// REMUNERACIÓN POR HORAS EXTRAS.
        /// </summary>
        public decimal S_May { get; set; }
        /// <summary>
        /// O H_JUN 32 ENTERO TOTAL DE HORAS TRABAJADAS EN JUNIO, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_Jun { get; set; }
        /// <summary>
        /// P S_JUN 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN JUNIO, SIN CONTAR 
        /// REMUNERACIÓN POR HORAS EXTRAS
        /// </summary>
        public decimal S_Jun { get; set; }
        /// <summary>
        /// Q H_JUL 32 ENTERO TOTAL DE HORAS TRABAJADAS EN JULIO, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_Jul { get; set; }
        /// <summary>
        /// R S_JUL 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN JULIO, SIN 
        /// CONTAR REMUNERACIÓN POR HORAS EXTRAS
        /// </summary>
        public decimal S_Jul { get; set; }
        /// <summary>
        /// S H_AGO 32 ENTERO TOTAL DE HORAS TRABAJADAS EN AGOSTO, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_Ago { get; set; }
        /// <summary>
        /// T S_AGO 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN AGOSTO, SIN CONTAR 
        /// REMUNERACIÓN POR HORAS EXTRAS
        /// </summary>
        public decimal S_Ago { get; set; }
        /// <summary>
        /// U H_SET 32 ENTERO TOTAL DE HORAS TRABAJADAS EN SETIEMBRE, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_Set { get; set; }
        /// <summary>
        /// V S_SET 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN SETIEMBRE, SIN CONTAR 
        /// REMUNERACIÓN POR HORAS EXTRAS.
        /// </summary>
        public decimal S_Set { get; set; }
        /// <summary>
        /// W H_OCT 32 ENTERO TOTAL DE HORAS TRABAJADAS EN OCTUBRE, SIN CONTAR LAS HORAS EXTRAS        /// </summary>
        public int H_Oct { get; set; }
        /// <summary>
        /// X S_OCT 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN OCTUBRE, SIN CONTAR 
        /// REMUNERACIÓN POR HORAS EXTRAS.
        /// </summary>
        public decimal S_Oct { get; set; }
        /// <summary>
        /// Y H_NOV 32 ENTERO TOTAL DE HORAS TRABAJADAS EN NOVIEMBRE, SIN CONTAR LAS HORAS EXTRAS
        /// </summary>
        public int H_Nov { get; set; }
        /// <summary>
        /// Z S_NOV 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN NOVIEMBRE, SIN CONTAR REMUNERACIÓN 
        /// POR HORAS EXTRAS.
        /// </summary>
        public decimal S_Nov { get; set; }
        /// <summary>
        /// AA H_DIC 32 ENTERO TOTAL DE HORAS TRABAJADAS EN DICIEMBRE, SIN CONTAR LAS HORAS EXTRAS        /// </summary>
        public int H_Dic { get; set; }
        /// <summary>
        /// AB S_DIC 32 ENTERO TOTAL DE SALARIO PERCIBIDO EN DICIEMBRE, SIN CONTAR 
        /// REMUNERACIÓN POR HORAS EXTRAS.
        /// </summary>
        public decimal S_Dic { get; set; }
        /// <summary>
        /// AC H_50 32 ENTERO HORAS EXTRAS AL 50% TRABAJADAS DURANTE EL AÑO        /// </summary>
        public int H_50 { get; set; }
        /// <summary>
        /// AD S_50 32 ENTERO IMPORTE RECIBIDO EN CONCEPTO DE HORAS EXTRAS AL 50% 
        /// TRABAJADAS DURANTE EL AÑO
        /// </summary>
        public decimal S_50 { get; set; }
        /// <summary>
        /// AE H_100 32 ENTERO HORAS EXTRAS AL 100% TRABAJADAS DURANTE EL AÑO
        /// </summary>
        public int H_100 { get; set; }
        /// <summary>
        /// AF S_100 32 ENTERO IMPORTE RECIBIDO EN CONCEPTO DE HORAS EXTRAS AL 100% 
        /// TRABAJADAS DURANTE EL AÑO
        /// </summary>
        public decimal S_100 { get; set; }
        /// <summary>
        /// AG AGUINALDO 32 ENTERO AGUINALDO O AGUINALDO PROPORCIONAL 
        /// (SEGÚN ART. 243 Y 244 DEL CÓDIGO LABORAL)
        /// </summary>
        public decimal Aguinaldo { get; set; }
        /// <summary>
        /// AH BENEFICIOS 32 ENTERO CORRESPONDE A BENEFICIOS RECIBIDOS DURANTE EL 
        /// AÑO Y EN CONCEPTO DE LIQUIDACIONES POR DESPIDO MÁS PRE-AVISO.
        /// </summary>
        public decimal Beneficios { get; set; }
        /// <summary>
        /// AI BONIFICACIONES 32 ENTERO CORRESPONDE A LAS BONIFICACIONES FAMILIARES RECIBIDAS DURANTE EL AÑO.
        /// </summary>
        public decimal Bonificaciones { get; set; }
        /// <summary>
        /// AJ VACACIONES 32 ENTERO CORRESPONDE AL IMPORTE RECIBIDO EN CONCEPTO DE VACACIONES
        /// </summary>
        public decimal Vacaciones { get; set; }
        /// <summary>
        /// AK TOTAL_H 32 ENTERO CORRESPONDE AL TOTAL DE HORAS TRABAJADAS INCLUYENDO HORAS EXTRAS.
        /// </summary>
        public int Total_H { get; set; }
        /// <summary>
        /// AL TOTAL_S 32 ENTERO TOTAL DE IMPORTE PERCIBIDO EN CONCEPTO DE SALARIO
        /// </summary>
        public decimal Total_S { get; set; }
        /// <summary>
        /// AM TOTALGENERAL 32 ENTERO TOTAL PERCIBIDO INCLUYENDO AGUINALDO, BENEFICIOS, BONIFICACIONES Y VACACIONES.
        /// </summary>
        public decimal TotalGeneral { get; set; }
    }
}
