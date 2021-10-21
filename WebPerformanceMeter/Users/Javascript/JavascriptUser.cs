using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;

namespace WebPerformanceMeter.Users.Javascript
{
    public abstract partial class JavascriptUser : BaseUser, IDisposable
    {
        protected readonly IPlaywright Playwright;

        protected readonly IBrowser Browser;

        protected readonly IBrowserContext BrowserContext;

        protected readonly IPage Page;

        public JavascriptUser()
            : base()
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

        public override async Task InvokeAsync(
            int loopCount = 1,
            IDataReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            ////this.Page.RequestFinished += (_, request) =>
            ////{
            ////    Console.WriteLine($"{TimeSpan.FromMilliseconds(request.Timing.ConnectStart)} " +
            ////        $"{TimeSpan.FromMilliseconds(request.Timing.ConnectEnd)} " +
            ////        $"{TimeSpan.FromMilliseconds(request.Timing.RequestStart)} " +
            ////        $"{TimeSpan.FromMilliseconds(request.Timing.ResponseStart)} " +
            ////        $"{TimeSpan.FromMilliseconds(request.Timing.ResponseEnd)} " +
            ////        $"{request.Method} " +
            ////        $"{request.Url}");
            ////};

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
