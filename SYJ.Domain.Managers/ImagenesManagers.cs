using SYJ.Application.Dto;
using SYJ.Domain.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SYJ.Domain.Managers {
    public class ImagenesManagers {
        public MensajeDto guardarImagen(long empleadoID, 
            int tipoImagenID, 
            byte[] imagen,         
            string nombreArchivo, 
            Guid userID) {
            long usuarioIDCarga = 0; 
            using (var context = new SueldosJornalesEntities()) {
                //Se carga el usuario
                var usuarioDb = context.Usuarios
                    .Where(u => u.UserID == userID).FirstOrDefault();
                if (usuarioDb == null) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "Error: Este usuario esta registrado pero no fue aceptado por la administracion aun"
                    };
                }
                usuarioIDCarga = usuarioDb.UsuarioID;
            }

            var mensajeConnString = RecuperarElconnectionStrings("SueldoJornalesDb");
            if (mensajeConnString.Error) {
                return mensajeConnString;
            }
            string CadConexion = mensajeConnString.Valor;

            using (SqlConnection conexionBD = new SqlConnection(CadConexion)) {
                try {
                    conexionBD.Open();
                    if (conexionBD.State == ConnectionState.Open) {
                        using (SqlTransaction transaccion = conexionBD.BeginTransaction()) {
                            //byte[] contenido = File.ReadAllBytes(ubicacion);
                            //Se ve su extencion
                            string[] nomArchi = nombreArchivo.Split('.');
                            var extension = nomArchi[nomArchi.Length - 1];

                            string cadSql = @"INSERT INTO [Sj].[Imagenes]
                            (EmpleadoID,
                             TipoImagenID,
                             Imagen,
                             Extencion,
                             UsuarioID, 
                             MomentoCarga) 
                               VALUES
                            (@EmpleadoID,
                             @TipoImagenID,
                             @Imagen,
                             @Extencion,
                             @UsuarioID, 
                             GETDATE())";
                            using (SqlCommand cmd = new SqlCommand(cadSql, conexionBD, transaccion)) {
                                cmd.Parameters.AddWithValue("@EmpleadoID", empleadoID);
                                cmd.Parameters.AddWithValue("@TipoImagenID", tipoImagenID);
                                cmd.Parameters.AddWithValue("@Imagen", imagen);
                                cmd.Parameters.AddWithValue("@Extencion", extension);
                                cmd.Parameters.AddWithValue("@UsuarioID", usuarioIDCarga);
                                cmd.ExecuteNonQuery();
                                transaccion.Commit();
                            }
                        }
                    }
                } catch (Exception ex) {
                    return new MensajeDto() {
                        Error = true,
                        MensajeDelProceso = "Error: " + ex.Message
                    };
                } finally {
                    if ((conexionBD != null) && (conexionBD.State == ConnectionState.Open)) {
                        conexionBD.Close();
                    }
                }
            }
            return new MensajeDto() {
                Error = false,
                MensajeDelProceso = "Insercion Exitosa del archivo: " + nombreArchivo
            };
        }
        #region Auxiliares
        private MensajeDto RecuperarElconnectionStrings(string nombreConeccion) {
            Configuration rootWebConfig;
            rootWebConfig = WebConfigurationManager.OpenWebConfiguration("~/");
            System.Configuration.ConnectionStringSettings connString;
            if (rootWebConfig.AppSettings.Settings.Count > 0) {
                connString = rootWebConfig.ConnectionStrings.ConnectionStrings[nombreConeccion];
                return new MensajeDto() {
                    Error = false,
                    MensajeDelProceso = "connectionString encontrado: " + nombreConeccion,
                    Valor = connString.ToString()
                };
            }
            return new MensajeDto() {
                Error = true,
                MensajeDelProceso = "No se encuentra un nombre de coneccion igual a " + nombreConeccion
            };
        }
        #endregion
    }
}
