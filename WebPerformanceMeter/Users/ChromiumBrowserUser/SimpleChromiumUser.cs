using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Reports;
using WebPerformanceMeter.Tools;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class ChromiumUser : BasicChromiumUser, ISimpleUser, IDisposable
    {
        public ChromiumUser(string? userName = null)
            : base(userName ?? typeof(ChromiumUser).Name) { }

        public async Task InvokeAsync(int userLoopCount = 1)
        {
            var pageContext = await this.BrowserTool.GetPageTool();

            pageContext.Page.RequestFinished += (_, request) =>
            {
                if (this.Watcher is not null)
                {
                    this.Watcher.SendMessage("PageRequestLog.json",
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
