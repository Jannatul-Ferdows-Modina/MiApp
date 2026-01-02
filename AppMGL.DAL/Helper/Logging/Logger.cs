using System;
using AppMGL.DAL.Helper.Helper;

namespace AppMGL.DAL.Helper.Logging
{
    public static class Logger
    {
        readonly static ILogger _logger;

        static Logger()
        {
            LoggerFactory.SetCurrent(new FileSourceLogFactory());
            _logger = LoggerFactory.CreateLog();
        }

        public static void WriteInfo(string message)
        {
            _logger.LogInfo(message);
        }

        public static void WriteInfo(string message, bool isClient)
        {
            _logger.LogInfo(message, isClient);
        }

        public static void WriteWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public static void WriteWarning(string message, bool isClient)
        {
            _logger.LogWarning(message, isClient);
        }

        public static void WriteError(Log log)
        {
            ClientException ex = new ClientException(log.Message, log.StackTrace);
            ex.Source = log.Url;
            _logger.LogError(log.Message, ex, true);
        }

        public static void WriteError(string message, Exception ex)
        {
            _logger.LogError(message, ex);
        }

        public static void WriteError(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        
    }
}
