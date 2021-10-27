using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Reports;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Tools;

namespace WebPerformanceMeter
{
    public sealed partial class HttpTool : Tool
    {
        public readonly HttpClient HttpClient;

        public HttpTool(
            string baseAddress,
            Watcher watcher,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null)
            : base(watcher)
        {
            var handler = new HttpClientHandler();
            handler.SetDefaultCookie(defaultCookies);

            this.HttpClient = new HttpClient(handler);
            this.HttpClient.SetDefaultHeader(defaultHeaders);

            this.TurnOffConnectionLimit(baseAddress);
            this.SetBaseSettings(baseAddress);
        }

        public HttpTool(HttpClient client, Watcher watcher)
            : base(watcher)
        {
            this.HttpClient = client;
        }

        private void SetBaseSettings(string baseAddress)
        {
            this.HttpClient.BaseAddress = new(baseAddress);
            this.HttpClient.DefaultRequestVersion = new(2, 0);
            this.HttpClient.Timeout = Timeout.InfiniteTimeSpan;
            this.HttpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
        }

        private void TurnOffConnectionLimit(string baseAddress)
        {
            var delayServicePoint = ServicePointManager.FindServicePoint(new Uri(baseAddress));
            delayServicePoint.ConnectionLeaseTimeout = 0;
        }

        public async Task<HttpResponse> RequestAsync(
            HttpRequestMessage httpRequestMessage,
            string userName = "",
            string requestLabel = "")
        {
            //
            //Console.WriteLine($"Start request"); +

            long startSendRequest;
            long startWaitResponse;
            long startResponse;
            long endResponse;
            long requestSize = 0;

            Task<HttpResponseMessage>? httpResponseMessageTask;
            HttpResponseMessage httpResponseMessage;
            byte[] content;

            startSendRequest = ScenarioTimer.Time.Elapsed.Ticks;
            httpResponseMessageTask = HttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);

            startWaitResponse = ScenarioTimer.Time.Elapsed.Ticks;
            httpResponseMessage = await httpResponseMessageTask;

            startResponse = ScenarioTimer.Time.Elapsed.Ticks;
            content = await httpResponseMessage.Content.ReadAsByteArrayAsync();
            endResponse = ScenarioTimer.Time.Elapsed.Ticks;

            int responseSize = content.Length;

            if (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.ContentLength.HasValue)
            {
                requestSize = httpRequestMessage.Content.Headers.ContentLength.Value;
            }

            if (this.Watcher is not null)
            {
                this.Watcher.SendMessage(
                    logName: "HttpClientToolLog.json",
                    logMessage: $"{userName},{httpRequestMessage.Method.Method},{httpRequestMessage.RequestUri},{requestLabel},{(int)httpResponseMessage.StatusCode},{startSendRequest},{startWaitResponse},{startResponse},{endResponse},{requestSize},{responseSize}",
                    logMessageType: typeof(HttpLogMessage)
                    );
            }

            var response = new HttpResponse(
                statusCode: (int)httpResponseMessage.StatusCode,
                content: content,
                filename: httpResponseMessage.Content.Headers.ContentDisposition?.FileName
            );

            return response;
        }

        public Task<HttpResponse> RequestAsync(
            HttpMethod httpMethod,
            string path,
            Dictionary<string, string>? requestHeaders = null,
            HttpContent? requestContent = null,
            string userName = "",
            string requestLabel = "")
        {
            // TODO
            //using HttpRequestMessage httpRequestMessage = new()
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = httpMethod,
                RequestUri = new Uri(path, UriKind.Relative),
                Content = requestContent,
            };

            if (requestHeaders is not null)
            {
                foreach ((var name, var value) in requestHeaders)
                {
                    httpRequestMessage.Headers.Add(name, value);
                }
            }

            return this.RequestAsync(httpRequestMessage, userName, requestLabel);
        }
    }
}
