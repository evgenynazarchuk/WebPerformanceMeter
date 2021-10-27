using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Logger
{
    public class ServerLogger : ILogger
    {
        public void AddLogMessage(string logName, string logMessage, Type logMessageType)
        {
            throw new NotImplementedException();
        }

        public void Finish()
        {
            throw new NotImplementedException();
        }

        public void PostProcessing(string logName)
        {
            throw new NotImplementedException();
        }

        public void PostProcessing()
        {
            throw new NotImplementedException();
        }

        public void ProcessStop()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync()
        {
            throw new NotImplementedException();
        }
    }
}
