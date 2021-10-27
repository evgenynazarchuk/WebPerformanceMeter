﻿using Microsoft.Playwright;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Report;
using WebPerformanceMeter.Support;

namespace WebPerformanceMeter.Tools
{
    public class PageTool
    {
        public readonly IPage Page;

        public readonly IBrowserContext BrowserContext;

        public readonly ILogger? Logger;

        public readonly string UserName;

        public string? Url { get; private set; }

        public PageTool(IBrowserContext browserContext, IPage page, string userName, ILogger? logger = null)
        {
            this.BrowserContext = browserContext;
            this.Page = page;

            this.Logger = logger;
            this.UserName = userName;
        }

        public async Task OpenPage(string url, string label = "goto")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.GotoAsync(url);
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Url = url;

            if (this.Logger is not null)
            {
                this.Logger.SendLogMessage("UserActionLog.json", $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}", typeof(ChromiumLogMessage));
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
                this.Logger.SendLogMessage("UserActionLog.json", $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}", typeof(ChromiumLogMessage));
            }
        }

        public async Task Click(string selector, string label = "click")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.ClickAsync(selector);
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Url = this.Page.Url;

            if (this.Logger is not null)
            {
                this.Logger.SendLogMessage("UserActionLog.json", $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}", typeof(ChromiumLogMessage));
            }
        }

        public async Task Type(string selector, string text, string label = "type")
        {
            var start = ScenarioTimer.Time.Elapsed.Ticks;
            await this.Page.TypeAsync(selector, text);
            await this.WaitAsync();
            var end = ScenarioTimer.Time.Elapsed.Ticks;

            this.Url = this.Page.Url;

            if (this.Logger is not null)
            {
                this.Logger.SendLogMessage("UserActionLog.json", $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}", typeof(ChromiumLogMessage));
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
