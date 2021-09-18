﻿namespace PerformanceTests.Tests.BrowserTests
{
    using PerformanceTests.Tests.Users;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Attributes;
    using WebPerformanceMeter.Extensions;
    using WebPerformanceMeter.PerformancePlans;
    using WebPerformanceMeter.Support;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using WebPerformanceMeter.Users;
    using WebPerformanceMeter.Tools.BrowserTool;

    public class GoGooleSearch
    {
        [PerformanceTest(1, 5)]
        [PerformanceTest(10, 5)]
        [PerformanceTest(30, 5)]
        [PerformanceTest(60, 10)]
        [PerformanceTest(90, 10)]
        public async Task GoSearch(int minutes, int usersCount)
        {
            var user = new SearchGoogleUser();
            var plan = new ActiveUsersOnPeriod(user, usersCount, minutes.Minutes());

            await new Scenario()
                .AddSequentialPlans(plan)
                .StartAsync();
        }
    }

    public class SearchGoogleUser : BrowserUser
    {
        protected override async Task PerformanceAsync(PageContext pageContext)
        {
            await pageContext.GotoAsync("https://google.com");
            await pageContext.TypeAsync("input[name='q']", "Google");
            await pageContext.ClickAsync("li+div>center>input[name='btnK']");
        }
    }
}
