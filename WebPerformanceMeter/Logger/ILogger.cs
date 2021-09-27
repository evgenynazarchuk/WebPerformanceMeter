using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger
{
    public interface ILogger
    {
        ConcurrentQueue<(string logName, string logMessage, Type logType)> LogQueue { get; }

        ConcurrentDictionary<string, StreamWriter> Writers { get; }

        void AppendLogMessage(string logName, string logMessage, Type logMessageType);

        Task ProcessStart();

        void ProcessStop();

        string Convert(string logMessage, Type logMessageType) => logMessage;

        void Finish();

        void PostProcessing(string logName);

        void PostProcessing();
    }
}
