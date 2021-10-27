using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Report
{
    public class Watcher
    {
        protected readonly List<ILogger> loggers;

        public Watcher()
        { 
            this.loggers = new List<ILogger>();
        }

        public void SendMessage(string logName, string logMessage, Type logMessageType)
        {
            foreach (var logger in this.loggers)
            {
                logger.SendLogMessage(logName, logMessage, logMessageType);
            }
        }

        public List<Task> StartAsync()
        {
            var tasks = new List<Task>();

            foreach (var logger in this.loggers)
            {
                tasks.Add(logger.StartAsync());
            }

            return tasks;
        }

        public void Stop()
        {
            foreach (var logger in this.loggers)
            {
                logger.Stop();
            }
        }
    }
}
