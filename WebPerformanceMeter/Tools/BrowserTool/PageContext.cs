using Microsoft.Playwright;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Tools
{
    public class PageContext
    {
        public readonly IPage Page;

        public readonly IBrowserContext BrowserContext;

        public readonly ILogger? Logger;

        public readonly string UserName;

        public string? Url { get; private set; }

        public PageContext(IBrowserContext browserContext, IPage page, string userName, ILogger? logger = null)
        {
            this.BrowserContext = browserContext;
            this.Page = page;

            this.Logger = logger;
            this.UserName = userName;
        }

        public async Task GotoAsync(string url, string label = "goto")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.GotoAsync(url);
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Url = url;

            if (this.Logger is not null)
            {
                this.Logger.AddLogMessage("UserActionLog.json", $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}", typeof(ChromiumActionLogMessage));
            }         
        }

        public async Task ReloadAsync(string label = "reload")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.ReloadAsync();
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Url = this.Page.Url;

            if (this.Logger is not null)
            {
                this.Logger.AddLogMessage("UserActionLog.json", $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}", typeof(ChromiumActionLogMessage));
            }
        }

        public async Task ClickAsync(string selector, string label = "click")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.ClickAsync(selector);
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Url = this.Page.Url;

            if (this.Logger is not null)
            {
                this.Logger.AddLogMessage("UserActionLog.json", $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}", typeof(ChromiumActionLogMessage));
            }            
        }

        public async Task TypeAsync(string selector, string text, string label = "type")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.TypeAsync(selector, text);
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Url = this.Page.Url;

            if (this.Logger is not null)
            {
                this.Logger.AddLogMessage("UserActionLog.json", $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}", typeof(ChromiumActionLogMessage));
            }
        }

        public async Task WaitAsync()
        {
            await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task CloseAsync()
        {
            await this.Page.CloseAsync();
            await this.BrowserContext.CloseAsync();
        }
    }
}
