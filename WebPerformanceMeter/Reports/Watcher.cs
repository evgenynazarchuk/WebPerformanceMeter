using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Reports
{
    public class Watcher
    {
        protected readonly List<IReport> reports;

        public Watcher()
        {
            this.reports = new List<IReport>();
        }

        public void AddReport(IReport report)
        {
            this.reports.Add(report);
        }

        public void ClearReports()
        {
            this.reports.Clear();
        }

        public void SendMessage(string logName, string logMessage, Type logMessageType)
        {
            foreach (var logger in this.reports)
            {
                logger.SendLogMessage(logName, logMessage, logMessageType);
            }
        }

        public List<Task> StartAsync()
        {
            var tasks = new List<Task>();

            foreach (var logger in this.reports)
            {
                tasks.Add(logger.StartAsync());
            }

            return tasks;
        }

        public void Stop()
        {
            foreach (var logger in this.reports)
            {
                logger.Stop();
            }
        }
    }
}
