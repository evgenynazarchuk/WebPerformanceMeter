using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Extensions;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.HttpClientTests
{
    public class Demo9
    {
        [PerformanceTest(10, 100, 200)]
        public async Task ActiveUsersByStepsTest(int minutes, int fromUsersCount, int toUsersCount)
        {
            var app = new TestApplication();
            var user = new UserRequest(app.HttpClient);
            var plan = new ActiveUsersBySteps(user, fromUsersCount, toUsersCount, usersStep: 3, stepPeriodDuration: minutes.Minutes());

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
                    "50ms");
            }
        }
    }
}
