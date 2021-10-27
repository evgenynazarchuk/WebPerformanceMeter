using System;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface IReport
    {
        Task StartAsync();

        void Stop();

        void SendLogMessage(string logName, string logMessage, Type logMessageType);
    }
}
