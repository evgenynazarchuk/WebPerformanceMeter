using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebPerformanceMeter.Support;

namespace WebPerformanceMeter.Tools.HttpTool
{
    public partial class HttpTool : Tool
    {
        public readonly HttpClient HttpClient;
        private readonly HttpClientHandler _handler = new();

        public HttpTool(
            string baseAddress,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null
            )
        {
            _handler = new();
            SetDefaultCookie(defaultCookies);

            HttpClient = new(_handler);
            SetDefaultHeaders(defaultHeaders);

            SetBaseSettings(baseAddress);
        }

        public HttpTool(HttpClient client)
        {
            HttpClient = client;
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
            //DateTime startSendRequest;
            //DateTime startWaitResponse;
            //DateTime startResponse;
            //DateTime endResponse;

            long startSendRequest;
            long startWaitResponse;
            long startResponse;
            long endResponse;

            //Console.WriteLine($"3333333 {Scenario.WatchTime.ElapsedMilliseconds}");

            Task<HttpResponseMessage>? httpResponseMessageTask = null;
            HttpResponseMessage httpResponseMessage;
            byte[] content;

            //startSendRequest = DateTime.UtcNow;
            startSendRequest = Scenario.WatchTime.Elapsed.Ticks;
            httpResponseMessageTask = HttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);

            //startWaitResponse = DateTime.UtcNow;
            startWaitResponse = Scenario.WatchTime.Elapsed.Ticks;
            httpResponseMessage = await httpResponseMessageTask;

            //startResponse = DateTime.UtcNow;
            startResponse = Scenario.WatchTime.Elapsed.Ticks;
            content = await httpResponseMessage.Content.ReadAsByteArrayAsync();
            //endResponse = DateTime.UtcNow;
            endResponse = Scenario.WatchTime.Elapsed.Ticks;

            int responseSize = content.Length;
            long requestSize = 0;
            if (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.ContentLength.HasValue)
            {
                requestSize = httpRequestMessage.Content.Headers.ContentLength.Value;
            }

            //Console.WriteLine($"123 {user},{httpRequestMessage.RequestUri},{(int)httpResponseMessage.StatusCode},{startSendRequest},{startWaitResponse},{startResponse},{endResponse},{requestSize},{responseSize}");
            Watcher.Send($"{userName},http,{httpRequestMessage.RequestUri},{requestLabel},{(int)httpResponseMessage.StatusCode},{startSendRequest},{startWaitResponse},{startResponse},{endResponse},{requestSize},{responseSize}");

            HttpResponse response = new(
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
