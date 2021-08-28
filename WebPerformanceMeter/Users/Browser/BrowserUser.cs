namespace WebPerformanceMeter.Users
{
    using Microsoft.Playwright;
    using System;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Interfaces;

    public abstract class BrowserUser : User, IDisposable
    {
        protected readonly IPlaywright Playwright;

        protected readonly IBrowser Browser;

        public BrowserUser()
        {
            this.SetUserName(this.GetType().Name);
            this.Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            this.Browser = Playwright.Chromium.LaunchAsync(new ()
            {
                Headless = true
            }).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            this.Browser.CloseAsync().GetAwaiter().GetResult();
        }

        public override async Task InvokeAsync(
            int loopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            IBrowserContext browserContext = await Browser.NewContextAsync();
            IPage page = await browserContext.NewPageAsync();

            page.Response += async (_, response) =>
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

            PageContext pageCtx = new(page);

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
                    await PerformanceAsync(pageCtx);
                }
                else
                {
                    await PerformanceAsync(pageCtx, entity);
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

            await page.CloseAsync();
            await browserContext.CloseAsync();
        }

        protected virtual Task PerformanceAsync(PageContext page, object entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PerformanceAsync(PageContext page)
        {
            return Task.CompletedTask;
        }
    }
}
