﻿using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;

namespace PerformanceTests.Tests.HttpClientTests
{
    public class Demo4
    {
        [PerformanceTest(1, 1000)]
        [PerformanceTest(30, 1000)]
        [PerformanceTest(30, 2000)]
        public async Task ActiveUsersOnPeriodTest(int minutes, int usersCount)
        {
            var app = new WebApplication();
            var user = new UserRequest(app.HttpClient);
            var plan = new ActiveUsersOnPeriod(user, usersCount, minutes.Minutes());

            await new Scenario()
                .AddSequentialPlans(plan)
                .StartAsync();
        }

        public class UserRequest : HttpUser
        {
            public UserRequest(HttpClient client)
                : base(client) { }

            protected override async Task PerformanceAsync()
            {
                await this.RequestAsJsonAsync(
                    HttpMethod.Post,
                    "/Test/TestWaitMethod",
                    new TestRequestContent { Timeout = 1 },
                    "1ms");
            }
        }
    }
}
