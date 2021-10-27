using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class HttpUser : BasicHttpUser, ISimpleUser
    {
        public HttpUser(
            HttpClient client,
            string? userName = null)
            : base(client, userName ?? typeof(HttpUser).Name) { }

        public HttpUser(
            string address,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null,
            string? userName = null)
            : base(address, userName ?? typeof(HttpUser).Name, defaultHeaders, defaultCookies) { }

        public async Task InvokeAsync(int userLoopCount)
        {
            for (int i = 0; i < userLoopCount; i++)
            {
                await Performance();
            }
        }

        protected abstract Task Performance();
    }
}
