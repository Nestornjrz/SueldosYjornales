using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SueldosYjornales.Controllers {
    public static class DatosInicio {
        public static string RecuperarValor(string clave) {
            Configuration rootWebConfig;
            string valor = "Configuracion no encontrada";
            rootWebConfig = WebConfigurationManager.OpenWebConfiguration("~/");
            if (rootWebConfig.AppSettings.Settings.Count > 0) {
                KeyValueConfigurationElement customSetting = rootWebConfig.AppSettings.Settings[clave];
                if (null != customSetting) {
                    valor = customSetting.Value;
                }
            }
            return valor;
        }
    }
}