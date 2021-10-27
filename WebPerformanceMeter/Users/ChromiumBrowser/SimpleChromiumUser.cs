using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Tools;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract class ChromiumUser : BasicChromiumUser, ISimpleUser, IDisposable
    {
        public ChromiumUser(string? userName = null, ILogger? logger = null)
            : base(userName, logger) { }

        public virtual async Task InvokeAsync(int userLoopCount = 1)
        {
            var pageContext = await this.BrowserTool.GetPageTool();

            pageContext.Page.RequestFinished += (_, request) =>
            {
                if (this.logger is not null)
                {
                    this.logger.AddLogMessage("PageRequestLog.json",
                        $"{this.UserName}\t" +
                        $"{request.Method}\t" +
                        $"{request.Url}\t" + // how to parse url, error parse csv
                        $"{TimeSpan.FromMilliseconds(request.Timing.RequestStart)}\t" +
                        $"{TimeSpan.FromMilliseconds(request.Timing.ResponseStart)}\t" +
                        $"{TimeSpan.FromMilliseconds(request.Timing.ResponseEnd)}",
                        typeof(ChromiumPageRequestLogMessage));
                }
            };

            for (int i = 0; i < userLoopCount; i++)
            {
                await Performance(pageContext);
            }

            await pageContext.CloseAsync();
        }

        protected abstract Task Performance(PageTool pageContext);
    }
}
