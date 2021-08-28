namespace WebPerformanceMeter.Users.HttpBrowser
{
    using Microsoft.Playwright;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Interfaces;

    public abstract partial class BrowserRequest : PerformanceUser, IDisposable
    {
        protected readonly IPlaywright Playwright;

        protected readonly IBrowser Browser;

        protected readonly IBrowserContext BrowserContext;

        protected readonly IPage Page;

        public BrowserRequest()
        {
            this.Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            this.Browser = Playwright.Firefox.LaunchAsync(new()
            {
                FirefoxUserPrefs = new Dictionary<string, object>()
                {
                    { "network.http.max-connections", 20000 }
                },
                Headless = false
            }).GetAwaiter().GetResult();

            this.BrowserContext = Browser.NewContextAsync().GetAwaiter().GetResult();
            this.Page = this.BrowserContext.NewPageAsync().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            this.Page.CloseAsync().GetAwaiter().GetResult();
            this.Browser.CloseAsync().GetAwaiter().GetResult();
        }

        public override async Task InvokeAsync(
            int loopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            this.Page.Response += async (_, response) =>
            {
                await response.FinishedAsync();

                Console.WriteLine($"{TimeSpan.FromMilliseconds(response.Request.Timing.ConnectStart)} " +
                    $"{TimeSpan.FromMilliseconds(response.Request.Timing.ConnectEnd)} " +
                    $"{TimeSpan.FromMilliseconds(response.Request.Timing.RequestStart)} " +
                    $"{TimeSpan.FromMilliseconds(response.Request.Timing.ResponseStart)} " +
                    $"{TimeSpan.FromMilliseconds(response.Request.Timing.ResponseEnd)} " +
                    $"{response.Request.Method} " +
                    $"{response.Url}");
            };

            object? entity = null;

            if (dataReader is not null)
            {
                entity = dataReader.GetEntity();

                if (entity is null)
                {
                    return;
                }
            }

            for (int i = 0; i < loopCount; i++)
            {
                if (entity is null)
                {
                    await PerformanceAsync();
                }
                else
                {
                    await PerformanceAsync(entity);
                }

                if (dataReader is not null && !reuseDataInLoop)
                {
                    entity = dataReader.GetEntity();

                    if (entity is null)
                    {
                        return;
                    }
                }
            }
        }

        protected virtual Task PerformanceAsync(object entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PerformanceAsync()
        {
            return Task.CompletedTask;
        }
    }
}
