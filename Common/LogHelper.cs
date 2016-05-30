using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Common
{
    /// <summary>
    /// 日志帮助类(log4net)
    /// </summary>
    public static class LogHelper
    {
        private static readonly log4net.ILog infoLog = log4net.LogManager.GetLogger("info");

        private static readonly log4net.ILog errorLog = log4net.LogManager.GetLogger("error");

        static LogHelper()
        {
            string log4netConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            if (!File.Exists(log4netConfig))
            {
                throw new Exception("log4net.config file can't find.");
            }
            log4net.Config.XmlConfigurator.Configure(
                new FileInfo(log4netConfig)
                );
        }
        /// <summary>
        /// 以错误的形式输出异常日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void WriteLog(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(ex);
        }

        /// <summary>
        /// 以INFO的形式输出日常日志到Log4Net
        /// </summary>
        /// <param name="info">内容</param>
        public static void WriteLog(string info)
        {
            if (infoLog.IsInfoEnabled)
            {
                infoLog.Info(info);
            }
        }
    }
}
