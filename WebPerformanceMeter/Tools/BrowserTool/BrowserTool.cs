﻿namespace WebPerformanceMeter.Tools.BrowserTool
{
    using Microsoft.Playwright;
    using System;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Logger;

    public class BrowserTool : IDisposable
    {
        public readonly IPlaywright Playwright;

        public readonly IBrowser Browser;

        public readonly ILogger Logger;

        public readonly string UserName;

        public BrowserTool(ILogger logger, string userName)
        {
            this.Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            this.Browser = Playwright.Chromium.LaunchAsync(new()
            {
                Headless = false
            }).GetAwaiter().GetResult();

            this.Logger = logger;
            this.UserName = userName;
        }

        public async Task<PageContext> GetNewPageContextAsync()
        {
            IBrowserContext browserContext = await Browser.NewContextAsync();
            IPage page = await browserContext.NewPageAsync();

            return new PageContext(browserContext, page, this.Logger, this.UserName);
        }

        //async ValueTask IAsyncDisposable.DisposeAsync()
        //{
        //    await this.Browser.CloseAsync();
        //}

        public void Dispose()
        {
            this.Browser.CloseAsync().GetAwaiter().GetResult();
        }
    }
}
