using Microsoft.Playwright;
using System;
using System.Collections.Generic;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicJavascriptUser : BasicUser, IDisposable
    {
        public BasicJavascriptUser(string userName)
            : base(userName)
        {
            this.playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            this.browser = playwright.Firefox.LaunchAsync(new()
            {
                FirefoxUserPrefs = new Dictionary<string, object>()
                {
                    { "network.http.max-connections", 20000 }
                },
                Headless = true
            }).GetAwaiter().GetResult();

            this.browserContext = browser.NewContextAsync().GetAwaiter().GetResult();
            this.page = this.browserContext.NewPageAsync().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            this.page.CloseAsync().GetAwaiter().GetResult();
            this.browser.CloseAsync().GetAwaiter().GetResult();
        }

        protected readonly IPlaywright playwright;

        protected readonly IBrowser browser;

        protected readonly IBrowserContext browserContext;

        protected readonly IPage page;
    }
}
