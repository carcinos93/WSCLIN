using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft;

namespace WSCLIN.Servicios
{
    public class Logger
    {


        public static void Setup()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            var logger = hierarchy.LoggerFactory.CreateLogger((ILoggerRepository)hierarchy, "logname");
            logger.Hierarchy = hierarchy;
            //hierarchy.Root.AddAppender(GetAdoAppender());
            hierarchy.Root.AddAppender(CreateRollingFileAppender(Level.All));
            hierarchy.Root.Level = Level.All;
            hierarchy.Threshold = Level.All;
            logger.Level = Level.All;
            hierarchy.Configured = true;
            //log4net.Config.BasicConfigurator.Configure(dotNet);
            // log4net.Config.BasicConfigurator.Configure(hierarchy);
        }

        private static IAppender GetAdoAppender()
        {
            RawLayoutConverter layoutConverter = new RawLayoutConverter();

            var databaseAppender = new AdoNetAppender
            {
                BufferSize = 1,
                Name = "AdoAppender",
                //ConnectionString = "server=localhost;database=log; user=nelson;password=nightmare;port=3306",
                ConnectionString = @"Data Source=CLINSVNB03\SQLEXPRESS2012;Initial Catalog=CLINERP;User ID=sa;Password=P@ssw0rd;",
                CommandText = "INSERT INTO auditoria_ws (MENSAJE,URL,IP,PARAMETROS, PARAMETROS_GET, CLIENTE, METODO , FECHA, EXCEPCION) " +
                "VALUES (@message, @url, @ip, @params, @query, @cliente,@metodo, GETDATE(), @exception)",
                CommandType = System.Data.CommandType.Text,
                ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                //ConnectionType = "MySql.Data.MySqlClient.MySqlConnection, MySql.Data"
            };

            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@message",
                DbType = System.Data.DbType.String,
                Size = 4000,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new PatternLayout("%m"))
            });

            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@exception",
                DbType = System.Data.DbType.String,
                Size = 2000,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new ExceptionLayout())
            });

            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@level",
                DbType = System.Data.DbType.String,
                Size = 50,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new PatternLayout("%-5p"))
            });

            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@logger",
                DbType = System.Data.DbType.String,
                Size = 255,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new PatternLayout("%c"))
            });

            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@thread",
                DbType = System.Data.DbType.String,
                Size = 255,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new PatternLayout("%t"))
            });

            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@url",
                DbType = System.Data.DbType.String,
                Size = 8000,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new UrlPatternLayout("%url%"))
            });
            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@query",
                DbType = System.Data.DbType.String,
                Size = 8000,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new UrlPatternLayout("%query%"))
            });
            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@ip",
                DbType = System.Data.DbType.String,
                Size = 50,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new UrlPatternLayout("%ip%"))
            });
            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@cliente",
                DbType = System.Data.DbType.String,
                Size = 255,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new UrlPatternLayout("%cli%"))
            });
            //metodo
            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@metodo",
                DbType = System.Data.DbType.String,
                Size = 50,
                Layout = (IRawLayout)layoutConverter.ConvertFrom(new UrlPatternLayout("%method%"))
            });
            databaseAppender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@params",
                DbType = System.Data.DbType.String,

                Layout = (IRawLayout)layoutConverter.ConvertFrom(new UrlPatternLayout("%params%"))
            });
            databaseAppender.ActivateOptions();

            return databaseAppender;
        }

        private static LogFileAppender CreateRollingFileAppender(Level level)
        {
            var usingFileName = string.Format("logs\\clin_{0}-{1}-{2}_{3}.log",
            DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, level.Name);
            //var layout = new PatternLayout("[%level] [%date] [%P{params}] %message%newline");
            var layout1 = new log4net.Layout.SerializedLayout();
          
            layout1.AddDecorator(new log4net.Layout.Decorators.StandardTypesFlatDecorator());
            layout1.AddRemove("message");
            layout1.AddMember("fecha:date");
            layout1.AddMember("mensaje:messageobject");
            layout1.AddMember("url:url");
            layout1.AddMember("ip:ip");
            layout1.AddMember("cliente:cli");
            layout1.AddMember("metodo:metodo");
            layout1.AddMember("parametros:parametros");
            layout1.AddMember("excepcion:exception");
          
            layout1.ActivateOptions();
            var rollingFileAppender = new LogFileAppender
            {
                Layout = layout1,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                File = usingFileName,
                ImmediateFlush = true,
                Threshold = level
            };
            rollingFileAppender.ActivateOptions();

            return rollingFileAppender;
        }
    }

    public class UrlPatternLayout : PatternLayout
    {
        private string _format;
        public UrlPatternLayout(string format)
        {
            this._format = format;
        }
        public override void Format(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        {
            string output = this._format;
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                if (new Regex("%url%").IsMatch(_format))
                {
                    output = Regex.Replace(output, "%url%", (context.Request.Url == null ? context.Request.RawUrl : context.Request.Url.AbsoluteUri));
                    //  
                }

                if (new Regex("%ip%").IsMatch(_format))
                {
                    output = Regex.Replace(output, "%ip%", (context.Request.ServerVariables["REMOTE_ADDR"]));
                }

                if (new Regex("%params%").IsMatch(_format))
                {
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    if (context.Request.QueryString.Count == 0)
                    {
                        foreach (string i in context.Request.Form.AllKeys)
                        {
                            parametros.Add(i, context.Request.Form.Get(i));
                        }
                    }
                    else
                    {
                        foreach (string i in context.Request.QueryString.AllKeys)
                        {
                            parametros.Add(i, context.Request.QueryString.Get(i));
                        }
                    }
                    output = Regex.Replace(output, "%params%", (JsonConvert.SerializeObject(parametros)));
                }
                if (new Regex("%query%").IsMatch(_format))
                {
                    output = Regex.Replace(output, "%query%", (context.Request.Url == null ? "" : context.Request.Url.Query));
                }
                if (new Regex("%cli%").IsMatch(_format))
                {
                    output = Regex.Replace(output, "%cli%", context.Request.UserAgent);
                }

                if (new Regex("%method%").IsMatch(_format))
                {
                    output = Regex.Replace(output, "%method%", context.Request.HttpMethod);
                }
            }
            writer.Write(output);

            //base.Format(writer, loggingEvent);
        }
        /*protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        {
            string url = "";
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                url = (context.Request.Url == null ? context.Request.RawUrl : context.Request.Url.AbsoluteUri);
            }
            writer.Write(url);
        }*/
    }
    public class LogFileAppender : RollingFileAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var properties = loggingEvent.Properties;
            properties["url"] = string.Empty;
            properties["ip"] = string.Empty;
            properties["parametros"] = string.Empty;
            properties["urlQuery"] = string.Empty;
            properties["cli"] = string.Empty;
            properties["metodo"] = string.Empty;
           
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                properties["url"] = (context.Request.Url == null ? context.Request.RawUrl : context.Request.Url.AbsoluteUri);
                properties["ip"] = (context.Request.ServerVariables["REMOTE_ADDR"]);

                Dictionary<string, string> parametros = new Dictionary<string, string>();
                if (context.Request.QueryString.Count == 0)
                {
                    foreach (string i in context.Request.Form.AllKeys)
                    {
                        parametros.Add(i, context.Request.Form.Get(i));
                    }
                }
                else
                {
                    foreach (string i in context.Request.QueryString.AllKeys)
                    {
                        parametros.Add(i, context.Request.QueryString.Get(i));
                    }
                }
                properties["parametros"] = (JsonConvert.SerializeObject(parametros));
                properties["urlQuery"] = (context.Request.Url == null ? "" : context.Request.Url.Query);
                properties["cli"] = context.Request.UserAgent;
                properties["metodo"] = context.Request.HttpMethod;
         
            }

            base.Append(loggingEvent);
        }
    }
  
}