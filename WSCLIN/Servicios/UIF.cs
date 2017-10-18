using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using log4net;
namespace WSCLIN.Servicios
{

    public class UIF : WSBase
    {
        private static ILog log = LogManager.GetLogger(typeof(UIF));
        public UIF()
        {
            
        }
        /// <summary>
        /// Inicio de sesion en la uif
        /// </summary>
        /// <returns></returns>
        public WSUIF.inicioSesionRes UIFInicioSesion()
        {
            try
            {
                //var respuestaSP = this.executeSP("WS_VALIDA", true, null);

                    WSUIF.inicioSesionReq credenciales;
                    //Variable que almacena la respuesta de la petición de inicio de sesion de la wsUIF
                    WSUIF.inicioSesionRes respuesta = new WSUIF.inicioSesionRes();
                    //Para crear la peticion de inicio de session
                    WSUIF.inicioSesionRequest peticion = new WSUIF.inicioSesionRequest();
                    //variable para invocar los metodos del WS de la UIF
                    WSUIF.ReporteTransaccionesWebServiceClient uifservice = new WSUIF.ReporteTransaccionesWebServiceClient();

                    credenciales = new WSUIF.inicioSesionReq();
                    credenciales.cuenta = this.USUARIO.Trim();
                    credenciales.clave = this.CREDENCIAL.Trim();
                    credenciales.mac = this.MAC.Trim();

                    //Se crea la petición de inicio de sesion enviando las credenciales de autenticacion
                    peticion = new WSUIF.inicioSesionRequest(credenciales);

                    //Invoca al metodo de inicio de sesion y retorna 
                    //la respuesta y de ser correcto el token de validacion de 24 horas de duracion
                    respuesta = uifservice.inicioSesion(peticion.request);
                    return respuesta;

                //return new RespuestaServicio { Codigo = respuesta.codigoMensaje.ToString(), Mensaje = respuesta.descripcionMensaje };
            }
            catch (SoapException ex)
            {
                log.Info("Error soap de inicio de sesion", ex);
                return new WSUIF.inicioSesionRes
                {
                    codigoMensaje = 97,
                    descripcionMensaje = ex.Detail.ToString(),
                    codigoMensajeSpecified = false,
                    token = null
                };
                //return new RespuestaServicio { Codigo = ex.Code.ToString(), Mensaje = string.Format("Error en nodo {0}, causa : {1}", ex.Actor, ex.Detail) };
            }
            catch (Exception ex)
            {
                log.Info("Error en el inicio de sesion de la uif",ex);
                //return new RespuestaServicio { Codigo = "96", Mensaje = MensajeSistema("96") };
                return new WSUIF.inicioSesionRes
                {
                    codigoMensaje = 97,
                    descripcionMensaje = ex.Message,
                    codigoMensajeSpecified = false,
                    token = null
                };
            }
        }

        /// <summary>
        /// Reporte diario efectivo
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public RespuestaServicioUIF reporteDiarioEfectivo(XDocument doc)
        {
            var x = doc.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();
            WSUIF.reporteDiarioEfectivoReq request = new WSUIF.reporteDiarioEfectivoReq();
            var usuario = x.Element(x.Name.Namespace + "USUARIO").ToStr();
            var credencial = x.Element(x.Name.Namespace + "CREDENCIAL").ToStr();
            var mac = x.Element(x.Name.Namespace + "MAC").ToStr();

            request.detalleTransacciones = new WSUIF.transaccionDTO[] {
                           this.transaccionDTO(doc)
                         };
            request.fechaReporte = DateTime.Now;
            request.fechaReporteSpecified = true;
            var r = enviar(request, credencial, usuario, mac);
            return r;
        }
        //


        /// <summary>
        /// Reporte diario otros medios
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public RespuestaServicioUIF reporteDiarioOtrosMedios(XDocument doc)
        {
            var x = doc.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();
            WSUIF.reporteDiarioOtrosMediosReq request = new WSUIF.reporteDiarioOtrosMediosReq();
            var usuario = x.Element(x.Name.Namespace + "USUARIO").ToStr();
            var credencial = x.Element(x.Name.Namespace + "CREDENCIAL").ToStr();
            var mac = x.Element(x.Name.Namespace + "MAC").ToStr();

            request.detalleTransacciones = new WSUIF.transaccionOtrosDTO[] {
                           this.transaccionOtrosDTO(doc)
                         };
             request.fechaReporte = DateTime.Now;
             request.fechaReporteSpecified = true;
            var r = enviar(request, credencial, usuario, mac);
            return r;
        }

