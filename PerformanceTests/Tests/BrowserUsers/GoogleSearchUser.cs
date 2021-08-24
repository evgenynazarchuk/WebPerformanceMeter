namespace PerformanceTests.Tests.BrowserUsers
{
    using System.Threading.Tasks;
    using WebPerformanceMeter.Users;

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
