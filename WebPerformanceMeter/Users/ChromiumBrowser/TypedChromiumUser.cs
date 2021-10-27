﻿using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Tools;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract class ChromiumUser<TData> : BasicChromiumUser, ITypedUser<TData>, IDisposable
        where TData : class
    {
        public ChromiumUser(string? userName = null, ILogger? logger = null)
            : base(userName, logger) { }

        public virtual async Task InvokeAsync(
            IDataReader<TData> dataReader,
            bool reuseDataInLoop = true,
            int userLoopCount = 1
            )
        {
            var pageContext = await this.BrowserTool.GetPageTool();

            pageContext.Page.RequestFinished += (_, request) =>
            {
                if (this.logger is not null)
                {
                    this.logger.SendLogMessage("PageRequestLog.json",
                        $"{this.UserName}\t" +
                        $"{request.Method}\t" +
                        $"{request.Url}\t" + // how to parse url, error parse csv
                        $"{TimeSpan.FromMilliseconds(request.Timing.RequestStart)}\t" +
                        $"{TimeSpan.FromMilliseconds(request.Timing.ResponseStart)}\t" +
                        $"{TimeSpan.FromMilliseconds(request.Timing.ResponseEnd)}",
                        typeof(ChromiumPageRequestLogMessage));
                }
            };

            TData? data = dataReader.GetData();

            for (int i = 0; i < userLoopCount; i++)
            {
                if (data is null)
                {
                    continue;
                }

                await PerformanceAsync(pageContext, data);

                if (!reuseDataInLoop)
                {
                    data = dataReader.GetData();
                }
            }

            await pageContext.CloseAsync();
        }

        protected abstract Task PerformanceAsync(PageTool pageContext, TData data);
    }
}
