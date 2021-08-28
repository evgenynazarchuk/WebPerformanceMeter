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

        public BrowserUser(string userName = "")
        {
            this.SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
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

            page.RequestFinished += (_, request) =>
            {
                Console.WriteLine($"{TimeSpan.FromMilliseconds(request.Timing.ConnectStart)} " +
                    $"{TimeSpan.FromMilliseconds(request.Timing.ConnectEnd)} " +
                    $"{TimeSpan.FromMilliseconds(request.Timing.RequestStart)} " +
                    $"{TimeSpan.FromMilliseconds(request.Timing.ResponseStart)} " +
                    $"{TimeSpan.FromMilliseconds(request.Timing.ResponseEnd)} " +
                    $"{request.Method} " +
                    $"{request.Url}");
            };

            PageContext pageContext = new(page);

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
                    await PerformanceAsync(pageContext);
                }
                else
                {
                    await PerformanceAsync(pageContext, entity);
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

        protected virtual Task PerformanceAsync(PageContext pageContext, object entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PerformanceAsync(PageContext pageContext)
        {
            return Task.CompletedTask;
        }
    }
}
