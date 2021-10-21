using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Tools.HttpTool;

namespace WebPerformanceMeter
{
    public partial class BaseHttpUser : BaseUser, IBaseHttpUser
    {
        public IHttpTool Tool { get; set; }

        public HttpClient Client { get; set; }

        public BaseHttpUser(HttpClient client, string userName = "")
            : base()
        {
            this.Client = client;
            this.Tool = new HttpTool(client);
            this.SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
        }

        public BaseHttpUser(
            string address,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null,
            string userName = "")
            : base()
        {
            this.Client = new HttpClient() { BaseAddress = new Uri(address) };
            this.Tool = new HttpTool(address, defaultHeaders, defaultCookies);
            this.SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
        }

        public virtual Task<HttpResponse> RequestAsync(
            HttpMethod httpMethod,
            string path,
            Dictionary<string, string>? requestHeaders = null,
            HttpContent? requestContent = null,
            string userName = "",
            string requestLabel = "")
        {
            return this.RequestAsync(httpMethod, path, requestHeaders, requestContent, userName, requestLabel);
        }
    }
}
