using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class HttpUser : BasicHttpUser, ISimpleUser
    {
        public HttpUser(
            HttpClient client, 
            string userName = "",
            ILogger? logger = null)
            : base(client, userName, logger) { }

        public HttpUser(
            string address,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null,
            string userName = "",
            ILogger? logger = null)
            : base(address, defaultHeaders, defaultCookies, userName, logger) { }

        protected abstract Task PerformanceAsync();

        public async Task InvokeAsync(int userLoopCount)
        {
            for (int i = 0; i < userLoopCount; i++)
            {
                await PerformanceAsync();
            }
        }
    }
}
