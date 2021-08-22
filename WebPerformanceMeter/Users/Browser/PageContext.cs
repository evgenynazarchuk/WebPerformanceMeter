using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace WebPerformanceMeter.Users
{
    public class PageContext
    {
        public readonly IPage Page;

        public PageContext(IPage page)
        {
            this.Page = page;
        }

        public async Task GotoAsync(string url)
        {
            await this.Page.GotoAsync(url);
        }

        public async Task ReloadAsync()
        {
            //await this.WaitAsync();
            await this.Page.ReloadAsync();
        }

        public async Task ClickAsync(string selector)
        {
            //await this.WaitAsync();
            await this.Page.ClickAsync(selector);
        }

        public async Task TypeAsync(string selector, string text)
        {
            //await this.WaitAsync();
            await this.Page.TypeAsync(selector, text);
        }

        public async Task PageAction(Action<IPage> action)
        {
            //await this.WaitAsync();
            action(this.Page);
        }

        //public virtual async Task WaitAsync()
        //{
        //    await Task.CompletedTask;
        //}
    }
}
