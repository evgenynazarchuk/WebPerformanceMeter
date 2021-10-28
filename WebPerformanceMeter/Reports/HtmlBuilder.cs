using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace WebPerformanceMeter.Reports
{
    public abstract class HtmlBuilder<TLog>
        where TLog : class
    {
        public HtmlBuilder(
            string sourceJsonFilePath,
            string destinationHtmlFilePath)
        {
            this.logs = new();
            this._reader = new(sourceJsonFilePath, Encoding.UTF8, false, 65535);
            this._writer = new(destinationHtmlFilePath, false, Encoding.UTF8, 65355);
        }

        public void Build()
        {
            this.ReadJsonObject();
            var htmlReport = this.GenerateHtml();
            this.SaveHtml(htmlReport);
        }

        protected abstract string GenerateHtml();

        private void SaveHtml(string htmlReport)
        {
            _writer.WriteLine(htmlReport);
            _writer.Flush();
            _writer.Close();
        }

        private void ReadJsonObject()
        {
            string? line;
            TLog? httpLogMessage;

            while ((line = this._reader.ReadLine()) != null)
            {
                httpLogMessage = JsonSerializer.Deserialize<TLog>(line);

                if (httpLogMessage is null)
                {
                    throw new ApplicationException("Error convertation");
                }

                this.logs.Add(httpLogMessage);
            }

            this._reader.Close();
        }

        protected readonly List<TLog> logs;

        private readonly StreamReader _reader;

        private readonly StreamWriter _writer;
    }
}
