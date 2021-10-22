﻿using System.Threading.Tasks;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;
using WebPerformanceMeter;
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
                .StartAsync();
        }
    }

    public class SearchGoogleUser : ChromiumUser
    {
        protected override async Task PerformanceAsync(PageContext pageContext)
        {
            await pageContext.GotoAsync("https://google.com");
            await pageContext.TypeAsync("input[name='q']", "Google");
            await pageContext.ClickAsync("li+div>center>input[name='btnK']");
        }
    }
}
