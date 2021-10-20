using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Tools.HttpTool;

namespace WebPerformanceMeter.Users
{
    public abstract partial class HttpUser : User
    {
        protected readonly HttpClient Client;

        protected readonly HttpTool Tool;

        public HttpUser(HttpClient client, ILogger? logger = null, string userName = "")
            : base(logger ?? HttpLoggerSingleton.GetInstance())
        {
            this.Client = client;
            this.Tool = new(this.Logger, this.Client);

            this.SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
        }

        public HttpUser(string host, ILogger logger, string userName = "")
            : base(logger)
        {
            this.Client = new HttpClient() { BaseAddress = new Uri(host) };
            this.Tool = new(this.Logger, this.Client);

            this.SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
        }

        public override async Task InvokeAsync(
            int loopCount = 1,
            IDataReader? dataReader = null,
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