        /// <summary>
        /// Reporte diario otros medios electronicos
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public RespuestaServicioUIF reporteDiarioOtrosMediosElectro(XDocument doc)
        {
            var x = doc.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();
            WSUIF.reporteDiarioOtrosMediosElectronicoReq request = new WSUIF.reporteDiarioOtrosMediosElectronicoReq();
            var usuario = x.Element(x.Name.Namespace + "USUARIO").ToStr();
            var credencial = x.Element(x.Name.Namespace + "CREDENCIAL").ToStr();
            var mac = x.Element(x.Name.Namespace + "MAC").ToStr();

            request.detalleTransacciones = new WSUIF.transaccionOtrosElectronicoDTO[] {
                           this.transaccionOtrosElectronicos(doc)
                         };
            request.fechaReporte = DateTime.Now;
            request.fechaReporteSpecified = true;
            var r = enviar(request, credencial, usuario, mac);
            return r;
        }

        /// <summary>
        /// Reporte mensual efectivo
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public RespuestaServicioUIF reporteMensualEfectivo(XDocument doc)
        {
            var x = doc.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();
            WSUIF.reporteMensualEfectivoReq request = new WSUIF.reporteMensualEfectivoReq();
            var usuario = x.Element(x.Name.Namespace + "USUARIO").ToStr();
            var credencial = x.Element(x.Name.Namespace + "CREDENCIAL").ToStr();
            var mac = x.Element(x.Name.Namespace + "MAC").ToStr();
            
            request.detalleMensualEfectivo = new WSUIF.transaccionMensualDTO[] {
                           this.transaccionMensualEfectivo(doc)
                         };
            request.fechaReporte = DateTime.Now;
            request.fechaReporteSpecified = true;
            request.cargoPersonaReporta = x.Element(x.Name.Namespace + "cargoColaborador").ToStr();
            request.codigoPersonaReporta = x.Element(x.Name.Namespace + "codigoColaborador").ToStr();
            var r = enviar(request, credencial, usuario, mac);
            return r;
        }

        /// <summary>
        /// Reporte mensual otros medios
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public RespuestaServicioUIF reporteMensualOtrosMedios(XDocument doc)
        {
            var x = doc.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();
            WSUIF.reporteMensualOtrosMediosReq request = new WSUIF.reporteMensualOtrosMediosReq();
            var usuario = x.Element(x.Name.Namespace + "USUARIO").ToStr();
            var credencial = x.Element(x.Name.Namespace + "CREDENCIAL").ToStr();
            var mac = x.Element(x.Name.Namespace + "MAC").ToStr();

            request.detalleMensualOtros = new WSUIF.transMensualOtrosDTO[] {
                           this.transaccionMensualOtros(doc)
                         };
            request.fechaReporte = DateTime.Now;
            request.fechaReporteSpecified = true;
            request.cargoPersonaReporta = x.Element(x.Name.Namespace + "cargoColaborador").ToStr();
            request.codigoPersonaReporta = x.Element(x.Name.Namespace + "codigoColaborador").ToStr();
            var r = enviar(request, credencial, usuario, mac);
            return r;
        }


