﻿using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Users
{
    using System;
    using System.Net.Http;
    using WebPerformanceMeter.Tools.HttpTool;

    public abstract partial class HttpClientUser : User
    {
        protected readonly HttpClient Client;

        protected readonly HttpTool Tool;

        public HttpClientUser(HttpClient client, string userName = "")
        {
            this.Client = client;
            this.Tool = new(Client);
            SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
        }

        public HttpClientUser(string host, string userName = "")
        {
            this.Client = new HttpClient()
            {
                BaseAddress = new Uri(host)
            };
            this.Tool = new(this.Client);
            SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
        }

        public override async Task InvokeAsync(
            int loopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            object? entity = null;

            if (dataReader is not null)
            {
                entity = dataReader.GetEntity();

                if (entity is null)
                {
                    return;
                }
            }

            for (int i = 0; i < loopCount; i++)
            {
                if (entity is null)
                {
                    await PerformanceAsync();
                }
                else
                {
                    await PerformanceAsync(entity);
                }

                if (dataReader is not null && !reuseDataInLoop)
                {
                    entity = dataReader.GetEntity();

                    if (entity is null)
                    {
                        return;
                    }
                }
            }
        }

        protected virtual Task PerformanceAsync(object entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PerformanceAsync()
        {
            return Task.CompletedTask;
        }
    }
}