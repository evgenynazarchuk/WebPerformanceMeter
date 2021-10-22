using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Tools;

namespace WebPerformanceMeter
{
    public class BrowserTool : Tool, IDisposable
    {
        public readonly IPlaywright Playwright;

        public readonly IBrowser Browser;

        public readonly string UserName;

        public BrowserTool(string userName, ILogger? logger = null)
            : base(logger)
        {
            this.Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            this.Browser = Playwright.Chromium.LaunchAsync(new()
            {
                Headless = true
            }).GetAwaiter().GetResult();
            this.UserName = userName;
        }

        public async Task<PageContext> GetNewPageContextAsync()
        {
            IBrowserContext browserContext = await Browser.NewContextAsync();
            IPage page = await browserContext.NewPageAsync();

            return new PageContext(browserContext, page, this.UserName, this.logger);
        }

        public void Dispose()
        {
            this.Browser.CloseAsync().GetAwaiter().GetResult();
        }
    }
}
