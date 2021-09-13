namespace WebPerformanceMeter.Tools.BrowserTool
{
    using Microsoft.Playwright;
    using System;
    using System.Threading.Tasks;

    public class BrowserTool : IDisposable
    {
        public readonly IPlaywright Playwright;

        public readonly IBrowser Browser;

        public BrowserTool()
        {
            this.Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            this.Browser = Playwright.Chromium.LaunchAsync(new()
            {
                Headless = false
            }).GetAwaiter().GetResult();
        }

        public async Task<PageContext> GetNewPageContextAsync()
        {
            IBrowserContext browserContext = await Browser.NewContextAsync();
            IPage page = await browserContext.NewPageAsync();
            return new PageContext(browserContext, page);
        }

        public void Dispose()
        {
            this.Browser.CloseAsync().GetAwaiter().GetResult();
        }
    }
}
