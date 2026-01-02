using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;

namespace AppMGL.DAL.Helper.Logging
{
    /// <summary>
    /// Implementation of contract 
    /// using System.Diagnostics API.
    /// </summary>
    public sealed class FileSourceLog
        : ILogger
    {
        #region Members

        static int expiryDays;
        static string serverLogPath;
        static string clientLogPath;
        static FileStream stream;
        static StreamWriter writer;
        static TraceEventType logLevel;

        #endregion

        #region  Constructor

        /// <summary>
        /// Create a new instance of this file manager
        /// </summary>
        public FileSourceLog()
        {
            // Create default source
            expiryDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpiryDays"]);
            serverLogPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogsPath"] + "//Server");
            clientLogPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogsPath"] + "//Client");
            logLevel = (TraceEventType)Enum.Parse(typeof(TraceEventType), ConfigurationManager.AppSettings["LogLevel"], true);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// File internal message in configured listeners
        /// </summary>
        /// <param name="eventType">Event type to File</param>
        /// <param name="message">Message of event</param>
        void FileInternal(TraceEventType eventType, string message, Exception exception, bool isClient)
        {
            if (eventType <= logLevel)
            {
                try
                {
                    string path = (isClient) ? clientLogPath : serverLogPath;
                    if (IsDirectoryExists(path, true))
                    {
                        Open(path);
                        WriteLog(eventType.ToString(), message, exception);
                    }
                }
                catch (Exception)
                {
                    // TODO
                }
                finally
                {
                    Close();
                    Delete();
                }
            }
        }

        bool IsDirectoryExists(string directory, bool create)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    if (create)
                    {
                        Directory.CreateDirectory(directory);
                        return true;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void Open(string directory)
        {
            try
            {
                int time = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
                string fileName = directory + "\\" + DateTime.Now.ToString("yyyy-MM-dd-");
                fileName += (time < 600 ? "0000" : time < 1200 ? "0600" : time < 1800 ? "1200" : "1800") + ".log";

                stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                writer = new StreamWriter(stream);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        void WriteLog(string entryType, string message, Exception exception)
        {
            string log = "=============================================================" + Environment.NewLine;
            log += "LOCAL DATE  : " + DateTime.Now.ToString("dddd, MMMM dd yyyy, hh:mm:ss.ffff tt") + Environment.NewLine;
            log += "ENTRY TYPE  : " + entryType + Environment.NewLine;
            log += "SOURCE      : " + (exception == null ? "N/A" : exception.Source) + Environment.NewLine;
            log += "MESSAGE     : " + (message ?? exception.Message) + Environment.NewLine;
            log += "STACK TRACE : " + (exception == null ? "N/A" : exception.StackTrace) + Environment.NewLine;
            writer.WriteLine(log);
        }

        void Close()
        {
            try
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }

        void Delete()
        {
            ThreadPool.QueueUserWorkItem(o => DeletingLogs(serverLogPath));
            ThreadPool.QueueUserWorkItem(o => DeletingLogs(clientLogPath));
        }

        void DeletingLogs(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles("????-??-??-????.log");
            foreach(var file in files)
            {
                if (file.CreationTime < DateTime.Now.AddDays(expiryDays))
                {
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
            }
        }

        string Encode(string message)
        {
            return message.Replace("{", "{{").Replace("}", "}}");
        }

        #endregion

        #region ILogger Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void LogInfo(string message, params object[] args)
        {
            LogInfo(message, false, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isClient"></param>
        /// <param name="args"></param>
        public void LogInfo(string message, bool isClient, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, message, args);

                FileInternal(TraceEventType.Information, messageToFile, null, isClient);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void LogWarning(string message, params object[] args)
        {
            LogWarning(message, false, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isClient"></param>
        /// <param name="args"></param>
        public void LogWarning(string message, bool isClient, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, message, args);

                FileInternal(TraceEventType.Warning, messageToFile, null, isClient);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void LogError(string message, params object[] args)
        {
            LogError(message, null, false, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="args"></param>
        public void LogError(string message, Exception exception, params object[] args)
        {
            LogError(message, exception, false, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="isClient"></param>
        /// <param name="args"></param>
        public void LogError(string message, Exception exception, bool isClient, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message) && exception != null)
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, Encode(message), args);

                FileInternal(TraceEventType.Error, messageToFile, exception, isClient);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Debug(string message, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, message, args);

                FileInternal(TraceEventType.Verbose, messageToFile, null, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="args"></param>
        public void Debug(string message, Exception exception, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message) && exception != null)
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, Encode(message), args);

                FileInternal(TraceEventType.Error, messageToFile, exception, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Debug(object item)
        {
            if (item != null)
            {
                FileInternal(TraceEventType.Verbose, item.ToString(), null, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Fatal(string message, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, message, args);

                FileInternal(TraceEventType.Critical, messageToFile, null, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="args"></param>
        public void Fatal(string message, Exception exception, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message) && exception != null)
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, Encode(message), args);

                FileInternal(TraceEventType.Critical, messageToFile, exception, false);
            }
        }

        #endregion
    }
}
