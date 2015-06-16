using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Application.Dto {
    public class MensajeDto {
        public bool Error { get; set; }
        public string MensajeDelProceso { get; set; }
        public string Valor { get; set; }
        public object ObjetoDto { get; set; }
    }
}
