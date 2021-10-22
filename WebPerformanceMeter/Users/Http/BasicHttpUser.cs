using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;

namespace WebPerformanceMeter.Users
{
    public partial class BasicHttpUser : BaseUser, IBaseHttpUser
    {
        public IHttpTool Tool { get; set; }

        public HttpClient Client { get; set; }

        public BasicHttpUser(HttpClient client, string userName = "", ILogger? logger = null)
            : base(userName, logger ?? HttpLoggerSingleton.GetInstance())
        {
            this.Client = client;
            this.Tool = new HttpTool(client, this.logger);
        }

        public BasicHttpUser(
            string address,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null,
            string userName = "",
            ILogger? logger = null)
            : base(userName, logger ?? HttpLoggerSingleton.GetInstance())
        {
            this.Client = new HttpClient() { BaseAddress = new Uri(address) };
            this.Tool = new HttpTool(address, defaultHeaders, defaultCookies, this.logger);
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
                this.userName, 
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
                this.userName, 
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
                this.userName, 
                requestLabel);

            return httpResponse.ContentAsUtf8String;
        }
    }
}
