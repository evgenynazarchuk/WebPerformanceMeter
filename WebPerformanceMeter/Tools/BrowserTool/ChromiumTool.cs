using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Tools;
using WebPerformanceMeter.Reports;

namespace WebPerformanceMeter
{
    public class ChromiumTool : Tool, IAsyncDisposable
    {
        public readonly IPlaywright Playwright;

        public readonly IBrowser Browser;

        public readonly string UserName;

        public ChromiumTool(string userName, Watcher watcher)
            : base(watcher)
        {
            this.Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            this.Browser = Playwright.Chromium.LaunchAsync(new()
            {
                Headless = true
            }).GetAwaiter().GetResult();
            this.UserName = userName;
        }

        public ChromiumTool(IPlaywright playwright, IBrowser browser, string userName, Watcher watcher)
            : base(watcher)
        {
            this.Playwright = playwright;
            this.Browser = browser;
            this.UserName = userName;
        }

        public async Task<PageTool> GetPageTool()
        {
            IBrowserContext browserContext = await Browser.NewContextAsync();
            IPage page = await browserContext.NewPageAsync();

            return new PageTool(browserContext, page, this.UserName, this.Watcher);
        }

        public static Task<IBrowserContext> GetNewContextAsync(IBrowser browser)
        {
            return browser.NewContextAsync();
        }

        public static Task<IPage> GetNewPageAsync(IBrowserContext browserContext)
        {
            return browserContext.NewPageAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await this.Browser.CloseAsync();
        }
    }
}