        /// <summary>
        /// Funcion que realiza el envio del request
        /// </summary>
        /// <param name="req">Objeto Request</param>
        /// <param name="credencial">Password del servicio web</param>
        /// <param name="usuario">Nombre de usuario</param>
        /// <param name="mac">Direccion MAC</param>
        /// <returns></returns>
        private RespuestaServicioUIF enviar(object req, string credencial, string usuario, string mac)
        {
            try
            {
                //Estblecemos las credenciales
                this.USUARIO = usuario;
                this.CREDENCIAL = credencial;
                this.MAC = mac;
                WSUIF.inicioSesionRes sesion = this.UIFInicioSesion();
                //Si el token es nulo, entonces fallo el inicio de sesion
                if (sesion.token == null)
                    return new RespuestaServicioUIF
                    {
                        Codigo = sesion.codigoMensaje.ToString(),
                        Mensaje = sesion.descripcionMensaje,
                        RegistrosProcesados = 0,
                        CodigoTransaccion = ""
                    };
                WSUIF.ReporteTransaccionesWebServiceClient uifservice = new WSUIF.ReporteTransaccionesWebServiceClient();
                ///si el objeto es reporte diario otros medios
                if ((req as WSUIF.reporteDiarioOtrosMediosReq) != null)
                {
                    var request = req as WSUIF.reporteDiarioOtrosMediosReq;
                    request.tokenValido = sesion.token;
                 
                    WSUIF.reporteDiarioOtrosMediosResp response = new WSUIF.reporteDiarioOtrosMediosResp();
                    response = uifservice.reporteDiarioOtrosMedios(request);
                    return new RespuestaServicioUIF
                    {
                        Codigo = response.codigoMensaje.ToString(),
                        CodigoTransaccion = response.codigoTransaccion,
                        Mensaje = response.descripcionMensaje,
                        RegistrosProcesados = response.registrosProcesados
                    };
                }

                //si el objeto es reporte diario efectivo
                if ((req as WSUIF.reporteDiarioEfectivoReq) != null)
                {
                    var request = req as WSUIF.reporteDiarioEfectivoReq;
                    request.tokenValido = sesion.token;

                    WSUIF.reporteDiarioEfectivoRes response = new WSUIF.reporteDiarioEfectivoRes();
                    response = uifservice.reporteDiarioEfectivo(request);
                    return new RespuestaServicioUIF
                    {
                        Codigo = response.codigoMensaje.ToString(),
                        CodigoTransaccion = response.codigoTransaccion,
                        Mensaje = response.descripcionMensaje,
                        RegistrosProcesados = response.registrosProcesados
                    };
                }
                //si el objeto es reporte diario otros medios electronicos
                if ((req as WSUIF.reporteDiarioOtrosMediosElectronicoReq) != null)
                {
                    var request = req as WSUIF.reporteDiarioOtrosMediosElectronicoReq;
                    request.tokenValido = sesion.token;

                    WSUIF.reporteDiarioOtrosMediosElectronicoRes response = new WSUIF.reporteDiarioOtrosMediosElectronicoRes();
                    response = uifservice.reporteOtrosMediosElectronicos(request);
                    return new RespuestaServicioUIF
                    {
                        Codigo = response.codigoMensaje.ToString(),
                        CodigoTransaccion = response.codigoTransaccion,
                        Mensaje = response.descripcionMensaje,
                        RegistrosProcesados = response.registrosProcesados
                    };
                }

                //REPORTES MENSUALES
                //si el objeto es reporte mensual efectivo
                if ((req as WSUIF.reporteMensualEfectivoReq) != null)
                {
                    var request = req as WSUIF.reporteMensualEfectivoReq;
                    request.tokenValido = sesion.token;
                    WSUIF.reporteMensualEfectivoRes response = new WSUIF.reporteMensualEfectivoRes();
                    response = uifservice.reporteMensualEfectivo(request);
                    return new RespuestaServicioUIF
                    {
                        Codigo = response.codigoMensaje.ToString(),
                        CodigoTransaccion = response.codigoTransaccion,
                        Mensaje = response.descripcionMensaje,
                        RegistrosProcesados = response.registrosProcesados
                    };
                }

                //si el objeto es reporte mensual otros medios
                if ((req as WSUIF.reporteMensualOtrosMediosReq) != null)
                {
                    var request = req as WSUIF.reporteMensualOtrosMediosReq;
                    request.tokenValido = sesion.token;
                    WSUIF.reporteMensualOtrosMediosRes response = new WSUIF.reporteMensualOtrosMediosRes();
                    response = uifservice.reporteMensualOtrosMedios(request);
                    return new RespuestaServicioUIF
                    {
                        Codigo = response.codigoMensaje.ToString(),
                        CodigoTransaccion = response.codigoTransaccion,
                        Mensaje = response.descripcionMensaje,
                        RegistrosProcesados = response.registrosProcesados
                    };
                }

            }
            catch (SoapException ex) //Se captura la excepcion soap
            {
                log.Info("Excepcion soap", ex);
                return new RespuestaServicioUIF
                {
                    Codigo = "97",
                    CodigoTransaccion = "",
                    Mensaje = ex.Message,
                    RegistrosProcesados = 0
                };
            } 
            catch (Exception ex)
            {
                log.Info("Excepcion en envio", ex);
                return new RespuestaServicioUIF
                {
                    Codigo = "97",
                    CodigoTransaccion = "",
                    Mensaje = ex.Message,
                    RegistrosProcesados = 0
                };
            }   
                
            
            return null;
        }
        /// <summary>
        /// Reporte de transaccion diaria
        /// </summary>
        /// <param name="TIPO_REPORTE">Tipo de reporte</param>
        /// <param name="CODIGO_TRANSACCION">Código de la transaccion</param>
        /// <returns></returns>
        public object reporteTransaccion(
            string TIPO_REPORTE,
            string CODIGO_TRANSACCION
            )
        {

            //Para iniciar sesion en el WSUIF
            WSUIF.inicioSesionRes sesion = this.UIFInicioSesion();
            WSUIF.ReporteTransaccionesWebServiceClient uifservice = new WSUIF.ReporteTransaccionesWebServiceClient();
            //se valida el token
            if (string.IsNullOrEmpty(sesion.token))
            {
                return new RespuestaServicioUIF
                {
                    Codigo = sesion.codigoMensaje.ToString(),
                    Mensaje = sesion.descripcionMensaje
                };
            }
            else
            {
                Dictionary<string, object> outputs = new Dictionary<string, object>(); //sirva para las variables output
                var respuesta = this.executeSP("WS_UIF", false, new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@TIPO_REPORTE", SqlDbType = SqlDbType.VarChar, Size = 10, Value = TIPO_REPORTE },
                    new SqlParameter { ParameterName = "@CODIGO_TRANSACCION", SqlDbType = SqlDbType.VarChar, Value = CODIGO_TRANSACCION },
                    new SqlParameter { ParameterName = "@xml", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Output }
                }, outputs);

                //if (!respuesta.hasException) //si no hubo ninguna excepcion 
                //{
                    //var xml = new XmlDocument();
                     var xml = XDocument.Load(@"C:\Users\nrodas\Documents\nelson\Nelson\Nelson\WSCLIN\WSCLIN\Servicios\XMLFile1.xml");
                    //var xml = XDocument.Load(outputs["@xml"].ToString()); //se obtiene el xml generado desde la base de datos
                    // xml.Load(outputs["@xml"].ToString());
                    if (TIPO_REPORTE == "1") // TRANSACCION EN EFECTIVO
                    {
                        //Variable que almacena la respuesta del Reporte Diario: codido de mensaje, descripcion del mensaje,
                        //numero de registros procesados, codigo de transaccion
                        WSUIF.reporteDiarioEfectivoRes diarioEfectivoRes = new WSUIF.reporteDiarioEfectivoRes();

                        //reporte diario efectivo
                        WSUIF.reporteDiarioEfectivoReq diarioEfectivoReq = new WSUIF.reporteDiarioEfectivoReq();

                        diarioEfectivoReq.detalleTransacciones = new WSUIF.transaccionDTO[] {
                            this.transaccionDTO(xml)
                        };
                        diarioEfectivoReq.fechaReporte = DateTime.Now;
                        diarioEfectivoReq.tokenValido = sesion.token;
                        diarioEfectivoRes = uifservice.reporteDiarioEfectivo(diarioEfectivoReq);
                        return diarioEfectivoRes;
                    }

                    if (TIPO_REPORTE == "2") // Reporte Diario Otros Medios
                    {
                        WSUIF.reporteDiarioOtrosMediosResp diarioOtrosMediosRes = new WSUIF.reporteDiarioOtrosMediosResp();
                        WSUIF.reporteDiarioOtrosMediosReq diarioOtrosMediosReq = new WSUIF.reporteDiarioOtrosMediosReq();

                        diarioOtrosMediosReq.detalleTransacciones = new WSUIF.transaccionOtrosDTO[] {
                            this.transaccionOtrosDTO(xml)
                        };
                        diarioOtrosMediosReq.fechaReporte = DateTime.Now;
                        diarioOtrosMediosReq.tokenValido = sesion.token;
                        diarioOtrosMediosRes = uifservice.reporteDiarioOtrosMedios(diarioOtrosMediosReq);
                        return diarioOtrosMediosRes;
                    }

                    if (TIPO_REPORTE == "3") // Report de otros medios electronicos
                    {
                        WSUIF.reporteDiarioOtrosMediosElectronicoReq request = new WSUIF.reporteDiarioOtrosMediosElectronicoReq();
                        WSUIF.reporteDiarioOtrosMediosElectronicoRes response = new WSUIF.reporteDiarioOtrosMediosElectronicoRes();
                        request.detalleTransacciones = new WSUIF.transaccionOtrosElectronicoDTO[] {
                            this.transaccionOtrosElectronicos(xml)
                        };
                        request.fechaReporte = DateTime.Now;
                        request.tokenValido = sesion.token;
                        response = uifservice.reporteOtrosMediosElectronicos(request);
                        return response;
                        //WSUIF.reporteOtrosMediosElectronicosResponse respo
                    }

                    if (TIPO_REPORTE == "4") //Reporte mensual transacciones efectivo
                    {
                        WSUIF.reporteMensualEfectivoReq request = new WSUIF.reporteMensualEfectivoReq();
                        WSUIF.reporteMensualEfectivoRes response = new WSUIF.reporteMensualEfectivoRes();
                        request.detalleMensualEfectivo = new WSUIF.transaccionMensualDTO[] {
                            this.transaccionMensualEfectivo(xml)
                        };
                        request.fechaReporte = DateTime.Now;
                        request.tokenValido = sesion.token;
                        request.cargoPersonaReporta = "PROGRAMADOR";
                        request.codigoPersonaReporta = "NRODAS";
                        response = uifservice.reporteMensualEfectivo(request);
                        return response;
                    }

                   
                }

