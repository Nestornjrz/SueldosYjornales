using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto.Mtess {
    public class ResumenGeneralDto {
        /// <summary>
        /// A NROPATRONAL 32 ENTERO NÚMERO PATRONAL DEL MTESS
        /// </summary>
        public int NroPatronal { get; set; }
        /// <summary>
        /// B ANHO 32 ENTERO AQUI SE COLOCA EL PERIODO DE PRESENTACIÓN
        /// </summary>
        public int Anho { get; set; }
        /// <summary>
        /// C SUPJEFESVARONES 32 ENTERO SUPERVISORES O JEFES VARONES
        /// </summary>
        public int SupJefesVarones { get; set; }
        /// <summary>
        /// D SUPJEFESMUJERES 32 ENTERO SUPERVISORES O JEFES MUJERES
        /// </summary>
        public int SupJefesMujeres { get; set; }
        /// <summary>
        /// E EMPLEADOSVARONES 32 ENTERO EMPLEADOS VARONES
        /// </summary>
        public int EmpleadosVarones { get; set; }
        /// <summary>
        /// F EMPLEADOSMUJERES 32 ENTERO EMPLEADAS MUJERES
        /// </summary>
        public int EmpleadosMujeres { get; set; }
        /// <summary>
        /// G OBREROSVARONES 32 ENTERO OBREROS HOMBRES
        /// </summary>
        public int ObrerosVarones { get; set; }
        /// <summary>
        /// H OBREROSMUJERES 32 ENTERO OBRERAS MUJERES
        /// </summary>
        public int ObrerosMujeres { get; set; }
        /// <summary>
        /// I MENORESVARONES 32 ENTERO MENORES VARONES
        /// </summary>
        public int MenoresVarones { get; set; }
        /// <summary>
        /// J MENORESMUJERES 32 ENTERO MENORES MUJERES
        /// </summary>
        public int MenoresMujeres { get; set; }
        /// <summary>
        /// K* ORDEN 32 ENTERO DESCRIPCIÓN DE LAS FILAS. NUMERACIÓN OBLIGATORIA DEL 1 AL 5.
        /// </summary>
        public int Orden { get; set; }
    }
}
