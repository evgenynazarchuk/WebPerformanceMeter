namespace WebPerformanceMeter.Tools.BrowserTool
{
    using Microsoft.Playwright;
    using System;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Logger;

    public class BrowserTool : IDisposable
    {
        public readonly IPlaywright Playwright;

        public readonly IBrowser Browser;

        public readonly IPerformanceLogger Logger;

        public readonly string UserName;

        public BrowserTool(IPerformanceLogger logger, string userName)
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

        public void Dispose()
        {
            this.Browser.CloseAsync().GetAwaiter().GetResult();
        }
    }
}
