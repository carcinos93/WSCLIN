using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Reflection;
using Newtonsoft;
using Newtonsoft.Json;
using WSCLIN.Servicios;
using Dapper;
using log4net;
namespace WSCLIN
{
    /// <summary>
    /// Summary description for ServiciosCLIN
    /// </summary>
    [WebService(Namespace = "CLINWS")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiciosCLIN : System.Web.Services.WebService
    {

        private static ILog log = LogManager.GetLogger(typeof(ServiciosCLIN));
        String var_cadenaconexion = (@"serverlocalhost;database=clinerp;uid=sa;password=sa;");
        SqlConnection var_conexion = new SqlConnection();
        SqlCommand var_comando = new SqlCommand();
        SqlDataAdapter var_adaptador = new SqlDataAdapter();

        private void abrirconexion()
        {
            var_conexion.ConnectionString = var_cadenaconexion;
            if (var_conexion.State == ConnectionState.Closed)
                var_conexion.Open();
        }
        private void cerrarconexion()
        {
            if (var_conexion.State == ConnectionState.Closed)
                var_conexion.Close();
        }

        
        [WebMethod]
        public ColectorServicio Colector(
            string empresa,
            string usuario,
            string clave,
            string referencia,
            string credito,
            string cuenta_banco,
            string sucursal,
            DateTime fecha,
            decimal  monto_efectivo,
            decimal monto_otros,
            string anulado,
            string nombre_cliente
        )
        {
            DataSet var_resultado = new DataSet();
            try
            {
                var_cadenaconexion = (@"server=localhost;database=" + empresa + ";uid=" + usuario + ";password=" + clave + ";");
                abrirconexion();
                var_comando.CommandText = "[dbo].[WS_COLECTOR]";
                var_comando.CommandType = CommandType.StoredProcedure;
                var_comando.Connection = var_conexion;
                var_comando.Parameters.Add("@codigo_usuario", SqlDbType.VarChar, 100).Value = usuario;
                var_comando.Parameters.Add("@clave_usuario", SqlDbType.VarChar, 100).Value = clave;
                var_comando.Parameters.Add("@referencia", SqlDbType.VarChar, 50).Value = referencia;
                var_comando.Parameters.Add("@credito", SqlDbType.VarChar, 40).Value = credito;
                var_comando.Parameters.Add("@cuenta_banco", SqlDbType.VarChar, 100).Value = cuenta_banco;
                var_comando.Parameters.Add("@sucursal", SqlDbType.VarChar, 150).Value = sucursal;
                var_comando.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                var_comando.Parameters.Add("@monto_efectivo", SqlDbType.Decimal).Value = monto_efectivo;
                var_comando.Parameters.Add("@monto_otros", SqlDbType.Decimal).Value = monto_otros;
                var_comando.Parameters.Add("@anulado", SqlDbType.Char, 1).Value = anulado;
                var_comando.Parameters.Add("@nombre_cliente", SqlDbType.VarChar, 200).Value = nombre_cliente;
                //var_adaptador.SelectCommand = var_comando;
                //var_adaptador.Fill(var_resultado, "consulta");
                using (SqlDataReader dr = var_comando.ExecuteReader())
                {
                    while (dr.Read())
                    {
                       
                         if (existenCol(dr, new string[] { "ErrorMessage", "ErrorNumber"})) {

                             var c = new ColectorServicio
                             {
                             Mensaje = valorCol(dr, "ErrorMessage").ToString(),
                             Codigo = valorCol(dr, "ErrorNumber").ToString()
                            };
                             log.Info(JsonConvert.SerializeObject(c));
                             return c;

                         }
                    }
                }
                var_comando = null;
                cerrarconexion();
            }
            catch (Exception ex)
            {
                //throw ex;
                log.Info("Error en colector", ex);
                return new ColectorServicio {  Mensaje =  MensajeSistema("96"), Codigo =  "96"};
            }

            return new ColectorServicio {  Mensaje = MensajeSistema("100"), Codigo = "100" };
        }

        [WebMethod]
        public RespuestaServicio CrearCliente(
            string USUARIO,
            string CREDENCIAL,
            string LICENCIA,
            string MAC,
            string IP,
            string Empresa,
            string CODIGO_CLIENTE,
            string PRIMER_NOMBRE,
            string APELLIDO1,
            string TELEFONO1,
            string CORREO1,
            string TIPO_PERSONA,
            string TIPO_CLIENTE,
            string TIPO_SOCIEDAD,
            string CATEGORIA_CLIENTE,
            string DOCUMENTO_UNICO,
            string REGISTRO_FISCAL,
            string CODIGO_SECTOR,
            string GIRO,
            string TIPO_NIT,
            string NUMERO_NIT,
            string PASAPORTE,
            string CARNET_RESIDENTE,
            string SEGURO_SOCIAL,
            string CODIGO_ALTERNO,
            string SEGUNDO_NOMBRE,
            string TERCER_NOMBRE,
            string APELLIDO2,
            string APELLIDO3,
            string ALIAS,
            string TELEFONO2,
            string MOVIL1,
            string MOVIL2,
            string TELEFONO_CONTACTO,
            string CORREO2,
            string LUGAR_NACIMIENTO,
            string CONTACTO,
            string CARGO,
            string TELEFONO3,
           string GENERO,
           string ESTADO_FAMILIAR,
           string CODIGO_VENDEDOR,
           string CODIGO_COBRADOR,
           string DIRECCION,
           string TIPO_VIVIENDA,
           string CODIGO_SUCURSAL,
           string NIVEL_ACADEMICO,
           DateTime @FECHA_NACIMIENTO,
           DateTime FECHA_INGRESO,
           string CODIGO_ACTIVIDAD,
            string CODIGO_TITULO,
            string CODIGO_PROFESION,
            string CODIGO_INDUSTRIA,
            string PAIS_NACIMIENTO,
            string PAIS_DIRECCION,
            string DIVISION_GEOGRAFICA1,
            string DIVISION_GEOGRAFICA2,
            string DIVISION_GEOGRAFICA3,
            string PROMEDIO_INGRESOS,
            string PROMEDIO_EGRESOS
       )
        {
            /* DataSet var_resultado = new DataSet();*/
            int var_resultado = 0;



            try
            {
                var_cadenaconexion = (@"server=localhost;database=" + Empresa + ";uid=" + USUARIO + ";password=" + CREDENCIAL + ";");
                /* var_cadenaconexion = (@"server=10.10.0.17;database=clinerp;uid=jgiron;password=P@ssw0rd;");*/
                abrirconexion();
                var_comando.CommandText = "[dbo].[WSCliente]";
                var_comando.CommandType = CommandType.StoredProcedure;
                var_comando.Connection = var_conexion;
                var_comando.Parameters.Add("@USUARIO", SqlDbType.VarChar, 100).Value = USUARIO;
                var_comando.Parameters.Add("@CREDENCIAL", SqlDbType.VarChar, 100).Value = CREDENCIAL;
                var_comando.Parameters.Add("@LICENCIA", SqlDbType.VarChar, 200).Value = LICENCIA;
                var_comando.Parameters.Add("@MAC", SqlDbType.VarChar, 200).Value = MAC;
                var_comando.Parameters.Add("@IP", SqlDbType.VarChar, 100).Value = IP;
                var_comando.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar, 40).Value = CODIGO_CLIENTE;
                var_comando.Parameters.Add("@PRIMER_NOMBRE", SqlDbType.VarChar, 50).Value = PRIMER_NOMBRE;
                var_comando.Parameters.Add("@APELLIDO1", SqlDbType.VarChar, 50).Value = APELLIDO1;
                var_comando.Parameters.Add("@TELEFONO1", SqlDbType.VarChar, 50).Value = TELEFONO1;
                var_comando.Parameters.Add("@CORREO1", SqlDbType.VarChar, 150).Value = CORREO1;
                var_comando.Parameters.Add("@FECHA_NACIMIENTO", SqlDbType.DateTime).Value = FECHA_NACIMIENTO;
                var_comando.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = FECHA_INGRESO;

                var_comando.Parameters.Add("@CODIGO_ACTIVIDAD", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(CODIGO_ACTIVIDAD) ? "ND" : CODIGO_ACTIVIDAD;
                var_comando.Parameters.Add("@CODIGO_TITULO", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(CODIGO_TITULO) ? "ND" : CODIGO_TITULO;
                var_comando.Parameters.Add("@CODIGO_PROFESION", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(CODIGO_PROFESION) ? "ND" : CODIGO_PROFESION;
                var_comando.Parameters.Add("@CODIGO_INDUSTRIA", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(CODIGO_INDUSTRIA) ? "ND" : CODIGO_INDUSTRIA;
                var_comando.Parameters.Add("@PAIS_NACIMIENTO", SqlDbType.VarChar, 8).Value = string.IsNullOrEmpty(PAIS_NACIMIENTO) ? "ND" : PAIS_NACIMIENTO;
                var_comando.Parameters.Add("@PAIS_DIRECCION ", SqlDbType.VarChar, 8).Value = string.IsNullOrEmpty(PAIS_DIRECCION) ? "ND" : PAIS_DIRECCION;
                var_comando.Parameters.Add("@DIVISION_GEOGRAFICA1", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(DIVISION_GEOGRAFICA1) ? "ND" : DIVISION_GEOGRAFICA1;
                var_comando.Parameters.Add("@DIVISION_GEOGRAFICA2", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(DIVISION_GEOGRAFICA2) ? "ND" : DIVISION_GEOGRAFICA2;
                var_comando.Parameters.Add("@DIVISION_GEOGRAFICA3", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(DIVISION_GEOGRAFICA3) ? "ND" : DIVISION_GEOGRAFICA3;



                var_comando.Parameters.Add("@TIPO_PERSONA", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(TIPO_PERSONA) ? "ND" : TIPO_PERSONA;
                var_comando.Parameters.Add("@TIPO_CLIENTE", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(TIPO_CLIENTE) ? "ND" : TIPO_CLIENTE;
                var_comando.Parameters.Add("@TIPO_SOCIEDAD", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(TIPO_SOCIEDAD) ? "ND" : TIPO_SOCIEDAD;
                var_comando.Parameters.Add("@CATEGORIA_CLIENTE", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(CATEGORIA_CLIENTE) ? "ND" : CATEGORIA_CLIENTE;
                var_comando.Parameters.Add("@DOCUMENTO_UNICO", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(DOCUMENTO_UNICO) ? "" : DOCUMENTO_UNICO;
                var_comando.Parameters.Add("@REGISTRO_FISCAL", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(REGISTRO_FISCAL) ? "" : REGISTRO_FISCAL;
                var_comando.Parameters.Add("@CODIGO_SECTOR", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(CODIGO_SECTOR) ? "" : CODIGO_SECTOR;
                var_comando.Parameters.Add("@GIRO", SqlDbType.VarChar, 250).Value = string.IsNullOrEmpty(GIRO) ? "" : GIRO;
                var_comando.Parameters.Add("@TIPO_NIT", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(TIPO_NIT) ? "ND" : TIPO_NIT;
                var_comando.Parameters.Add("@NUMERO_NIT", SqlDbType.VarChar, 80).Value = string.IsNullOrEmpty(NUMERO_NIT) ? "" : NUMERO_NIT;
                var_comando.Parameters.Add("@PASAPORTE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(PASAPORTE) ? "" : PASAPORTE;
                var_comando.Parameters.Add("@CARNET_RESIDENTE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(CARNET_RESIDENTE) ? "" : CARNET_RESIDENTE;
                var_comando.Parameters.Add("@SEGURO_SOCIAL", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(SEGURO_SOCIAL) ? "" : SEGURO_SOCIAL;
                var_comando.Parameters.Add("@CODIGO_ALTERNO", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(CODIGO_ALTERNO) ? "" : CODIGO_ALTERNO;
                var_comando.Parameters.Add("@SEGUNDO_NOMBRE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(SEGUNDO_NOMBRE) ? "" : SEGUNDO_NOMBRE;
                var_comando.Parameters.Add("@TERCER_NOMBRE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(TERCER_NOMBRE) ? "" : TERCER_NOMBRE;
                var_comando.Parameters.Add("@APELLIDO2", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(APELLIDO2) ? "" : APELLIDO2;
                var_comando.Parameters.Add("@APELLIDO3", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(APELLIDO3) ? "" : APELLIDO3;
                var_comando.Parameters.Add("@ALIAS", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(ALIAS) ? "" : ALIAS;
                var_comando.Parameters.Add("@TELEFONO2", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(TELEFONO2) ? "" : TELEFONO2;
                var_comando.Parameters.Add("@MOVIL1", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(MOVIL1) ? "" : MOVIL1;
                var_comando.Parameters.Add("@MOVIL2", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(MOVIL2) ? "" : MOVIL2;
                var_comando.Parameters.Add("@TELEFONO_CONTACTO", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(TELEFONO_CONTACTO) ? "" : TELEFONO_CONTACTO;
                var_comando.Parameters.Add("@CORREO2", SqlDbType.VarChar, 150).Value = string.IsNullOrEmpty(CORREO2) ? "" : CORREO2;
                var_comando.Parameters.Add("@LUGAR_NACIMIENTO", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(LUGAR_NACIMIENTO) ? "" : LUGAR_NACIMIENTO;
                var_comando.Parameters.Add("@CONTACTO", SqlDbType.VarChar, 150).Value = string.IsNullOrEmpty(CONTACTO) ? "" : CONTACTO;
                var_comando.Parameters.Add("@CARGO", SqlDbType.VarChar, 150).Value = string.IsNullOrEmpty(CARGO) ? "" : CARGO;
                var_comando.Parameters.Add("@TELEFONO3", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(TELEFONO3) ? "" : TELEFONO3;
                var_comando.Parameters.Add("@GENERO", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(GENERO) ? "" : GENERO;
                var_comando.Parameters.Add("@ESTADO_FAMILIAR", SqlDbType.VarChar, 1).Value = string.IsNullOrEmpty(ESTADO_FAMILIAR) ? "ND" : ESTADO_FAMILIAR;
                var_comando.Parameters.Add("@CODIGO_VENDEDOR", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(CODIGO_VENDEDOR) ? "ND" : CODIGO_VENDEDOR;
                var_comando.Parameters.Add("@CODIGO_COBRADOR", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(CODIGO_COBRADOR) ? "ND" : CODIGO_COBRADOR;
                var_comando.Parameters.Add("@DIRECCION", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(DIRECCION) ? "ND" : DIRECCION;
                var_comando.Parameters.Add("@TIPO_VIVIENDA", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(TIPO_VIVIENDA) ? "" : TIPO_VIVIENDA;
                var_comando.Parameters.Add("@CODIGO_SUCURSAL", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(CODIGO_SUCURSAL) ? "ND" : CODIGO_SUCURSAL;
                var_comando.Parameters.Add("@NIVEL_ACADEMICO", SqlDbType.VarChar, 25).Value = string.IsNullOrEmpty(NIVEL_ACADEMICO) ? "ND" : NIVEL_ACADEMICO;
                var_comando.Parameters.Add("@PROMEDIO_INGRESOS", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(PROMEDIO_INGRESOS) ? "0" : PROMEDIO_INGRESOS;
                var_comando.Parameters.Add("@PROMEDIO_EGRESOS", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(PROMEDIO_EGRESOS) ? "0" : PROMEDIO_EGRESOS;

                // var_resultado = var_comando.ExecuteNonQuery();
                using (SqlDataReader dr = var_comando.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (existenCol(dr, new string[] { "ErrorMessage", "ErrorNumber" }))
                        {
                            var c = new RespuestaServicio
                            {
                                Mensaje = valorCol(dr, "ErrorMessage").ToString(),
                                Codigo = valorCol(dr, "ErrorNumber").ToString()
                            };
                            log.Info(JsonConvert.SerializeObject(c));
                            return c;
                        }

                    }
                }
                cerrarconexion();
            }

            catch (Exception ex)
            {
                //throw ex;
                log.Info("Excepcion en CrearCliente", ex);
                return new RespuestaServicio { Mensaje = MensajeSistema("96"), Codigo = "96" };
            }
            //return var_resultado;
            return new RespuestaServicio { Mensaje = MensajeSistema("100"), Codigo = "100" };
        }
        [WebMethod]
        public RespuestaServicio InsertaTransacAML(
          string USUARIO,
          string CREDENCIAL,
          string LICENCIA,
          string MAC,
          string IP,
          string Empresa,
          string TIPO_SOCIO,
          string CODIGO_CLIENTE,
          string TRANSACCION_ORIGEN,
          string TIPO_MOVIMIENTO,
          string CODIGO_TRANSACCION,
           string REFERENCIA,
           string CODIGO_ARTICULO,
           string TIPO_TRANSACCION,
           string SUB_TIPOTRANSACCION,
           string DISTINTO_ALCLIENTE,
           string CODIGO_BENEFICIARIO,
           string CODIGO_USUARIO,
           string CODIGO_SUCURSAL,
           string CODIGO_CAJERO,
           string CONCEPTO,
           string PROCEDENCIA,
           string DESTINO,
           Decimal MONTO,
           Decimal MONTO_CHEQUE,
           Decimal MONTO_OTROS,
           DateTime FECHA,
        #region Datos de cuenta de origen de la transaccion
 string NUMEROCUENTA_PO,
           string CLASECUENTA_PO,
           string CONCEPTO_PO,
        #endregion
        #region Datos de cuenta de destino de la transaccion
 string PRODUCTO_PB,
           string CLASECUENTA_PB,
           string BANCOCUENTA_PB,
        #endregion
 string ESTADO,
           string CREADO_POR
)
        {
            /* DataSet var_resultado = new DataSet();*/
            int var_resultado = 0;

            try
            {
                var_cadenaconexion = (@"server=localhost;database=" + Empresa + ";uid=" + USUARIO + ";password=" + CREDENCIAL + ";");
                /* var_cadenaconexion = (@"server=10.10.0.17;database=clinerp;uid=jgiron;password=P@ssw0rd;");*/
                abrirconexion();
                var_comando.CommandText = "[dbo].[WSAMLTRANSACCION]";
                var_comando.CommandType = CommandType.StoredProcedure;
                var_comando.Connection = var_conexion;
                var_comando.Parameters.Add("@USUARIO", SqlDbType.VarChar, 100).Value = USUARIO;
                var_comando.Parameters.Add("@CREDENCIAL", SqlDbType.VarChar, 100).Value = CREDENCIAL;
                var_comando.Parameters.Add("@LICENCIA", SqlDbType.VarChar, 200).Value = LICENCIA;
                var_comando.Parameters.Add("@MAC", SqlDbType.VarChar, 200).Value = MAC;
                var_comando.Parameters.Add("@IP", SqlDbType.VarChar, 100).Value = IP;
                var_comando.Parameters.Add("@TIPO_SOCIO", SqlDbType.Int).Value = TIPO_SOCIO;
                var_comando.Parameters.Add("@CCODIGO_CLIENTE", SqlDbType.VarChar, 40).Value = CODIGO_CLIENTE;
                var_comando.Parameters.Add("@CCODIGO_TRANSACCION", SqlDbType.VarChar, 60).Value = CODIGO_TRANSACCION;
                var_comando.Parameters.Add("@CCODIGO_ARTICULO", SqlDbType.VarChar, 40).Value = CODIGO_ARTICULO;
                var_comando.Parameters.Add("@CTIPO_TRANSACCION", SqlDbType.VarChar, 50).Value = TIPO_TRANSACCION;
                var_comando.Parameters.Add("@CSUB_TIPOTRANSACCION", SqlDbType.VarChar, 50).Value = SUB_TIPOTRANSACCION;
                var_comando.Parameters.Add("@CDISTINTO_ALCLIENTE", SqlDbType.Char, 1).Value = DISTINTO_ALCLIENTE;
                var_comando.Parameters.Add("@CCODIGO_BENEFICIARIO", SqlDbType.VarChar, 40).Value = CODIGO_BENEFICIARIO;
                var_comando.Parameters.Add("@CCODIGO_USUARIO", SqlDbType.VarChar, 40).Value = CODIGO_USUARIO;
                var_comando.Parameters.Add("@CCODIGO_SUCURSAL", SqlDbType.VarChar, 15).Value = CODIGO_SUCURSAL;
                var_comando.Parameters.Add("@CCODIGO_CAJERO", SqlDbType.VarChar, 100).Value = CODIGO_CAJERO;
                var_comando.Parameters.Add("@CCONCEPTO", SqlDbType.VarChar, 200).Value = CONCEPTO;
                var_comando.Parameters.Add("@CPROCEDENCIA", SqlDbType.VarChar, 200).Value = PROCEDENCIA;
                var_comando.Parameters.Add("@CMONTO", SqlDbType.Decimal).Value = MONTO;
                var_comando.Parameters.Add("@CMONTO_CHEQUE", SqlDbType.Decimal).Value = MONTO_CHEQUE;
                var_comando.Parameters.Add("@CMONTO_OTROS", SqlDbType.Decimal).Value = MONTO_OTROS;
                var_comando.Parameters.Add("@CFECHA", SqlDbType.DateTime).Value = FECHA;
                var_comando.Parameters.Add("@CESTADO", SqlDbType.VarChar, 25).Value = ESTADO;
                var_comando.Parameters.Add("@CCREADO_POR", SqlDbType.VarChar, 50).Value = CREADO_POR;
                var_comando.Parameters.Add("@CTRANSACCION_ORIGEN", SqlDbType.VarChar, 60).Value = TRANSACCION_ORIGEN;
                var_comando.Parameters.Add("@CTIPO_MOVIMIENTO", SqlDbType.VarChar, 10).Value = TIPO_MOVIMIENTO;
                var_comando.Parameters.Add("@CREFERENCIA", SqlDbType.VarChar, 100).Value = REFERENCIA;
                var_comando.Parameters.Add("@CDESTINO", SqlDbType.VarChar, 200).Value = DESTINO;
                #region Datos de cuenta de origen de la transaccion
                var_comando.Parameters.Add("@CNUMEROCUENTA_PO", SqlDbType.VarChar, 50).Value = NUMEROCUENTA_PO;
                var_comando.Parameters.Add("@CCLASECUENTA_PO", SqlDbType.VarChar, 50).Value = CLASECUENTA_PO;
                var_comando.Parameters.Add("@CCONCEPTO_PO", SqlDbType.VarChar, 200).Value = CONCEPTO_PO;
                #endregion
                #region Datos de cuenta de destino de la transaccion
                var_comando.Parameters.Add("@CPRODUCTO_PB", SqlDbType.VarChar, 50).Value = PRODUCTO_PB;
                var_comando.Parameters.Add("@CCLASECUENTA_PB", SqlDbType.VarChar, 50).Value = CLASECUENTA_PB;
                var_comando.Parameters.Add("@CBANCOCUENTA_PB", SqlDbType.VarChar, 50).Value = BANCOCUENTA_PB;
                #endregion
                ///var_resultado = var_comando.ExecuteNonQuery();
                using (SqlDataReader dr = var_comando.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (existenCol(dr, new string[] { "ErrorMessage", "ErrorNumber" }))
                        {
                            
                            var c = new RespuestaServicio
                            {
                                Mensaje = valorCol(dr, "ErrorMessage").ToString(),
                                Codigo = valorCol(dr, "ErrorNumber").ToString()
                            };
                            log.Info(JsonConvert.SerializeObject(c));
                            return c;
                        }
                    }
                }
                cerrarconexion();
            }

            catch (Exception ex)
            {
                //throw ex;
                log.Info("Excepcion en InsertaTransacAML", ex);
                return new RespuestaServicio { Mensaje = MensajeSistema("96"), Codigo = "96" };
            }
            //return var_resultado;
            return new RespuestaServicio { Mensaje = MensajeSistema("100"), Codigo = "100" };
        }

        [WebMethod]
        public RespuestaServicio ACTUALIZAR_TABLA(
            string IP,
            string USUARIO,
            string CREDENCIAL,
            string EMPRESA,
            string TABLA,
            string VALORLLAVE,
            string CAMPO1,
            string VALOR1,
            string CAMPO2,
            string VALOR2,
            string CAMPO3,
            string VALOR3,
            string CAMPO4,
            string VALOR4,
            string CAMPO5,
            string VALOR5
            )
        {
            try
            {
                using (var conn = DbFactory.Conn())
                {
                    conn.Open();
                    var affectedRows = conn.Execute("WS_ACTUALIZAR_CAMPOS",
                        new
                        {
                            CTABLA = TABLA,
                            CVALORLLAVE = VALORLLAVE,
                            CCAMPO1 = (string.IsNullOrEmpty(CAMPO1) ? "ND" : CAMPO1),
                            CVALOR1 = VALOR1,
                            CCAMPO2 = (string.IsNullOrEmpty(CAMPO2) ? "ND" : CAMPO2),
                            CVALOR2 = VALOR2,
                            CCAMPO3 = (string.IsNullOrEmpty(CAMPO3) ? "ND" : CAMPO3),
                            CVALOR3 = VALOR3,
                            CCAMPO4 = (string.IsNullOrEmpty(CAMPO4) ? "ND" : CAMPO4),
                            CVALOR4 = VALOR4,
                            CCAMPO5 = (string.IsNullOrEmpty(CAMPO5) ? "ND" : CAMPO5),
                            CVALOR5 = VALOR5
                        },
                        commandType: CommandType.StoredProcedure);
                    return new RespuestaServicio
                    {
                        Codigo = "100",
                        Mensaje = DbFactory.MensajeSistema("100")
                    };
                }
            }
            catch (SqlException ex)
            {
                log.Info("Error en el evento", ex);
                return new RespuestaServicio
                {
                    Codigo = "97",
                    Mensaje = ex.Message
                };
            }
            catch (Exception ex)
            {
                log.Info("Error en el evento", ex);
                return new RespuestaServicio
                {
                    Codigo = "97",
                    Mensaje = ex.Message
                };
            }
        }
      
       

        #region Metodos de la UIF
        [WebMethod]
        public RespuestaServicioUIF reporteDiarioEfectivo(
        #region Credenciales 
                string USUARIO,
                string CREDENCIAL,
                string LICENCIA,
                string MAC,
                string IP,
                string EMPRESA,
        #endregion
        #region datos generales del reporte
                string idRegistroBancario,
                string cargoColaborador,
                string codigoColaborador,
                string fechaTransaccion,
                string distintoAlCliente,
                string tipoTransaccion,
                string numeroProducto,
                string claseProducto,
                string montoTransaccion,
                string valorEfectivo,
                string conceptoTransaccion,
                string objetivoEfectivo,
        #endregion
        #region punto de servicio
                string PSdireccionAgencia,
                string PSidDepartamento,
                string PSidMunicipio,
        #endregion
        #region Datos de la Persona Fisica que realiza la transaccion
                string PTprimerApellido,
                string PTsegundoApellido,
                string PTapellidoCasado,
                string PTprimerNombre,
                string PTsegundoNombre,
                string PTfechaNacimiento,
                string PTlugarNacimiento,
                string PTcodigoNacionalidad,
                string PTcodigoEstadoCivil,
                string PTtipoDocumento,
                string PTnumeroDocumento,
                string PTpersonaDomicilio,
                string PTcodigoMunicipio,
                string PTcodigoDepartamento,
                string PTprofesionPersona,
        #endregion
        #region Datos persona natural B propietaria de la cuenta
                string tipoPersonaB,
                string PBprimerApellido,
                string PBsegundoApellido,
                string PBapellidoCasado,
                string PBprimerNombre,
                string PBsegundoNombre,
                string PBfechaNacimiento,
                string PBlugarNacimiento,
                string PBcodigoNacionalidad,
                string PBcodigoEstadoCivil,
                string PBtipoDocumento,
                string PBnumeroDocumento,
                string PBpersonaDomicilio,
                string PBcodigoMunicipio,
                string PBcodigoDepartamento,
                string PBprofesionPersona,
        #endregion
        #region Datos persona juridica B propietaria de la cuenta
                string PJBrazonSocial,
                string PJBdomicilioComercial,
                string PJBactividadEconomica,
                string PJBtipoIdentificacionT,
                string PJBnumeroIdentificacionT,
        #endregion
        #region Datos de la persona natural C
                string tipoPersonaC,
                string PCprimerApellido,
                string PCsegundoApellido,
                string PCapellidoCasado,
                string PCprimerNombre,
                string PCsegundoNombre,
                string PCfechaNacimiento,
                string PClugarNacimiento,
                string PCcodigoNacionalidad,
                string PCcodigoEstadoCivil,
                string PCtipoDocumento,
                string PCnumeroDocumento,
                string PCpersonaDomicilio,
                string PCcodigoMunicipio,
                string PCcodigoDepartamento,
                string PCprofesionPersona,
        #endregion
        #region Datos persona juridica C
                string PJCrazonSocial,
                string PJCdomicilioComercial,
                string PJCactividadEconomica,
                string PJCtipoIdentificacionT,
                string PJCnumeroIdentificacionT
        #endregion
)
        {

            XElement elem = new XElement("detalleTransacciones");
        #region Credenciales 
                elem.Add(new XElement("USUARIO",USUARIO));
                elem.Add(new XElement("CREDENCIAL",CREDENCIAL));
                elem.Add(new XElement("LICENCIA",LICENCIA));
                elem.Add(new XElement("MAC",MAC));
                elem.Add(new XElement("IP",IP));
                elem.Add(new XElement("EMPRESA",EMPRESA));
        #endregion
        #region datos generales del reporte
                elem.Add(new XElement("idRegistroBancario",idRegistroBancario));
                elem.Add(new XElement("cargoColaborador",cargoColaborador));
                elem.Add(new XElement("codigoColaborador",codigoColaborador));
                elem.Add(new XElement("fechaTransaccion",fechaTransaccion));
                elem.Add(new XElement("distintoAlCliente",distintoAlCliente));
                elem.Add(new XElement("tipoTransaccion",tipoTransaccion));
                elem.Add(new XElement("numeroProducto",numeroProducto));
                elem.Add(new XElement("claseProducto",claseProducto));
                elem.Add(new XElement("montoTransaccion",montoTransaccion));
                elem.Add(new XElement("valorEfectivo",valorEfectivo));
                elem.Add(new XElement("conceptoTransaccion",conceptoTransaccion));
                elem.Add(new XElement("objetivoEfectivo",objetivoEfectivo));
        #endregion
        #region punto de servicio
                elem.Add(new XElement("PSdireccionAgencia",PSdireccionAgencia));
                elem.Add(new XElement("PSidDepartamento",PSidDepartamento));
                elem.Add(new XElement("PSidMunicipio",PSidMunicipio));
        #endregion
        #region Datos de la Persona Fisica que realiza la transaccion
                elem.Add(new XElement("PTprimerApellido",PTprimerApellido));
                elem.Add(new XElement("PTsegundoApellido",PTsegundoApellido));
                elem.Add(new XElement("PTapellidoCasado",PTapellidoCasado));
                elem.Add(new XElement("PTprimerNombre",PTprimerNombre));
                elem.Add(new XElement("PTsegundoNombre",PTsegundoNombre));
                elem.Add(new XElement("PTfechaNacimiento",PTfechaNacimiento));
                elem.Add(new XElement("PTlugarNacimiento",PTlugarNacimiento));
                elem.Add(new XElement("PTcodigoNacionalidad",PTcodigoNacionalidad));
                elem.Add(new XElement("PTcodigoEstadoCivil",PTcodigoEstadoCivil));
                elem.Add(new XElement("PTtipoDocumento",PTtipoDocumento));
                elem.Add(new XElement("PTnumeroDocumento",PTnumeroDocumento));
                elem.Add(new XElement("PTpersonaDomicilio",PTpersonaDomicilio));
                elem.Add(new XElement("PTcodigoMunicipio",PTcodigoMunicipio));
                elem.Add(new XElement("PTcodigoDepartamento",PTcodigoDepartamento));
                elem.Add(new XElement("PTprofesionPersona",PTprofesionPersona));
        #endregion
        #region Datos persona natural B propietaria de la cuenta
                elem.Add(new XElement("tipoPersonaB",tipoPersonaB));
                elem.Add(new XElement("PBprimerApellido",PBprimerApellido));
                elem.Add(new XElement("PBsegundoApellido",PBsegundoApellido));
                elem.Add(new XElement("PBapellidoCasado",PBapellidoCasado));
                elem.Add(new XElement("PBprimerNombre",PBprimerNombre));
                elem.Add(new XElement("PBsegundoNombre",PBsegundoNombre));
                elem.Add(new XElement("PBfechaNacimiento",PBfechaNacimiento));
                elem.Add(new XElement("PBlugarNacimiento",PBlugarNacimiento));
                elem.Add(new XElement("PBcodigoNacionalidad",PBcodigoNacionalidad));
                elem.Add(new XElement("PBcodigoEstadoCivil",PBcodigoEstadoCivil));
                elem.Add(new XElement("PBtipoDocumento",PBtipoDocumento));
                elem.Add(new XElement("PBnumeroDocumento",PBnumeroDocumento));
                elem.Add(new XElement("PBpersonaDomicilio",PBpersonaDomicilio));
                elem.Add(new XElement("PBcodigoMunicipio",PBcodigoMunicipio));
                elem.Add(new XElement("PBcodigoDepartamento",PBcodigoDepartamento));
                elem.Add(new XElement("PBprofesionPersona",PBprofesionPersona));
        #endregion
        #region Datos persona juridica B propietaria de la cuenta
                elem.Add(new XElement("PJBrazonSocial",PJBrazonSocial));
                elem.Add(new XElement("PJBdomicilioComercial",PJBdomicilioComercial));
                elem.Add(new XElement("PJBactividadEconomica",PJBactividadEconomica));
                elem.Add(new XElement("PJBtipoIdentificacionT",PJBtipoIdentificacionT));
                elem.Add(new XElement("PJBnumeroIdentificacionT",PJBnumeroIdentificacionT));
        #endregion
        #region Datos de la persona natural C
                elem.Add(new XElement("tipoPersonaC",tipoPersonaC));
                elem.Add(new XElement("PCprimerApellido",PCprimerApellido));
                elem.Add(new XElement("PCsegundoApellido",PCsegundoApellido));
                elem.Add(new XElement("PCapellidoCasado",PCapellidoCasado));
                elem.Add(new XElement("PCprimerNombre",PCprimerNombre));
                elem.Add(new XElement("PCsegundoNombre",PCsegundoNombre));
                elem.Add(new XElement("PCfechaNacimiento",PCfechaNacimiento));
                elem.Add(new XElement("PClugarNacimiento",PClugarNacimiento));
                elem.Add(new XElement("PCcodigoNacionalidad",PCcodigoNacionalidad));
                elem.Add(new XElement("PCcodigoEstadoCivil",PCcodigoEstadoCivil));
                elem.Add(new XElement("PCtipoDocumento",PCtipoDocumento));
                elem.Add(new XElement("PCnumeroDocumento",PCnumeroDocumento));
                elem.Add(new XElement("PCpersonaDomicilio",PCpersonaDomicilio));
                elem.Add(new XElement("PCcodigoMunicipio",PCcodigoMunicipio));
                elem.Add(new XElement("PCcodigoDepartamento",PCcodigoDepartamento));
                elem.Add(new XElement("PCprofesionPersona",PCprofesionPersona));
        #endregion
        #region Datos persona juridica C
                elem.Add(new XElement("PJCrazonSocial",PJCrazonSocial));
                elem.Add(new XElement("PJCdomicilioComercial",PJCdomicilioComercial));
                elem.Add(new XElement("PJCactividadEconomica",PJCactividadEconomica));
                elem.Add(new XElement("PJCtipoIdentificacionT",PJCtipoIdentificacionT));
                elem.Add(new XElement("PJCnumeroIdentificacionT", PJCnumeroIdentificacionT));
        #endregion
            XDocument doc = new XDocument();
            doc.Add(elem);
            return new Servicios.UIF().reporteDiarioEfectivo(doc);
            //return null;
        }
        [WebMethod]
        public RespuestaServicioUIF reporteDiarioOtrosMedios(
        #region Credenciales
                string USUARIO,
                string CREDENCIAL,
                string LICENCIA,
                string MAC,
                string IP,
                string EMPRESA,
        #endregion
        #region datos generales del reporte
                string idRegistroBancario,
                string cargoColaborador,
                string codigoColaborador,
                string fechaTransaccion,
                string distintoAlCliente,
                string tipoTransaccion,
                string numeroProducto,
                string claseProducto,
                string montoTransaccion,
                string valorOtrosMedios,
                string conceptoTransaccion,
        #endregion
        #region punto de servicio
 string PSdireccionAgencia,
                string PSidDepartamento,
                string PSidMunicipio,
        #endregion
        #region Datos de la Persona Fisica que realiza la transaccion
 string PTprimerApellido,
                string PTsegundoApellido,
                string PTapellidoCasado,
                string PTprimerNombre,
                string PTsegundoNombre,
                string PTfechaNacimiento,
                string PTlugarNacimiento,
                string PTcodigoNacionalidad,
                string PTcodigoEstadoCivil,
                string PTtipoDocumento,
                string PTnumeroDocumento,
                string PTpersonaDomicilio,
                string PTcodigoMunicipio,
                string PTcodigoDepartamento,
                string PTprofesionPersona,
        #endregion
        #region Datos persona natural B propietaria de la cuenta
                string tipoPersonaB,
                string PBprimerApellido,
                string PBsegundoApellido,
                string PBapellidoCasado,
                string PBprimerNombre,
                string PBsegundoNombre,
                string PBfechaNacimiento,
                string PBlugarNacimiento,
                string PBcodigoNacionalidad,
                string PBcodigoEstadoCivil,
                string PBtipoDocumento,
                string PBnumeroDocumento,
                string PBpersonaDomicilio,
                string PBcodigoMunicipio,
                string PBcodigoDepartamento,
                string PBprofesionPersona,
        #endregion
        #region Datos persona juridica B propietaria de la cuenta
 string PJBrazonSocial,
                string PJBdomicilioComercial,
                string PJBactividadEconomica,
                string PJBtipoIdentificacionT,
                string PJBnumeroIdentificacionT,
        #endregion
        #region Datos de la persona natural C
 string tipoPersonaC,
                string PCprimerApellido,
                string PCsegundoApellido,
                string PCapellidoCasado,
                string PCprimerNombre,
                string PCsegundoNombre,
                string PCfechaNacimiento,
                string PClugarNacimiento,
                string PCcodigoNacionalidad,
                string PCcodigoEstadoCivil,
                string PCtipoDocumento,
                string PCnumeroDocumento,
                string PCpersonaDomicilio,
                string PCcodigoMunicipio,
                string PCcodigoDepartamento,
                string PCprofesionPersona,
        #endregion
        #region Datos persona juridica C
 string PJCrazonSocial,
                string PJCdomicilioComercial,
                string PJCactividadEconomica,
                string PJCtipoIdentificacionT,
                string PJCnumeroIdentificacionT
        #endregion
            )
        {
            
            XElement elem = new XElement("detalleTransacciones");
            #region Credenciales
            elem.Add(new XElement("USUARIO", USUARIO));
            elem.Add(new XElement("CREDENCIAL", CREDENCIAL));
            elem.Add(new XElement("LICENCIA", LICENCIA));
            elem.Add(new XElement("MAC", MAC));
            elem.Add(new XElement("IP", IP));
            elem.Add(new XElement("EMPRESA", EMPRESA));
            #endregion
            #region datos generales del reporte
            elem.Add(new XElement("idRegistroBancario", idRegistroBancario));
            elem.Add(new XElement("cargoColaborador", cargoColaborador));
            elem.Add(new XElement("codigoColaborador", codigoColaborador));
            elem.Add(new XElement("fechaTransaccion", fechaTransaccion));
            elem.Add(new XElement("distintoAlCliente", distintoAlCliente));
            elem.Add(new XElement("tipoTransaccion", tipoTransaccion));
            elem.Add(new XElement("numeroProducto", numeroProducto));
            elem.Add(new XElement("claseProducto", claseProducto));
            elem.Add(new XElement("montoTransaccion", montoTransaccion));
            elem.Add(new XElement("valorOtrosMedios", valorOtrosMedios));
            elem.Add(new XElement("conceptoTransaccion", conceptoTransaccion));
            #endregion
            #region punto de servicio
            elem.Add(new XElement("PSdireccionAgencia", PSdireccionAgencia));
            elem.Add(new XElement("PSidDepartamento", PSidDepartamento));
            elem.Add(new XElement("PSidMunicipio", PSidMunicipio));
            #endregion
            #region Datos de la Persona Fisica que realiza la transaccion
            elem.Add(new XElement("PTprimerApellido", PTprimerApellido));
            elem.Add(new XElement("PTsegundoApellido", PTsegundoApellido));
            elem.Add(new XElement("PTapellidoCasado", PTapellidoCasado));
            elem.Add(new XElement("PTprimerNombre", PTprimerNombre));
            elem.Add(new XElement("PTsegundoNombre", PTsegundoNombre));
            elem.Add(new XElement("PTfechaNacimiento", PTfechaNacimiento));
            elem.Add(new XElement("PTlugarNacimiento", PTlugarNacimiento));
            elem.Add(new XElement("PTcodigoNacionalidad", PTcodigoNacionalidad));
            elem.Add(new XElement("PTcodigoEstadoCivil", PTcodigoEstadoCivil));
            elem.Add(new XElement("PTtipoDocumento", PTtipoDocumento));
            elem.Add(new XElement("PTnumeroDocumento", PTnumeroDocumento));
            elem.Add(new XElement("PTpersonaDomicilio", PTpersonaDomicilio));
            elem.Add(new XElement("PTcodigoMunicipio", PTcodigoMunicipio));
            elem.Add(new XElement("PTcodigoDepartamento", PTcodigoDepartamento));
            elem.Add(new XElement("PTprofesionPersona", PTprofesionPersona));
            #endregion
            #region Datos persona natural B propietaria de la cuenta
            elem.Add(new XElement("tipoPersonaB", tipoPersonaB));
            elem.Add(new XElement("PBprimerApellido", PBprimerApellido));
            elem.Add(new XElement("PBsegundoApellido", PBsegundoApellido));
            elem.Add(new XElement("PBapellidoCasado", PBapellidoCasado));
            elem.Add(new XElement("PBprimerNombre", PBprimerNombre));
            elem.Add(new XElement("PBsegundoNombre", PBsegundoNombre));
            elem.Add(new XElement("PBfechaNacimiento", PBfechaNacimiento));
            elem.Add(new XElement("PBlugarNacimiento", PBlugarNacimiento));
            elem.Add(new XElement("PBcodigoNacionalidad", PBcodigoNacionalidad));
            elem.Add(new XElement("PBcodigoEstadoCivil", PBcodigoEstadoCivil));
            elem.Add(new XElement("PBtipoDocumento", PBtipoDocumento));
            elem.Add(new XElement("PBnumeroDocumento", PBnumeroDocumento));
            elem.Add(new XElement("PBpersonaDomicilio", PBpersonaDomicilio));
            elem.Add(new XElement("PBcodigoMunicipio", PBcodigoMunicipio));
            elem.Add(new XElement("PBcodigoDepartamento", PBcodigoDepartamento));
            elem.Add(new XElement("PBprofesionPersona", PBprofesionPersona));
            #endregion
            #region Datos persona juridica B propietaria de la cuenta
            elem.Add(new XElement("PJBrazonSocial", PJBrazonSocial));
            elem.Add(new XElement("PJBdomicilioComercial", PJBdomicilioComercial));
            elem.Add(new XElement("PJBactividadEconomica", PJBactividadEconomica));
            elem.Add(new XElement("PJBtipoIdentificacionT", PJBtipoIdentificacionT));
            elem.Add(new XElement("PJBnumeroIdentificacionT", PJBnumeroIdentificacionT));
            #endregion
            #region Datos de la persona natural C
            elem.Add(new XElement("tipoPersonaC", tipoPersonaC));
            elem.Add(new XElement("PCprimerApellido", PCprimerApellido));
            elem.Add(new XElement("PCsegundoApellido", PCsegundoApellido));
            elem.Add(new XElement("PCapellidoCasado", PCapellidoCasado));
            elem.Add(new XElement("PCprimerNombre", PCprimerNombre));
            elem.Add(new XElement("PCsegundoNombre", PCsegundoNombre));
            elem.Add(new XElement("PCfechaNacimiento", PCfechaNacimiento));
            elem.Add(new XElement("PClugarNacimiento", PClugarNacimiento));
            elem.Add(new XElement("PCcodigoNacionalidad", PCcodigoNacionalidad));
            elem.Add(new XElement("PCcodigoEstadoCivil", PCcodigoEstadoCivil));
            elem.Add(new XElement("PCtipoDocumento", PCtipoDocumento));
            elem.Add(new XElement("PCnumeroDocumento", PCnumeroDocumento));
            elem.Add(new XElement("PCpersonaDomicilio", PCpersonaDomicilio));
            elem.Add(new XElement("PCcodigoMunicipio", PCcodigoMunicipio));
            elem.Add(new XElement("PCcodigoDepartamento", PCcodigoDepartamento));
            elem.Add(new XElement("PCprofesionPersona", PCprofesionPersona));
            #endregion
            #region Datos persona juridica C
            elem.Add(new XElement("PJCrazonSocial", PJCrazonSocial));
            elem.Add(new XElement("PJCdomicilioComercial", PJCdomicilioComercial));
            elem.Add(new XElement("PJCactividadEconomica", PJCactividadEconomica));
            elem.Add(new XElement("PJCtipoIdentificacionT", PJCtipoIdentificacionT));
            elem.Add(new XElement("PJCnumeroIdentificacionT", PJCnumeroIdentificacionT));
            #endregion
            XDocument doc = new XDocument();
            doc.Add(elem);
            //WSUIF.reporteDiarioOtrosMediosResp response = new WSUIF.reporteDiarioOtrosMediosResp();
            //WSUIF.reporteDiarioOtrosMediosReq request = new WSUIF.reporteDiarioOtrosMediosReq();

           /* request.detalleTransacciones = new WSUIF.transaccionOtrosDTO[] {
                          new Servicios.UIF().transaccionOtrosDTO(doc)
                        };
            request.fechaReporte = DateTime.Now;
            request.fechaReporteSpecified = true;*/
            
            /*response = uifservice.reporteDiarioOtrosMedios(request);
            return Newtonsoft.Json.JsonConvert.SerializeObject(response)
                + " " +  Newtonsoft.Json.JsonConvert.SerializeObject(request);*/
            //return doc.ToString();
            //return diarioOtrosMediosRes;
            return new Servicios.UIF().reporteDiarioOtrosMedios(doc);
        }
        [WebMethod]
        public RespuestaServicioUIF reporteDiarioOtrosElectronicos(
                 #region Credenciales
				string USUARIO,
                string CREDENCIAL,
                string LICENCIA,
                string MAC,
                string IP,
                string EMPRESA,
        #endregion
        #region datos generales del reporte
                string fechaTransaccion,
        #endregion
        #region punto de servicio
				string PSdireccionAgencia,
                string PSidDepartamento,
                string PSidMunicipio,
        #endregion
        #region Datos de la Persona A propietaria de la cuenta
				string tipoPersonaA,
				string PTprimerApellido,
                string PTsegundoApellido,
                string PTapellidoCasado,
                string PTprimerNombre,
                string PTsegundoNombre,
                string PTfechaNacimiento,
                string PTlugarNacimiento,
                string PTcodigoNacionalidad,
                string PTcodigoEstadoCivil,
                string PTtipoDocumento,
                string PTnumeroDocumento,
                string PTpersonaDomicilio,
                string PTcodigoMunicipio,
                string PTcodigoDepartamento,
                string PTprofesionPersona,
        #endregion
        #region Datos persona natural B propietaria de la cuenta destino
                string tipoPersonaB,
                string PBprimerApellido,
                string PBsegundoApellido,
                string PBapellidoCasado,
                string PBprimerNombre,
                string PBsegundoNombre,
                string PBfechaNacimiento,
                string PBlugarNacimiento,
                string PBcodigoNacionalidad,
                string PBcodigoEstadoCivil,
                string PBtipoDocumento,
                string PBnumeroDocumento,
                string PBpersonaDomicilio,
                string PBcodigoMunicipio,
                string PBcodigoDepartamento,
                string PBprofesionPersona,
        #endregion
        #region Datos persona juridica A propietaria de la cuenta
				string PJArazonSocial,
                string PJAdomicilioComercial,
                string PJAactividadEconomica,
                string PJAtipoIdentificacionT,
                string PJAnumeroIdentificacionT,
        #endregion
		#region Datos persona juridica B propietaria de la cuenta
				string PJBrazonSocial,
                string PJBdomicilioComercial,
                string PJBactividadEconomica,
                string PJBtipoIdentificacionT,
                string PJBnumeroIdentificacionT,
        #endregion
		#region Cuenta de origen de la transaccion
				string numeroCuentaPO,
				string claseCuentaPO,
				string conceptoTransaccionPO,
				double valorOtrosMediosElectronicoPO,
		#endregion
		#region Cuenta destino de la transaccion
				string numeroProductoPB,
				string claseCuentaPB,
				double montoTransaccionPB,
				double valorMedioElectronicoPB,
				string bancoCuentaDestinatariaPB
		#endregion
 
            )
        {
          
            XElement elem = new XElement("detalleTransacciones");
            #region Credenciales
            elem.Add(new XElement("USUARIO", USUARIO));
            elem.Add(new XElement("CREDENCIAL", CREDENCIAL));
            elem.Add(new XElement("LICENCIA", LICENCIA));
            elem.Add(new XElement("MAC", MAC));
            elem.Add(new XElement("IP", IP));
            elem.Add(new XElement("EMPRESA", EMPRESA));
            #endregion
            #region datos generales del reporte
            elem.Add(new XElement("fechaTransaccion", fechaTransaccion));
            #endregion
            #region punto de servicio
            elem.Add(new XElement("PSdireccionAgencia", PSdireccionAgencia));
            elem.Add(new XElement("PSidDepartamento", PSidDepartamento));
            elem.Add(new XElement("PSidMunicipio", PSidMunicipio));
            #endregion
            #region Datos de la Persona A propietaria de la cuenta
            elem.Add(new XElement("tipoPersonaA", tipoPersonaA));
            elem.Add(new XElement("PTprimerApellido", PTprimerApellido));
            elem.Add(new XElement("PTsegundoApellido", PTsegundoApellido));
            elem.Add(new XElement("PTapellidoCasado", PTapellidoCasado));
            elem.Add(new XElement("PTprimerNombre", PTprimerNombre));
            elem.Add(new XElement("PTsegundoNombre", PTsegundoNombre));
            elem.Add(new XElement("PTfechaNacimiento", PTfechaNacimiento));
            elem.Add(new XElement("PTlugarNacimiento", PTlugarNacimiento));
            elem.Add(new XElement("PTcodigoNacionalidad", PTcodigoNacionalidad));
            elem.Add(new XElement("PTcodigoEstadoCivil", PTcodigoEstadoCivil));
            elem.Add(new XElement("PTtipoDocumento", PTtipoDocumento));
            elem.Add(new XElement("PTnumeroDocumento", PTnumeroDocumento));
            elem.Add(new XElement("PTpersonaDomicilio", PTpersonaDomicilio));
            elem.Add(new XElement("PTcodigoMunicipio", PTcodigoMunicipio));
            elem.Add(new XElement("PTcodigoDepartamento", PTcodigoDepartamento));
            elem.Add(new XElement("PTprofesionPersona", PTprofesionPersona));
            #endregion
            #region Datos persona natural B propietaria de la cuenta destino
            elem.Add(new XElement("tipoPersonaB", tipoPersonaB));
            elem.Add(new XElement("PBprimerApellido", PBprimerApellido));
            elem.Add(new XElement("PBsegundoApellido", PBsegundoApellido));
            elem.Add(new XElement("PBapellidoCasado", PBapellidoCasado));
            elem.Add(new XElement("PBprimerNombre", PBprimerNombre));
            elem.Add(new XElement("PBsegundoNombre", PBsegundoNombre));
            elem.Add(new XElement("PBfechaNacimiento", PBfechaNacimiento));
            elem.Add(new XElement("PBlugarNacimiento", PBlugarNacimiento));
            elem.Add(new XElement("PBcodigoNacionalidad", PBcodigoNacionalidad));
            elem.Add(new XElement("PBcodigoEstadoCivil", PBcodigoEstadoCivil));
            elem.Add(new XElement("PBtipoDocumento", PBtipoDocumento));
            elem.Add(new XElement("PBnumeroDocumento", PBnumeroDocumento));
            elem.Add(new XElement("PBpersonaDomicilio", PBpersonaDomicilio));
            elem.Add(new XElement("PBcodigoMunicipio", PBcodigoMunicipio));
            elem.Add(new XElement("PBcodigoDepartamento", PBcodigoDepartamento));
            elem.Add(new XElement("PBprofesionPersona", PBprofesionPersona));
            #endregion
            #region Datos persona juridica A propietaria de la cuenta
            elem.Add(new XElement("PJArazonSocial", PJArazonSocial));
            elem.Add(new XElement("PJAdomicilioComercial", PJAdomicilioComercial));
            elem.Add(new XElement("PJAactividadEconomica", PJAactividadEconomica));
            elem.Add(new XElement("PJAtipoIdentificacionT", PJAtipoIdentificacionT));
            elem.Add(new XElement("PJAnumeroIdentificacionT", PJAnumeroIdentificacionT));
            #endregion
            #region Datos persona juridica B propietaria de la cuenta
            elem.Add(new XElement("PJBrazonSocial", PJBrazonSocial));
            elem.Add(new XElement("PJBdomicilioComercial", PJBdomicilioComercial));
            elem.Add(new XElement("PJBactividadEconomica", PJBactividadEconomica));
            elem.Add(new XElement("PJBtipoIdentificacionT", PJBtipoIdentificacionT));
            elem.Add(new XElement("PJBnumeroIdentificacionT", PJBnumeroIdentificacionT));
            #endregion
            #region Cuenta de origen de la transaccion
            elem.Add(new XElement("numeroCuentaPO", numeroCuentaPO));
            elem.Add(new XElement("claseCuentaPO", claseCuentaPO));
            elem.Add(new XElement("conceptoTransaccionPO", conceptoTransaccionPO));
            elem.Add(new XElement("valorOtrosMediosElectronicoPO", valorOtrosMediosElectronicoPO));
            #endregion
            #region Cuenta destino de la transaccion
            elem.Add(new XElement("numeroProductoPB", numeroProductoPB));
            elem.Add(new XElement("claseCuentaPB", claseCuentaPB));
            elem.Add(new XElement("montoTransaccionPB", montoTransaccionPB));
            elem.Add(new XElement("valorMedioElectronicoPB", valorMedioElectronicoPB));
            elem.Add(new XElement("bancoCuentaDestinatariaPB", bancoCuentaDestinatariaPB));
            #endregion
            XDocument doc = new XDocument();
            doc.Add(elem);
            return new Servicios.UIF().reporteDiarioOtrosMediosElectro(doc);
        }
        [WebMethod]
        public RespuestaServicioUIF reporteMensualEfectivo(
        #region Credenciales
                string USUARIO,
                string CREDENCIAL,
                string LICENCIA,
                string MAC,
                string IP,
                string EMPRESA,
        #endregion
        #region datos generales del reporte
				string tipoPersona,
                string cargoColaborador,
                string codigoColaborador,
        #endregion
        #region Ingresos acumulados
				string INGRtotalTransacciones,
				string INGRvalorTotal,
				string INGRvalorEfectivo,
        #endregion
        #region Egresos acumulados
				string EGREtotalTransacciones,
				string EGREvalorTotal,
				string EGREvalorEfectivo,
        #endregion
        #region Datos de la Persona Fisica que realiza la transaccion
                string PTprimerApellido,
                string PTsegundoApellido,
                string PTapellidoCasado,
                string PTprimerNombre,
                string PTsegundoNombre,
                string PTfechaNacimiento,
                string PTlugarNacimiento,
                string PTcodigoNacionalidad,
                string PTcodigoEstadoCivil,
                string PTtipoDocumento,
                string PTnumeroDocumento,
                string PTpersonaDomicilio,
                string PTcodigoMunicipio,
                string PTcodigoDepartamento,
                string PTprofesionPersona,
        #endregion
        #region Datos persona juridica B propietaria de la cuenta
                string PJrazonSocial,
                string PJdomicilioComercial,
                string PJactividadEconomica,
                string PJtipoIdentificacionT,
                string PJnumeroIdentificacionT
        #endregion
            )
        {
            
            XElement elem = new XElement("detalleTransacciones");
            #region Credenciales
            elem.Add(new XElement("USUARIO", USUARIO));
            elem.Add(new XElement("CREDENCIAL", CREDENCIAL));
            elem.Add(new XElement("LICENCIA", LICENCIA));
            elem.Add(new XElement("MAC", MAC));
            elem.Add(new XElement("IP", IP));
            elem.Add(new XElement("EMPRESA", EMPRESA));
            #endregion
            #region datos generales del reporte
            elem.Add(new XElement("tipoPersona", tipoPersona));
            elem.Add(new XElement("cargoColaborador", cargoColaborador));
            elem.Add(new XElement("codigoColaborador", codigoColaborador));
            #endregion
            #region Ingresos acumulados
            elem.Add(new XElement("INGRtotalTransacciones", INGRtotalTransacciones));
            elem.Add(new XElement("INGRvalorTotal", INGRvalorTotal));
            elem.Add(new XElement("INGRvalorEfectivo", INGRvalorEfectivo));
            #endregion
            #region Egresos acumulados
            elem.Add(new XElement("EGREtotalTransacciones", EGREtotalTransacciones));
            elem.Add(new XElement("EGREvalorTotal", EGREvalorTotal));
            elem.Add(new XElement("EGREvalorEfectivo", EGREvalorEfectivo));
            #endregion
            #region Datos de la Persona Fisica que realiza la transaccion
            elem.Add(new XElement("PTprimerApellido", PTprimerApellido));
            elem.Add(new XElement("PTsegundoApellido", PTsegundoApellido));
            elem.Add(new XElement("PTapellidoCasado", PTapellidoCasado));
            elem.Add(new XElement("PTprimerNombre", PTprimerNombre));
            elem.Add(new XElement("PTsegundoNombre", PTsegundoNombre));
            elem.Add(new XElement("PTfechaNacimiento", PTfechaNacimiento));
            elem.Add(new XElement("PTlugarNacimiento", PTlugarNacimiento));
            elem.Add(new XElement("PTcodigoNacionalidad", PTcodigoNacionalidad));
            elem.Add(new XElement("PTcodigoEstadoCivil", PTcodigoEstadoCivil));
            elem.Add(new XElement("PTtipoDocumento", PTtipoDocumento));
            elem.Add(new XElement("PTnumeroDocumento", PTnumeroDocumento));
            elem.Add(new XElement("PTpersonaDomicilio", PTpersonaDomicilio));
            elem.Add(new XElement("PTcodigoMunicipio", PTcodigoMunicipio));
            elem.Add(new XElement("PTcodigoDepartamento", PTcodigoDepartamento));
            elem.Add(new XElement("PTprofesionPersona", PTprofesionPersona));
            #endregion
            #region Datos persona juridica B propietaria de la cuenta
            elem.Add(new XElement("PJrazonSocial", PJrazonSocial));
            elem.Add(new XElement("PJdomicilioComercial", PJdomicilioComercial));
            elem.Add(new XElement("PJactividadEconomica", PJactividadEconomica));
            elem.Add(new XElement("PJtipoIdentificacionT", PJtipoIdentificacionT));
            elem.Add(new XElement("PJnumeroIdentificacionT", PJnumeroIdentificacionT));
            #endregion
            XDocument doc = new XDocument();
            doc.Add(elem);
            return new UIF().reporteMensualEfectivo(doc);
            //return Newtonsoft.Json.JsonConvert.SerializeObject(request);
        }

        [WebMethod]
        public RespuestaServicioUIF reporteMensualOtros(
        #region Credenciales
                string USUARIO,
                string CREDENCIAL,
                string LICENCIA,
                string MAC,
                string IP,
                string EMPRESA,
        #endregion
        #region datos generales del reporte
                string tipoPersona,
                string cargoColaborador,
                string codigoColaborador,
        #endregion
        #region Ingresos acumulados
 string INGRtotalTransacciones,
                string INGRvalorTotal,
                string INGRvalorOtrosMedios,
        #endregion
        #region Egresos acumulados
 string EGREtotalTransacciones,
                string EGREvalorTotal,
                string EGREvalorOtrosMedios,
        #endregion
        #region Datos de la Persona Fisica que realiza la transaccion
 string PTprimerApellido,
                string PTsegundoApellido,
                string PTapellidoCasado,
                string PTprimerNombre,
                string PTsegundoNombre,
                string PTfechaNacimiento,
                string PTlugarNacimiento,
                string PTcodigoNacionalidad,
                string PTcodigoEstadoCivil,
                string PTtipoDocumento,
                string PTnumeroDocumento,
                string PTpersonaDomicilio,
                string PTcodigoMunicipio,
                string PTcodigoDepartamento,
                string PTprofesionPersona,
        #endregion
        #region Datos persona juridica que realiza la transaccion
 string PJrazonSocial,
                string PJdomicilioComercial,
                string PJactividadEconomica,
                string PJtipoIdentificacionT,
                string PJnumeroIdentificacionT
        #endregion
            )
        {
            XElement elem = new XElement("detalleTransacciones");
            #region Credenciales
            elem.Add(new XElement("USUARIO", USUARIO));
            elem.Add(new XElement("CREDENCIAL", CREDENCIAL));
            elem.Add(new XElement("LICENCIA", LICENCIA));
            elem.Add(new XElement("MAC", MAC));
            elem.Add(new XElement("IP", IP));
            elem.Add(new XElement("EMPRESA", EMPRESA));
            #endregion
            #region datos generales del reporte
            elem.Add(new XElement("tipoPersona", tipoPersona));
            elem.Add(new XElement("cargoColaborador", cargoColaborador));
            elem.Add(new XElement("codigoColaborador", codigoColaborador));
            #endregion
            #region Ingresos acumulados
            elem.Add(new XElement("INGRtotalTransacciones", INGRtotalTransacciones));
            elem.Add(new XElement("INGRvalorTotal", INGRvalorTotal));
            elem.Add(new XElement("INGRvalorOtrosMedios", INGRvalorOtrosMedios));
            #endregion
            #region Egresos acumulados
            elem.Add(new XElement("EGREtotalTransacciones", EGREtotalTransacciones));
            elem.Add(new XElement("EGREvalorTotal", EGREvalorTotal));
            elem.Add(new XElement("EGREvalorOtrosMedios", EGREvalorOtrosMedios));
            #endregion
            #region Datos de la Persona Fisica que realiza la transaccion
            elem.Add(new XElement("PTprimerApellido", PTprimerApellido));
            elem.Add(new XElement("PTsegundoApellido", PTsegundoApellido));
            elem.Add(new XElement("PTapellidoCasado", PTapellidoCasado));
            elem.Add(new XElement("PTprimerNombre", PTprimerNombre));
            elem.Add(new XElement("PTsegundoNombre", PTsegundoNombre));
            elem.Add(new XElement("PTfechaNacimiento", PTfechaNacimiento));
            elem.Add(new XElement("PTlugarNacimiento", PTlugarNacimiento));
            elem.Add(new XElement("PTcodigoNacionalidad", PTcodigoNacionalidad));
            elem.Add(new XElement("PTcodigoEstadoCivil", PTcodigoEstadoCivil));
            elem.Add(new XElement("PTtipoDocumento", PTtipoDocumento));
            elem.Add(new XElement("PTnumeroDocumento", PTnumeroDocumento));
            elem.Add(new XElement("PTpersonaDomicilio", PTpersonaDomicilio));
            elem.Add(new XElement("PTcodigoMunicipio", PTcodigoMunicipio));
            elem.Add(new XElement("PTcodigoDepartamento", PTcodigoDepartamento));
            elem.Add(new XElement("PTprofesionPersona", PTprofesionPersona));
            #endregion
            #region Datos persona juridica que realiza la transaccion
            elem.Add(new XElement("PJrazonSocial", PJrazonSocial));
            elem.Add(new XElement("PJdomicilioComercial", PJdomicilioComercial));
            elem.Add(new XElement("PJactividadEconomica", PJactividadEconomica));
            elem.Add(new XElement("PJtipoIdentificacionT", PJtipoIdentificacionT));
            elem.Add(new XElement("PJnumeroIdentificacionT", PJnumeroIdentificacionT));
            #endregion

            XDocument doc = new XDocument();
            doc.Add(elem);

            return new UIF().reporteMensualOtrosMedios(doc);
        }
        #endregion
        private void valorPropiedad(string nombre, string valor, WSUIF.personaDTO persona)
        {
            PropertyInfo[] properties = typeof(WSUIF.personaDTO).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == nombre)
                {
                    if (!string.IsNullOrEmpty(valor))
                    {
                        if (property.PropertyType == typeof(DateTime))
                        property.SetValue(persona, (DateTime.Parse(valor)));
                    }
                }
            }
        }
        /*  [WebMethod]
          public int Actualizar
          (
              int param_idproducto,
              string param_nombre,
              string param_marca,
              string param_modelo,
              DateTime param_fecharegistro,
              string param_estado
          )
          {
              int var_resultado = 0;
              try
              {
                  abrirconexion();
                  var_comando.CommandText = "pa_producto_actualizar";
                  var_comando.CommandType = CommandType.StoredProcedure;
                  var_comando.Connection = var_conexion;
                  var_comando.Parameters.Add("@idproducto", SqlDbType.Int).Value = param_idproducto;
                  var_comando.Parameters.Add("@nombreproducto", SqlDbType.VarChar, 150).Value = param_nombre;
                  var_comando.Parameters.Add("@marcaproducto", SqlDbType.VarChar, 50).Value = param_marca;
                  var_comando.Parameters.Add("@modeloproducto", SqlDbType.VarChar, 50).Value = param_modelo;
                  var_comando.Parameters.Add("@fecharegproducto", SqlDbType.DateTime).Value = param_fecharegistro;
                  var_comando.Parameters.Add("@estadoproducto", SqlDbType.VarChar).Value = param_estado;
                  var_resultado = var_comando.ExecuteNonQuery();
                  cerrarconexion();
              }
              catch (Exception ex)
              {
                  throw ex;
              }
              return var_resultado;
          }
       
         [WebMethod]
  public int insertar_producto
  (
      string param_nombre, 
      string param_marca, 
      string param_modelo, 
      DateTime param_fecharegistro, 
      string param_estado
  )
  {
      int var_resultado = 0;
      try
      {
          abrirconexion();
          var_comando.CommandText = "pa_producto_insertar";
          var_comando.CommandType = CommandType.StoredProcedure;
          var_comando.Connection = var_conexion;
          var_comando.Parameters.Add("@nombreproducto", SqlDbType.VarChar, 150).Value = param_nombre;
          var_comando.Parameters.Add("@marcaproducto", SqlDbType.VarChar, 50).Value = param_marca;
          var_comando.Parameters.Add("@modeloproducto", SqlDbType.VarChar, 50).Value = param_modelo;
          var_comando.Parameters.Add("@fecharegproducto", SqlDbType.DateTime).Value = param_fecharegistro;
          var_comando.Parameters.Add("@estadoproducto", SqlDbType.VarChar).Value = param_estado;
          var_resultado = var_comando.ExecuteNonQuery();
          cerrarconexion();
      }
      catch (Exception ex)
      {
          throw ex;
      }
      return var_resultado;
  }
       [WebMethod]
public int borrar_producto(int param_idproducto)
{
    int var_resultado = 0;
    try
    {
        abrirconexion();
        var_comando.CommandText = "pa_producto_borrar";
        var_comando.CommandType = CommandType.StoredProcedure;
        var_comando.Connection = var_conexion;
        var_comando.Parameters.Add("@idproducto", SqlDbType.Int).Value = param_idproducto;
        var_resultado = var_comando.ExecuteNonQuery();
        cerrarconexion();
    }
    catch (Exception ex)
    {
        throw ex;
    }
    return var_resultado;
}
       
         */
        /// <summary>
        /// Funcion que retorna un valor de un SqlDataReader
        /// verifica si la columna existe
        /// </summary>
        /// <param name="dr">Fila</param>
        /// <param name="col">Nombre de la columna</param>
        /// <returns></returns>
        private object valorCol(SqlDataReader dr, string col)
        {

           // if (dr.GetSchemaTable().Columns.Contains(col))
          //  {
 
                return dr[col];
           // }
            //return "";
        }
        /// <summary>
        /// Funcion que verifica si existen todas las columnas en un DataReader
        /// </summary>
        /// <param name="dr">Fila</param>
        /// <param name="cols">Nombre de las columnas</param>
        /// <returns></returns>
   
        private bool existenCol(SqlDataReader dr, string[] cols) {
            for (int i = 0, len = dr.FieldCount ; i < len; i++)
            {
                if (!cols.Contains(dr.GetName(i)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Funcion que retorna un mensaje de sistema
        /// </summary>
        /// <param name="codigo">codigo del mensaje de sistema</param>
        /// <returns></returns>
        private string MensajeSistema(string codigo)
        {
            string msj = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(var_cadenaconexion))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SELECT [dbo].[MensajeAlerta](@CODIGO_MENSAJE)";
                        cmd.Parameters.Add("@CODIGO_MENSAJE", SqlDbType.VarChar, 50).Value = codigo;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cn;
                        msj = cmd.ExecuteScalar().ToString();
                    }
                    cn.Close();
                }
                return msj;
            }
            catch (SqlException ex)
            {
                if (cadenaPropiedad(var_cadenaconexion, "database").Length == 0)
                    return "Empresa no ha sido especificada";

                return ex.Message;
            }
            catch (Exception ex)
            {

                return "Error al retornar mensaje de sistema";
                
            }
         

        }
        /// <summary>
        /// Funcion que retorna el valor de una cadena delimitada
        /// </summary>
        /// <param name="cadena">Cadena</param>
        /// <param name="nombre">Nombre del valor</param>
        /// <returns></returns>
        private string cadenaPropiedad(string cadena, string nombre)
        {
            if (string.IsNullOrEmpty(cadena))
                return "";

            var variables = cadena.Split(';'); //se obtiene los deliminados
            var s = variables.Where((t, i) =>
            {
                var v = t.Split('='); 
                if (v.Length > 1)
                {
                    return (v[0] == nombre); //se reduce la lista si la propiedad es igual al nombre
                }
                return false;
            }).FirstOrDefault();
            if (s == null) // si no retorna ninguna coincidencia
                return "";

            return (s.Trim().Split('=').Length == 1 ? "" : s.Trim().Split('=')[1]);
        }
    }

    
    public class RespuestaServicio
    {
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
    }
    public class ColectorServicio : RespuestaServicio
    {
        public string Datos { get; set; }
    }

    
    
}
