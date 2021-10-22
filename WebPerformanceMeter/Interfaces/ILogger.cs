using System;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface ILogger
    {
        void AddLogMessage(string logName, string logMessage, Type logMessageType);

        Task StartAsync();

        void ProcessStop();

        string Convert(string logMessage, Type logMessageType) => logMessage;

        void Finish();

        void PostProcessing(string logName);

        void PostProcessing();
    }
}
