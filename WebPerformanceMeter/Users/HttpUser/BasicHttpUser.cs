using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Reports;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicHttpUser : BasicUser
    {
        public HttpTool Tool { get; set; }

        public HttpClient Client { get; set; }

        public BasicHttpUser(HttpClient client, string userName)
            : base(userName)
        {
            this.Client = client;
            this.Tool = new HttpTool(client, this.Watcher);
        }

        public BasicHttpUser(
            string address,
            string userName,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null)
            : base(userName)
        {
            this.Client = new HttpClient() { BaseAddress = new Uri(address) };
            this.Tool = new HttpTool(address, this.Watcher, defaultHeaders, defaultCookies);
        }

        public Task<HttpResponse> Request(
            HttpMethod httpMethod,
            string path,
            Dictionary<string, string>? requestHeaders = null,
            HttpContent? requestContent = null,
            string requestLabel = "")
        {
            return this.Tool.RequestAsync(
                httpMethod,
                path,
                requestHeaders,
                requestContent,
                this.UserName,
                requestLabel);
        }

        public async Task<string> Get(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string requestLabel = "")
        {
            var httpResponse = await this.Tool.RequestAsync(
                HttpMethod.Get,
                path,
                requestHeaders,
                null,
                this.UserName,
                requestLabel);

            return httpResponse.ContentAsUtf8String;
        }

        public async Task<string> Delete(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string requestLabel = "")
        {
            var httpResponse = await this.Tool.RequestAsync(
                HttpMethod.Delete,
                path,
                requestHeaders,
                null,
                this.UserName,
                requestLabel);

            return httpResponse.ContentAsUtf8String;
        }


    }
}
