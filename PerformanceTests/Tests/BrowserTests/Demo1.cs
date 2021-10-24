using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Tools;

namespace PerformanceTests.Tests.BrowserTests
{
    public class Demo1
    {
        [PerformanceTest(1, 5)]
        [PerformanceTest(10, 5)]
        public async Task GoSearch(int minutes, int usersCount)
        {
            var user = new SearchGoogleUser();
            var plan = new ActiveUsersOnPeriod(user, usersCount, minutes.Minutes());

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }
    }

    public class SearchGoogleUser : ChromiumUser
    {
        protected override async Task Performance(PageTool tool)
        {
            await tool.OpenPage("https://google.com");
            await tool.Type("input[name='q']", "Google");
            await tool.Click("li+div>center>input[name='btnK']");
        }
    }
}