           // }
            return new RespuestaServicioUIF
            {

            };
        }
        #region Reporte de transacciones diarias
        /// <summary> Evento que retorna la transaccion en otros medios </summary> <param
        /// name="document"><Documento xml</param> <returns></returns>
        public WSUIF.transaccionOtrosDTO transaccionOtrosDTO(XDocument document)
        {
            var t = new WSUIF.transaccionOtrosDTO();
            var x = document.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();
           
            t.idRegistroBancario = (string)x.Element(x.Name.Namespace + "idRegistroBancario").Value;
            t.puntoServicio = new WSUIF.puntoServicioDTO
            {
                direccionAgencia = (string)x.Element(x.Name.Namespace + "PSdireccionAgencia").Value,
                idDepartamento = (string)x.Element(x.Name.Namespace + "PSidDepartamento").Value,
                idMunicipio = (string)x.Element(x.Name.Namespace + "PSidMunicipio").Value
            };
            t.codigoColaborador = (string)x.Element(x.Name.Namespace + "codigoColaborador").Value;
            t.cargoColaborador = (string)x.Element(x.Name.Namespace + "cargoColaborador").Value;
            t.fechaTransaccion = x.Element(x.Name.Namespace + "fechaTransaccion").ToDate();
            t.fechaTransaccionSpecified = true;
            t.distintoalCliente = x.Element(x.Name.Namespace + "distintoAlCliente").ToInt();
            t.tipoTransaccion = x.Element(x.Name.Namespace + "tipoTransaccion").ToInt();
            t.numeroProducto = (string)x.Element(x.Name.Namespace + "numeroProducto").Value;
            t.claseProducto = (string)x.Element(x.Name.Namespace + "claseProducto").Value;
            t.montoTransaccion = x.Element(x.Name.Namespace + "montoTransaccion").ToDouble();
            t.montoTransaccionSpecified = true;
            t.valorOtrosMedios = x.Element(x.Name.Namespace + "valorOtrosMedios").ToDouble();
            t.valorOtrosMediosSpecified = true;
            t.conceptoTransaccion = (string)x.Element(x.Name.Namespace + "conceptoTransaccion").Value;

            t.personaFisicaT = persona("PT", x);
            t.tipoPersonaB = x.Element(x.Name.Namespace + "tipoPersonaB").ToInt();
            if (t.tipoPersonaB == 1)
            {
                t.listaPersonaB = new WSUIF.personaDTO[] { 
                        persona("PB", x)
                    };
            }
            else if (t.tipoPersonaB == 2)
            {
                t.listaJuridicaB = new WSUIF.juridicaDTO[] { 
                        juridica("PJB",x)
                    };
            }
            t.tipoPersonaC = x.Element(x.Name.Namespace + "tipoPersonaC").ToInt();
            if (t.tipoPersonaC == 1)
            {
                t.listaPersonaC = new WSUIF.personaDTO[] { 
                        persona("PC", x)
                    };
            }
            else if (t.tipoPersonaC == 2)
            {
                t.listaJuridicaC = new WSUIF.juridicaDTO[] { 
                       juridica("PJC",x)
                    };
            }
            return t;
        }
        /// <summary>
        /// Evento que retorna las transaccion en efectivo
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public WSUIF.transaccionDTO transaccionDTO(XDocument document)
        {
            var t = new WSUIF.transaccionDTO();
            var x = document.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();

            t.idRegistroBancario = x.Element(x.Name.Namespace + "idRegistroBancario").ToStr();
            t.puntoServicio = new WSUIF.puntoServicioDTO
            {
                direccionAgencia = x.Element(x.Name.Namespace + "PSdireccionAgencia").ToStr(),
                idDepartamento = x.Element(x.Name.Namespace + "PSidDepartamento").ToStr(),
                idMunicipio = x.Element(x.Name.Namespace + "PSidMunicipio").ToStr()
            };
            t.codigoColaborador = x.Element(x.Name.Namespace + "codigoColaborador").ToStr();
            t.cargoColaborador = x.Element(x.Name.Namespace + "cargoColaborador").ToStr();
            t.fechaTransaccion = x.Element(x.Name.Namespace + "fechaTransaccion").ToDate();
            t.distintoalCliente = x.Element(x.Name.Namespace + "distintoAlCliente").ToInt();
            t.tipoTransaccion = x.Element(x.Name.Namespace + "tipoTransaccion").ToInt();
            t.numeroProducto = x.Element(x.Name.Namespace + "numeroProducto").ToStr();
            t.claseProducto = x.Element(x.Name.Namespace + "claseProducto").ToStr();
            t.montoTransaccion = x.Element(x.Name.Namespace + "montoTransaccion").ToDouble();
            t.fechaTransaccionSpecified = true;
            t.montoTransaccionSpecified = true;
            t.distintoalClienteSpecified = true;
            t.valorEfectivo = x.Element(x.Name.Namespace + "valorEfectivo").ToDouble();
            t.conceptoTransaccion = x.Element(x.Name.Namespace + "conceptoTransaccion").ToStr();
            t.objetivoEfectivo = x.Element(x.Name.Namespace + "objetivoEfectivo").ToStr();

            t.personaFisicaT = persona("PT", x);
            t.tipoPersonaB = x.Element(x.Name.Namespace + "tipoPersonaB").ToInt();
            if (t.tipoPersonaB == 1)
            {
                t.listaPersonaB = new WSUIF.personaDTO[] { 
                        persona("PB", x)
                    };
            }
            else if (t.tipoPersonaB == 2)
            {
                t.listaJuridicaB = new WSUIF.juridicaDTO[] { 
                        juridica("PJB",x)
                    };
            }
           
            t.tipoPersonaC = x.Element(x.Name.Namespace + "tipoPersonaC").ToInt();

            if (t.tipoPersonaC == 1)
            {
                t.listaPersonaC = new WSUIF.personaDTO[] { 
                        persona("PC", x)
                    };
            }
            else if (t.tipoPersonaC == 2)
            {
                t.listaJuridicaC = new WSUIF.juridicaDTO[] { 
                        juridica("PJC",x)
                    };
            }
            return t;
        }
        /// <summary>
        /// Funcion que retorna una transaccion de otros medios electrocnicos
        /// </summary>
        /// <param name="document">documento xml</param>
        /// <returns></returns>
        public WSUIF.transaccionOtrosElectronicoDTO transaccionOtrosElectronicos(XDocument document)
        {
            var t = new WSUIF.transaccionOtrosElectronicoDTO();
            var x = document.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();
            //Detalle de la estacion o sucursal de servicio
            t.estacionServicio = new WSUIF.puntoServicioDTO
            {
                direccionAgencia = x.Element(x.Name.Namespace + "PSdireccionAgencia").ToStr(),
                idDepartamento = x.Element(x.Name.Namespace + "PSidDepartamento").ToStr(),
                idMunicipio = x.Element(x.Name.Namespace + "PSidMunicipio").ToStr()
            };
            t.fechaTransaccion = x.Element(x.Name.Namespace + "fechaTransaccion").ToDate();
            t.fechaTransaccionSpecified = true;
            //Tipo de persona A : Juridica o natural
            t.tipoPersonaA = x.Element(x.Name.Namespace + "tipoPersonaA").ToInt();

            //Persona A: Propietaria de la cuenta
            t.personaNaturalSeccionA = persona("PT", x);
          
            //Persona juridica A: Propietaria de la cuenta
            t.personaJuridicaSeccionA = juridica("PJA", x);

            // Tipo persona B 
            t.tipoPersonaB = x.Element(x.Name.Namespace + "tipoPersonaB").ToInt();
            // Persona B: propietaria de la cuenta destino
            t.listaPersonaB = new WSUIF.personaDTO[] { 
                        persona("PB", x)
            };
            t.listaJuridicaB = new WSUIF.juridicaDTO[] { 
                        juridica("PJB",x)
            };
           
            //Cuenta de origen de la transaccion
            t.numeroCuentaPO = x.Element(x.Name.Namespace + "numeroCuentaPO").ToStr();
            t.claseCuentaPO = x.Element(x.Name.Namespace + "claseCuentaPO").ToStr();
            t.conceptoTransaccionPO = x.Element(x.Name.Namespace + "conceptoTransaccionPO").ToStr();
            t.valorOtrosMediosElectronicoPO = x.Element(x.Name.Namespace + "valorOtrosMediosElectronicoPO").ToDouble();
            t.valorOtrosMediosElectronicoPOSpecified = true;
            //Cuenta destino de la transaccion
            t.numeroProductoPB = x.Element(x.Name.Namespace + "numeroProductoPB").ToStr();
            t.claseCuentaPB = x.Element(x.Name.Namespace + "claseCuentaPB").ToStr();
            t.montoTransaccionPB = x.Element(x.Name.Namespace + "montoTransaccionPB").ToDouble();
            t.montoTransaccionPBSpecified = true;
            t.valorMedioElectronicoPB = x.Element(x.Name.Namespace + "valorMedioElectronicoPB").ToDouble();
            t.bancoCuentaDestinatariaPB = x.Element(x.Name.Namespace + "claseCuentaPB").ToStr();
            t.valorMedioElectronicoPBSpecified = true;
           
            return t;
        }

