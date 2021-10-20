using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Logger.BrowserLog;
using WebPerformanceMeter.Tools.BrowserTool;

namespace WebPerformanceMeter.Users
{
    public abstract class ChromiumUser : User, IDisposable
    {
        protected readonly BrowserTool BrowserTool;

        public ChromiumUser(ILogger? logger = null, string userName = "")
            : base(logger ?? BrowserLoggerSingleton.GetInstance())
        {
            this.SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
            this.BrowserTool = new(this.Logger, this.UserName);
        }

        public void Dispose()
        {
            this.BrowserTool.Dispose();
        }

        public override async Task InvokeAsync(
            int loopCount = 1,
            IDataReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            var pageContext = await this.BrowserTool.GetNewPageContextAsync();

            pageContext.Page.RequestFinished += (_, request) =>
            {
                this.Logger.AppendLogMessage("PageRequestLog.json",
                    $"{this.UserName}\t" +
                    $"{request.Method}\t" +
                    $"{request.Url}\t" + // how to parse url, error parse csv
                    $"{TimeSpan.FromMilliseconds(request.Timing.RequestStart)}\t" +
                    $"{TimeSpan.FromMilliseconds(request.Timing.ResponseStart)}\t" +
                    $"{TimeSpan.FromMilliseconds(request.Timing.ResponseEnd)}",
                    typeof(PageRequestLogMessage));
            };

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
