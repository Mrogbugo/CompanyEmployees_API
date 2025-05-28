using Contracts;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = NLog.ILogger;

namespace LoggerService
{
    public class LoggerManger : ILoggerManager 
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public LoggerManger() { }

        public void LogDebug(string message) => logger.Debug(message);
        public void LogError(string message) => logger.Error(message);
        public void LogInfo(string message) => logger.Info(message);
        public void LogWarn(string message) => logger.Warn(message);

        /* the above method is used to log the message in the file which contain four method 
         * logDebug for debugging purpose
         * LogError for error purpose
         * LogInfo for information purpose
         * LogWarn for warning purpose
         */

    }
}
