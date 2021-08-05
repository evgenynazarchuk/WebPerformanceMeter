using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<HttpResponse> RequestAsync(HttpRequestMessage httpRequestMessage, string user = "")
        {
            DateTime startSendRequest;
            DateTime startWaitResponse;
            DateTime startResponse;
            DateTime endResponse;

            Task<HttpResponseMessage>? httpResponseMessageTask = null;
            HttpResponseMessage httpResponseMessage;
            byte[] content;

            startSendRequest = DateTime.UtcNow;
            httpResponseMessageTask = HttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);

            startWaitResponse = DateTime.UtcNow;
            httpResponseMessage = await httpResponseMessageTask;

            startResponse = DateTime.UtcNow;
            content = await httpResponseMessage.Content.ReadAsByteArrayAsync();
            endResponse = DateTime.UtcNow;

            var responseSize = content.Length;
            long requestSize = 0;
            if (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.ContentLength.HasValue)
            {
                requestSize = httpRequestMessage.Content.Headers.ContentLength.Value;
            }

            Watcher.Send($"{user},{httpRequestMessage.RequestUri},{(int)httpResponseMessage.StatusCode},{startSendRequest:O},{startWaitResponse:O},{startResponse:O},{endResponse:O},{requestSize},{responseSize}");

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
            string user = "")
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

            return this.RequestAsync(httpRequestMessage, user);
        }
    }
}
