using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
namespace WSCLIN.Servicios
{
    [Serializable]
    public class RespuestaServicio
    {
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
    }
    [Serializable]
    public class RespuestaServicioUIF
    {
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public string CodigoTransaccion { get; set; }
        public int RegistrosProcesados { get; set; }
    }



}