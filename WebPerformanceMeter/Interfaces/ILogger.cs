using System;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface ILogger
    {
        Task StartAsync();

        void Stop();

        void SendLogMessage(string logName, string logMessage, Type logMessageType);
    }
}
