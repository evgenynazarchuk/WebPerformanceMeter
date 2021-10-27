﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class HttpUser<TData> : BasicHttpUser, ITypedUser<TData>
        where TData : class
    {
        public HttpUser(HttpClient client, string? userName = null)
            : base(client, userName ?? typeof(HttpUser<TData>).Name) { }

        public HttpUser(
            string address,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null,
            string? userName = null)
            : base(address, userName ?? typeof(HttpUser<TData>).Name, defaultHeaders, defaultCookies) { }

        public async Task InvokeAsync(
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

                await Performance(data);

                if (!reuseDataInLoop)
                {
                    data = dataReader.GetData();
                }
            }
        }

        protected abstract Task Performance(TData data);
    }
}
