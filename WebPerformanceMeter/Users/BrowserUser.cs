using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using Microsoft.Playwright;

namespace WebPerformanceMeter.Users
{
    public abstract class BrowserUser : PerformanceUser
    {
        protected readonly IPlaywright Playwright;

        protected readonly IBrowser Browser;

        public BrowserUser()
        {
            SetUserName(this.GetType().Name);
            Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
            Browser = Playwright.Chromium.LaunchAsync(new()
            {
                Headless = false
            }).GetAwaiter().GetResult();
        }

        public override async Task InvokeAsync(
            int loopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            IBrowserContext ctx = await Browser.NewContextAsync();
            IPage page = await ctx.NewPageAsync();
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
                    await PerformanceAsync(page);
                }
                else
                {
                    await PerformanceAsync(page, entity);
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
        }

        protected virtual Task PerformanceAsync(IPage page, object entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PerformanceAsync(IPage page)
        {
            return Task.CompletedTask;
        }
    }
}
