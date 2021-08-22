using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using System.Net.Http;
using TestWebApiServer.Models;
using WebPerformanceMeter.Users;

namespace PerformanceTests.Tests.BrowserUsers
{
    public class GoogleSearchUser : BrowserUser
    {
        protected override async Task PerformanceAsync(PageContext page)
        {
            await page.GotoAsync("https://google.com");
            await page.TypeAsync("//input[@name='q']", "Google");
            await page.ClickAsync("//div[@class='lJ9FBc']//input[@name='btnK']");
        }
    }
}
