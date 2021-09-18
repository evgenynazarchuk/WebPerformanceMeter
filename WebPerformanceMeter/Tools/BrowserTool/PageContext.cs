namespace WebPerformanceMeter.Tools.BrowserTool
{
    using Microsoft.Playwright;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Support;
    using WebPerformanceMeter.Logger;

    public class PageContext
    {
        public readonly IPage Page;

        public readonly IBrowserContext BrowserContext;

        public readonly ILogger Logger;

        public readonly string UserName;

        public string? Url { get; private set; }

        public PageContext(IBrowserContext browserContext, IPage page, ILogger logger, string userName)
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
            this.Logger.AddMessageLog($"{this.Url},{this.UserName},{label},{start},{end}");
        }

        public async Task ReloadAsync(string label = "reload")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.ReloadAsync();
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Logger.AddMessageLog($"{this.Url},{this.UserName},{label},{start},{end}");
        }

        public async Task ClickAsync(string selector, string label = "click")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.ClickAsync(selector);
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Logger.AddMessageLog($"{this.Url},{this.UserName},{label},{start},{end}");
        }

        public async Task TypeAsync(string selector, string text, string label = "type")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.TypeAsync(selector, text);
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Logger.AddMessageLog($"{this.Url},{this.UserName},{label},{start},{end}");
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
