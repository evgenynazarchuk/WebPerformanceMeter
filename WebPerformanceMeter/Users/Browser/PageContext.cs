namespace WebPerformanceMeter.Users
{
    using Microsoft.Playwright;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class PageContext
    {
        public readonly IPage Page;

        public PageContext(IPage page)
        {
            this.Page = page;
        }

        public async Task GotoAsync(string url, string label = "goto")
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await this.Page.GotoAsync(url);
            await this.WaitAsync();
            stopWatch.Stop();
            Console.WriteLine($"{label} {stopWatch.Elapsed}");
        }

        public async Task ReloadAsync(string label = "reload")
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await this.Page.ReloadAsync();
            await this.WaitAsync();
            stopWatch.Stop();
            Console.WriteLine($"{label} {stopWatch.Elapsed}");
        }

        public async Task ClickAsync(string selector, string label = "click")
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await this.Page.ClickAsync(selector);
            await this.WaitAsync();
            stopWatch.Stop();
            Console.WriteLine($"{label} {stopWatch.Elapsed}");
        }

        public async Task TypeAsync(string selector, string text, string label = "type")
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await this.Page.TypeAsync(selector, text);
            await this.WaitAsync();
            stopWatch.Stop();
            Console.WriteLine($"{label} {stopWatch.Elapsed}");
        }

        public async Task WaitAsync()
        {
            await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}
