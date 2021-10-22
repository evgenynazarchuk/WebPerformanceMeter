using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicJavascriptUser : BaseUser, IDisposable
    {
        protected readonly IPlaywright Playwright;

        protected readonly IBrowser Browser;

        protected readonly IBrowserContext BrowserContext;

        protected readonly IPage Page;

        public BasicJavascriptUser(string userName = "", ILogger? logger = null)
            : base(userName, logger)
        {
            this.Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            this.Browser = Playwright.Firefox.LaunchAsync(new()
            {
                FirefoxUserPrefs = new Dictionary<string, object>()
                {
                    { "network.http.max-connections", 20000 }
                },
                Headless = true
            }).GetAwaiter().GetResult();

            this.BrowserContext = Browser.NewContextAsync().GetAwaiter().GetResult();
            this.Page = this.BrowserContext.NewPageAsync().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            this.Page.CloseAsync().GetAwaiter().GetResult();
            this.Browser.CloseAsync().GetAwaiter().GetResult();
        }
    }
}
