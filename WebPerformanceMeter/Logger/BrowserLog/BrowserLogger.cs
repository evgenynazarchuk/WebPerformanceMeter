namespace WebPerformanceMeter.Logger.BrowserLog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text.Json;
    using System.Threading;

    public class BrowserLogger : PerformanceLogger
    {
        public BrowserLogger(string fileNamePrefix)
            : base(fileNamePrefix)
        {
            this.BrowserRequestLogFileName = fileNamePrefix + "_browser_request.log";
            this.BrowserRequestFileWriter = new StreamWriter(this.ToolLogFileName, false, Encoding.UTF8, 65535);
            this.BrowserRequestLogQueue = new();
        }

        public readonly StreamWriter BrowserRequestFileWriter;

        public readonly ConcurrentQueue<string> BrowserRequestLogQueue;

        public readonly string BrowserRequestLogFileName;

        private BrowserLogMessage GetBrowserLogMessage(string message)
        {
            var splittedMessage = message.Split(',');

            BrowserLogMessage log = new(splittedMessage[0],
                splittedMessage[1],
                splittedMessage[2],
                Int64.Parse(splittedMessage[3]),
                Int64.Parse(splittedMessage[4]));
            
            return log;
        }

        public override void PostProcessing()
        {
        }

        public override void UserWriteLogSerialize(string message)
        {
            // TODO serialize
            base.UserWriteLogSerialize(message);
        }

        public override void ToolWriteLogSerialize(string message)
        {
            // TODO serialize
            base.ToolWriteLogSerialize(message);
        }
    }
}
