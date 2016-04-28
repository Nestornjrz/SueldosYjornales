using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    /// <summary>
    /// Esta tabla no existe en la base de datos pero se utiliza para trasportar la informacion
    /// </summary>
    public class SexoDto {
        /// <summary>
        /// Masculino es igual a 1, Femenino es igual a 2
        /// </summary>
        public int SexoID { get; set; }
        public string NombreSexo { get; set; }
    }
}
