namespace WebPerformanceMeter.Logger.HttpClientLog
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public class HttpClientLogger : PerformanceLogger
    {
        public HttpClientLogger(string userLogFileName, string toolLogFileName)
            : base(userLogFileName, toolLogFileName) { }
        
        public override void UserWriteLogSerialize(string message)
        {
            throw new NotImplementedException();
            //var logMesageEntity = this.GetHttpClientLogMessage(message);
            //var logMessageJsonString = JsonSerializer.Serialize(logMesageEntity, this.JsonSerializerOptions);
            //base.UserWriteLogSerialize(logMessageJsonString);
        }

        public override void ToolWriteLogSerialize(string message)
        {
            var logMesageEntity = this.GetHttpClientLogMessage(message);
            var logMessageJsonString = JsonSerializer.Serialize(logMesageEntity, this.JsonSerializerOptions);
            base.ToolWriteLogSerialize(logMessageJsonString);
        }

        public override void PostProcessing()
        {
            GenerateHtmlReport();
        }

        private HttpClientToolLogMessage GetHttpClientLogMessage(string message)
        {
            var splittedMessage = message.Split(',');

            HttpClientToolLogMessage log = new(splittedMessage[0],
                splittedMessage[1],
                splittedMessage[2],
                splittedMessage[3],
                Int32.Parse(splittedMessage[4]),
                Int64.Parse(splittedMessage[5]),
                Int64.Parse(splittedMessage[6]),
                Int64.Parse(splittedMessage[7]),
                Int64.Parse(splittedMessage[8]),
                Int32.Parse(splittedMessage[9]),
                Int32.Parse(splittedMessage[10]));

            return log;
        }

        public void GenerateHtmlReport()
        {
            var htmlGenerate = new HttpClientToolLogHtmlReportGenerator(this.ToolLogFileName, "httpclient_report.html");
            htmlGenerate.GenerateReport();
        }
    }
}
