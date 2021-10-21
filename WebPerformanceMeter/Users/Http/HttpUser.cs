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
    public abstract partial class HttpUser : BaseHttpUser, IHttpUser
    {
        public HttpUser(HttpClient client, string userName = "")
            : base(client, userName) { }

        public HttpUser(
            string address,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null,
            string userName = "")
            : base(address, defaultHeaders, defaultCookies, userName) { }

        public abstract Task PerformanceAsync();

        public async Task InvokeAsync(int userLoopCount)
        {
            for (int i = 0; i < userLoopCount; i++)
            {
                await PerformanceAsync();
            }
        }
    }
}
