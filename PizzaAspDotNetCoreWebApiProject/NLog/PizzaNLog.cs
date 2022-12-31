using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace PizzaAspDotNetCoreWebApiProject.NLog
{
    public class PizzaNLog : IPizzaNLog
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void Information(string logMessage)
        {
            logger.Info(logMessage);
        }

        public void Error(string logMessage)
        {
            logger.Error(logMessage);
        }
    }
}
