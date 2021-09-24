namespace WebPerformanceMeter.Logger
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Collections.Concurrent;

    public interface IPerformanceLogger
    {
        ConcurrentQueue<(string logName, string logMessage, Type logType)> LogQueue { get; }

        Dictionary<string, StreamWriter> Writers { get; }

        void AppendLogMessage(string logName, string logMessage, Type logMessageType);

        Task ProcessStart();

        void ProcessStop();

        string Convert(string logMessage, Type logMessageType) => logMessage;

        void Finish();

        void PostProcessing(string logName);

        void PostProcessing();
    }
}