        #endregion

        #region Report de transacciones mensuales

        /// <summary>
        /// Reporte mensual efectivo
        /// </summary>
        /// <param name="document">Documento xml</param>
        /// <returns></returns>
        public WSUIF.transaccionMensualDTO transaccionMensualEfectivo(XDocument document)
        {
            var t = new WSUIF.transaccionMensualDTO();
            var x = document.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();

            t.tipoPersona = x.Element(x.Name.Namespace + "tipoPersona").ToInt();
            t.tipoPersonaSpecified = true;
            if (t.tipoPersona == 1)
            {
                //Datos de la persona natural que acumulo las transacciones
                t.personaTransaccion = persona("PT", x);
            }
            if (t.tipoPersona == 2)
            {
                //Datos de la persona juridica que acumulo las transacciones
                t.juridicaTransaccion = juridica("PJ", x);
            }
            //Total de transacciones de ingresos acumuladas
            t.detalleIngreso = totalizadorDTO("INGR", x);
            //Total de transacciones de egresos acumuladas
            t.detalleEgreso = totalizadorDTO("EGRE", x);
            

            return t;
        }
        /// <summary>
        /// Reporte mensual otros medios
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public WSUIF.transMensualOtrosDTO transaccionMensualOtros(XDocument document)
        {
            var t = new WSUIF.transMensualOtrosDTO();
            var x = document.Descendants().Where(q => q.Name.LocalName == "detalleTransacciones").FirstOrDefault();

            //Tipo persona
            t.tipoPersona = x.Element(x.Name.Namespace + "tipoPersona").ToInt();
            t.tipoPersonaSpecified = true;

            if (t.tipoPersona == 1)
            {
                //Datos persona natural propietaria de la cuenta
                t.personaTransaccion = persona("PT", x);
            }
            if (t.tipoPersona == 2)
            {
                //Datos persona juridica propietaria de la cuenta
                t.juridicaTransaccion = juridica("PJ", x);
            }
            //Total de transacciones ingreso acumuladas
            t.detalleIngreso = this.totalizadorOtros("INGR", x);
            //Total de transacciones egreso acumuladas
            t.detalleEgreso = this.totalizadorOtros("EGRE", x);
            //Tipo de persona especificada
            return t;
        }
        #endregion

