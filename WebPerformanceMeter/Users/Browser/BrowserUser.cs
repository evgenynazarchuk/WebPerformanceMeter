﻿namespace WebPerformanceMeter.Users
{
    using System;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Interfaces;
    using WebPerformanceMeter.Logger;
    using WebPerformanceMeter.Tools.BrowserTool;

    public abstract class BrowserUser : User, IDisposable
    {
        protected readonly BrowserTool BrowserTool;

        public BrowserUser(ILogger logger, string userName = "")
            : base(logger)
        {
            this.SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
            this.BrowserTool = new();
        }

        public void Dispose()
        {
            this.BrowserTool.Dispose();
        }

        public override async Task InvokeAsync(
            int loopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            var pageContext = await this.BrowserTool.GetNewPageContextAsync();

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
                    await PerformanceAsync(pageContext);
                }
                else
                {
                    await PerformanceAsync(pageContext, entity);
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

            await pageContext.CloseAsync();
        }

        protected virtual Task PerformanceAsync(PageContext pageContext, object entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PerformanceAsync(PageContext pageContext)
        {
            return Task.CompletedTask;
        }
    }
}
