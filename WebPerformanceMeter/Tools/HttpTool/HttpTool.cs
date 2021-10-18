using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Logger.HttpClientLog;
using WebPerformanceMeter.Support;

namespace WebPerformanceMeter.Tools.HttpTool
{
    public partial class HttpTool : Tool
    {
        public readonly HttpClient HttpClient;

        private readonly HttpClientHandler _handler = new();

        public readonly ILogger Logger;

        public HttpTool(
            ILogger logger,
            string baseAddress,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null)
        {
            this.Logger = logger;

            this._handler = new();
            this.SetDefaultCookie(defaultCookies);

            this.HttpClient = new(_handler);
            this.SetDefaultHeaders(defaultHeaders);

            this.SetBaseSettings(baseAddress);
        }

        public HttpTool(ILogger logger, HttpClient client)
        {
            this.Logger = logger;
            this.HttpClient = client;
        }

        private void SetDefaultHeaders(IDictionary<string, string>? headers)
        {
            if (headers is not null)
            {
                foreach (var (headerName, headerValue) in headers)
                {
                    HttpClient.DefaultRequestHeaders.Add(headerName, headerValue);
                }
            }
        }

        private void SetDefaultCookie(IEnumerable<Cookie>? cookies)
        {
            if (cookies is not null && _handler is not null)
            {
                CookieContainer cookieContainer = new();
                foreach (var cookie in cookies)
                {
                    cookieContainer.Add(cookie);
                }

                _handler.CookieContainer = cookieContainer;
                _handler.UseCookies = true;
            }
        }

        private void SetBaseSettings(string baseAddress)
        {
            var delayServicePoint = ServicePointManager.FindServicePoint(new Uri(baseAddress));
            delayServicePoint.ConnectionLeaseTimeout = 0;

            HttpClient.BaseAddress = new(baseAddress);
            HttpClient.DefaultRequestVersion = new(2, 0);
            HttpClient.Timeout = Timeout.InfiniteTimeSpan;
            HttpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
        }

        public async Task<HttpResponse> RequestAsync(
            HttpRequestMessage httpRequestMessage,
            string userName = "",
            string requestLabel = "")
        {
            long startSendRequest;
            long startWaitResponse;
            long startResponse;
            long endResponse;
            long requestSize = 0;

            Task<HttpResponseMessage>? httpResponseMessageTask = null;
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

            this.Logger.AppendLogMessage("HttpClientToolLog.json", $"{userName},{httpRequestMessage.Method.Method},{httpRequestMessage.RequestUri},{requestLabel},{(int)httpResponseMessage.StatusCode},{startSendRequest},{startWaitResponse},{startResponse},{endResponse},{requestSize},{responseSize}", typeof(HttpClientToolLogMessage));

            var response = new HttpResponse(
                statusCode: (int)httpResponseMessage.StatusCode,
                content: content,
                filename: httpResponseMessage.Content.Headers.ContentDisposition?.FileName
            );

            return response;
        }

        public Task<HttpResponse> RequestAsync(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            HttpContent? requestContent = null,
            string userName = "",
            string requestLabel = "")
        {
            using HttpRequestMessage httpRequestMessage = new()
            {
                Method = httpMethod,
                RequestUri = new(requestUri, UriKind.Relative),
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
