using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract class HttpUser<TData> : BasicHttpUser, ITypedUser<TData>
        where TData : class
    {
        public HttpUser(HttpClient client, string userName = "", ILogger? logger = null)
            : base(client, userName, logger) { }

        public HttpUser(
            string address,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null,
            string userName = "",
            ILogger? logger = null)
            : base(address, defaultHeaders, defaultCookies, userName, logger) { }

        public virtual async Task InvokeAsync(
            IDataReader<TData> dataReader,
            bool reuseDataInLoop = true,
            int userloopCount = 1
            )
        {
            TData? data = dataReader.GetData();

            for (int i = 0; i < userloopCount; i++)
            {
                if (data is null)
                {
                    continue;
                }

                await PerformanceAsync(data);

                if (!reuseDataInLoop)
                {
                    data = dataReader.GetData();
                }
            }
        }

        public abstract Task PerformanceAsync(TData data);
    }
}
