using NLog;
using System;
using System.Reflection;
using System.Diagnostics;
using NLog.Fluent;
using System.Text;

namespace LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private readonly static NLog.ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            try
            {
                logger.Error(message);
            }
            catch (Exception ex)
            {
                string exMsg = ex.Message;
            }

        }

        public void LogError(Exception ex)
        {
            Object objException = new
            {
                Type = ex.GetType().ToString(),
                StackTrace = GetStackTrace(ex),
                Source = ex.TargetSite.ReflectedType.FullName,
                InnerException = (ex.InnerException != null) ? ex.GetBaseException().Message : ex.Message,
                Method = ex.TargetSite.Name
            };

            logger.Error().Message(ex.Message).Exception(ex).Property("CustomizedException", objException).Write();
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

        public string GetStackTrace(Exception x)
        {
            var st = new StackTrace(x, true);
            var frames = st.GetFrames();
            var traceString = new StringBuilder();

            foreach (var frame in frames)
            {
                if (frame.GetFileLineNumber() < 1 || frame.GetMethod().Name == "MoveNext")
                    continue;

                traceString.Append("File: " + frame.GetFileName());
                traceString.Append(", Method:" + frame.GetMethod().Name);
                traceString.Append(", LineNumber: " + frame.GetFileLineNumber());
                traceString.AppendLine();
                traceString.Append("                                  ");

            }
            return traceString.ToString();
        }
    }
}