        #region Tipo de personas
        private WSUIF.personaDTO persona(string tipo, XElement x)
        {
            var p = new WSUIF.personaDTO();
                
                p.primerApellido = (string)x.Element(x.Name.Namespace + tipo + "primerApellido").Value;
                p.segundoApellido = (string)x.Element(x.Name.Namespace + tipo + "segundoApellido").Value;
                p.apellidoCasado = (string)x.Element(x.Name.Namespace + tipo + "apellidoCasado").Value;
                p.primerNombre = (string)x.Element(x.Name.Namespace + tipo + "primerNombre").Value;
                p.segundoNombre = (string)x.Element(x.Name.Namespace + tipo + "segundoNombre").Value;
                p.fechaNacimiento = x.Element(x.Name.Namespace + tipo + "fechaNacimiento").ToDate();
                p.fechaNacimientoSpecified = true;
                p.lugarNacimiento = (string)x.Element(x.Name.Namespace + tipo + "lugarNacimiento").Value;
                p.codigoNacionalidad = (string)x.Element(x.Name.Namespace + tipo + "codigoNacionalidad").Value;
                p.codigoEstadoCivil = x.Element(x.Name.Namespace + tipo + "codigoEstadoCivil").ToInt();
                p.codigoEstadoCivilSpecified = true;
                p.tipoDocumentoSpecified = true;
                p.tipoDocumento = x.Element(x.Name.Namespace + tipo + "tipoDocumento").ToInt();
                p.numeroDocumento = (string)x.Element(x.Name.Namespace + tipo + "numeroDocumento").Value;
                p.personaDomicilio = (string)x.Element(x.Name.Namespace + tipo + "personaDomicilio").Value;
                p.codigoMunicipio = (string)x.Element(x.Name.Namespace + tipo + "codigoMunicipio").Value;
                p.codigoDepartamento = (string)x.Element(x.Name.Namespace + tipo + "codigoDepartamento").Value;
                p.profesionPersona = (string)x.Element(x.Name.Namespace + tipo + "profesionPersona").Value;
             return p;

        }
        /// <summary>
        /// Funcion que retorna los datos de la persona juridica
        /// </summary>
        /// <param name="tipo">debe ser un sufijo</param>
        /// <param name="x">Documento xml</param>
        /// <returns></returns>
        private WSUIF.juridicaDTO juridica(string tipo, XElement x)
        {
            return new WSUIF.juridicaDTO
                {
                   
                    razonSocial = x.Element(x.Name.Namespace + tipo + "razonSocial").ToStr(),
                    domicilioComercial = x.Element(x.Name.Namespace + tipo + "domicilioComercial").ToStr(),
                    actividadEconomica = x.Element(x.Name.Namespace + tipo + "actividadEconomica").ToStr(),
                    tipoIdentificacionT = x.Element(x.Name.Namespace + tipo + "tipoIdentificacionT").ToInt(),
                    numeroIdentificacionT = x.Element(x.Name.Namespace + tipo + "numeroIdentificacionT").ToStr()
                };            
        }
        #endregion
        #region Totalizadores de Reportes mensuales
        /// <summary>
        /// Totalizador de transacciones en efectivo
        /// </summary>
        /// <param name="tipo">Sufijo</param>
        /// <param name="x">Elemento xml</param>
        /// <returns></returns>
        private WSUIF.totalizadorDTO totalizadorDTO(string tipo, XElement x) {
            var p = new WSUIF.totalizadorDTO();
                p.totalTransaccionesSpecified = true;
                p.valorEfectivoSpecified = true;
                p.valorTotalSpecified = true;
                p.totalTransacciones = x.Element(x.Name.Namespace + tipo + "totalTransacciones").ToInt();
                p.valorTotal = x.Element(x.Name.Namespace + tipo + "valorTotal").ToDouble();
                p.valorEfectivo = x.Element(x.Name.Namespace + tipo + "valorEfectivo").ToDouble();
            return p;
        }
        /// <summary>
        /// Totalizador de transacciones de otros medios
        /// </summary>
        /// <param name="tipo">Sufijo</param>
        /// <param name="x">Elemento xml</param>
        /// <returns></returns>
        private WSUIF.totalizadorOtrosDTO totalizadorOtros(string tipo, XElement x)
        {
            var p = new WSUIF.totalizadorOtrosDTO();
                p.totalTransacciones = x.Element(x.Name.Namespace + tipo + "totalTransacciones").ToInt();
                p.valorOtrosMedios = x.Element(x.Name.Namespace + tipo + "valorOtrosMedios").ToDouble();
                p.valorTotal = x.Element(x.Name.Namespace + tipo + "valorTotal").ToDouble();

                p.totalTransaccionesSpecified = true;
                p.valorOtrosMediosSpecified = true;
                p.valorTotalSpecified = true;
            return p;
        }
        #endregion
    }


    #region Conversiones de los XElement
    /// <summary>
    /// Clase estatica para las conversiones de los elemento xml
    /// </summary>
    public static class XmlParse
    {
        /// <summary>
        /// Funcion que convierte a entero
        /// </summary>
        /// <param name="element"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static int ToInt(this XElement element, Int32? @default = null)
        {
            int i;
            if (Int32.TryParse(element.Value, out i))
            {
                return i;
            }
            if (@default.HasValue)
                return @default.Value;
            return 0;
        }
        /// <summary>
        /// Funcion que convierte el valor a fecha
        /// </summary>
        /// <param name="element">>elemento del xml</param>
        /// <returns></returns>
        public static DateTime ToDate(this XElement element)
        {
            DateTime d = (DateTime.Parse(element.Value));
            return d;
        }
        /// <summary>
        ///  Funcion que convierte el valor a doble
        /// </summary>
        /// <param name="element">>elemento del xml</param>
        /// <returns></returns>
        public static Double ToDouble(this XElement element)
        {
            return (Double.Parse(element.Value));
        }
        /// <summary>
        /// Funcion que retorna el valor del elemento
        /// </summary>
        /// <param name="element">elemento del xml</param>
        /// <returns>String del valor</returns>
        public static String ToStr(this XElement element)
        {
            return (element == null ? "" : element.Value.Trim());
        }
    }
    #endregion
}