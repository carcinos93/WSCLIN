using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace WSCLIN.Servicios
{
    public class WSBase
    {
        /// <summary>
        /// Usuario
        /// </summary>
        public string USUARIO { get; set; }
        /// <summary>
        /// Credencial o contraseña
        /// </summary>
        public string CREDENCIAL { get; set; }
        /// <summary>
        /// Licencia
        /// </summary>
        public string LICENCIA { get; set; }
        /// <summary>
        /// Direccion MAC
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// Direccion IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// Nombre de empresa
        /// </summary>
        public string EMPRESA { get; set; }
        /// <summary>
        /// Cadena de la conexion
        /// </summary>
        public string getConn
        {
            get
            {
                return (@"server=localhost;database=" + this.EMPRESA + ";uid=" + this.USUARIO + ";password=" + CREDENCIAL + ";");
            }
        }
        /// <summary>
        /// Funcion que ejecuta un sp
        /// </summary>
        /// <param name="storeProcedure">Procedimiento almacenado</param>
        /// <param name="includeGlobal">si debe incluir las variables globales</param>
        /// <param name="parameters">lista de parametros</param>
         /// <param name="outputs">lista de variables de salida</param>
        /// <returns></returns>
        public RespuestaSP executeSP(string storeProcedure, bool includeGlobal, IList<SqlParameter> parameters,  Dictionary<string, object> outputs = null)
        {
            return executeSP(storeProcedure, includeGlobal, (parameters == null ? null : parameters.ToArray()), outputs);
        }
        /// <summary>
        /// Funcion que ejecuta un sp 
        /// </summary>
        /// <param name="storeProcedure">Procedimiento almacenado</param>
        /// <param name="includeGlobal">si debe incluir las variables globales</param>
        /// <param name="parameters">lista de parametros</param>
        /// <param name="outputs">lista de variables de salida</param>
        /// <returns></returns>
        public RespuestaSP executeSP(string storeProcedure, bool includeGlobal, SqlParameter[] parameters, Dictionary<string, object> outputs = null) {
            bool hasError = false;
            string message = "";
            string code = "0";
            try
            {
                if (!this.isValidCon())
                {
                    hasError = true;
                    message = "";
                    code = "0";
                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(this.getConn))
                    {
                        cn.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = storeProcedure;
                            if (includeGlobal) // indica si debe incluir las propiedades de la clase en la ejecucion del sp
                            {
                                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar, 100).Value = this.USUARIO;
                                cmd.Parameters.Add("@CREDENCIAL", SqlDbType.VarChar, 100).Value = this.CREDENCIAL;
                                cmd.Parameters.Add("@LICENCIA", SqlDbType.VarChar, 200).Value = this.LICENCIA;
                                cmd.Parameters.Add("@MAC", SqlDbType.VarChar, 200).Value = this.MAC;
                                cmd.Parameters.Add("@IP", SqlDbType.VarChar, 100).Value = this.IP;
                            }
                            if (parameters != null) //si hay parametros adicionales
                            {
                                if (parameters.Length > 0)
                                {
                                    cmd.Parameters.AddRange(parameters);
                                }
                            }
                            using (SqlDataReader dr = cmd.ExecuteReader()) //se ejecuta 
                            {
                                while (dr.Read())
                                {
                                    //si contiene las columnas de codigo de error y mensaje
                                    if (existenCol(dr, new string[] { "ErrorMessage", "ErrorNumber" }))
                                    {
                                        message = dr["ErrorMessage"].ToString();
                                        code = dr["ErrorNumber"].ToString();
                                        hasError = true;
                                        break;
                                    }
                                }
                                if (outputs != null) //si se ha definido un diccionario
                                {
                                    foreach (SqlParameter p in cmd.Parameters)
                                    {
                                        //si los parametros estan en output o inputouput
                                        if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                                        {
                                            outputs[p.ParameterName] = p.Value;
                                        }
                                    }
                                }
                            }
                        }
                        cn.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                hasError = true;
                code = ex.Number.ToString();
                message = ex.Message.ToString();
            }
            catch (Exception ex)
            {
                hasError = true;
                code = "96";
                message = ex.Message;
            }

            return new RespuestaSP { 
                hasException = hasError,
                respuesta = new RespuestaServicio
                {
                    Codigo = code,
                    Mensaje = message
                }
            };
        }

        /// <summary>
        /// Valida si la cadena es correcta
        /// </summary>
        /// <returns></returns>
        public bool isValidCon()
        {
            try
            {
                if (string.IsNullOrEmpty(this.EMPRESA))
                    return false;
                System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(this.getConn);
                cn.Open();
                cn.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Funcion que identifica la lista de columnas existen
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        private bool existenCol(SqlDataReader dr, string[] cols)
        {
            for (int i = 0, len = dr.FieldCount; i < len; i++)
            {
                if (!cols.Contains(dr.GetName(i)))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class RespuestaSP
    {
        public bool hasException { get; set; }
        public RespuestaServicio respuesta { get; set; }

    }
}