using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.HttpClientTests
{
    public class Demo3
    {
        [PerformanceTest(1, 250, 500, 1000)]
        [PerformanceTest(10, 250, 500, 1000)]
        public async Task ActiveUsersOnPeriodTest(
            int minutesPerPeriod,
            int firstPeriodUsersCount,
            int secondPeriodUsersCount,
            int thirdPeriodUsersCount
            )
        {
            // webapi
            var app = new WebApplication();

            //
            var user1 = new User1(app.HttpClient);
            var user2 = new User2(app.HttpClient);

            // first period
            var userPlan1 = new ActiveUsersOnPeriod(user1, firstPeriodUsersCount, minutesPerPeriod.Minutes());
            var userPlan2 = new ActiveUsersOnPeriod(user2, firstPeriodUsersCount, minutesPerPeriod.Minutes());

            // second period
            var userPlan3 = new ActiveUsersOnPeriod(user1, secondPeriodUsersCount, minutesPerPeriod.Minutes());
            var userPlan4 = new ActiveUsersOnPeriod(user2, secondPeriodUsersCount, minutesPerPeriod.Minutes());

            // third period
            var userPlan5 = new ActiveUsersOnPeriod(user1, thirdPeriodUsersCount, minutesPerPeriod.Minutes());
            var userPlan6 = new ActiveUsersOnPeriod(user2, thirdPeriodUsersCount, minutesPerPeriod.Minutes());

            await new Scenario()
                .AddParallelPlans(userPlan1, userPlan2)
                .AddParallelPlans(userPlan3, userPlan4)
                .AddParallelPlans(userPlan5, userPlan6)
                .Start();
        }

        public class User1 : HttpUser
        {
            public User1(HttpClient client)
                : base(client) { }

            protected override async Task Performance()
            {
                await RequestAsJson(
                    HttpMethod.Post,
                    "/Test/TestWaitMethod",
                    new TestRequestContent { Timeout = 100 },
                    "100ms");
            }
        }

        public class User2 : HttpUser
        {
            public User2(HttpClient client)
                : base(client) { }

            protected override async Task Performance()
            {

                await RequestAsJson(
                    HttpMethod.Post,
                    "/Test/TestWaitMethod",
                    new TestRequestContent { Timeout = 200 },
                    "200ms");
            }
        }
    }
}
