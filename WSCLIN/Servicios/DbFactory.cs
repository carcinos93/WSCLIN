using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using Dapper;
namespace WSCLIN.Servicios
{
    public class DbFactory
    {
        public static IDbConnection Conn()
        {
           
            var connectionString = @"server=localhost;database={0};uid={1};password={2};";
            HttpContext context = HttpContext.Current;
            var usuario = "";
            var empresa ="";
            var password = "";
            NameValueCollection parametros = null;
            if (context.Request.HttpMethod == "POST")
            {
                parametros = context.Request.Form;
            }
            else if (context.Request.HttpMethod == "GET")
            {
                parametros = context.Request.QueryString;
            }
            usuario = parametros["USUARIO"];
            password = parametros["CREDENCIAL"];
            empresa = parametros["EMPRESA"];
            var connection = new SqlConnection(
                string.Format(connectionString, empresa, usuario, password)
                );
            return connection;
        }

        public static string MensajeSistema(string codigo)
        {
            string msj = "";
            try
            {
                using (var cn = Conn())
                {
                    cn.Open();
                    msj = cn.ExecuteScalar<string>("SELECT [dbo].[MensajeAlerta](@CODIGO_MENSAJE)",
                        new { CODIGO_MENSAJE = codigo });                     
                }
                return msj;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}