using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.HttpClientTests
{
    public class Demo8
    {
        [PerformanceTest(1, 100, 200)]
        [PerformanceTest(30, 100, 1000)]
        public async Task ActiveUsersByStepsTest(int minutes, int fromUsersCount, int toUsersCount)
        {
            var app = new WebApplication();
            var user = new UserRequest(app.HttpClient);
            var plan = new ActiveUsersBySteps(user, fromUsersCount, toUsersCount, usersStep: 100, performancePlanDuration: minutes.Minutes());

            await new Scenario()
                .AddSequentialPlans(plan)
                .Start();
        }

        public class UserRequest : HttpUser
        {
            public UserRequest(HttpClient client)
                : base(client) { }

            protected override async Task Performance()
            {
                await this.RequestAsJson(
                    HttpMethod.Post,
                    "/Test/TestWaitMethod",
                    new TestRequestContent { Timeout = 50 },
                    "100ms");
            }
        }
    }
}
