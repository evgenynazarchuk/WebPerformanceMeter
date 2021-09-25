namespace PerformanceTests.Tests.HttpClientTests
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;
    using WebPerformanceMeter.Attributes;
    using WebPerformanceMeter.Extensions;
    using WebPerformanceMeter.PerformancePlans;
    using WebPerformanceMeter.Support;
    using WebPerformanceMeter.Users;

    public class PostRequestWithTimeout100ms
    {
        [PerformanceTest(1, 200)]
        [PerformanceTest(10, 200)]
        [PerformanceTest(20, 200)]
        [PerformanceTest(60, 200)]
        [PerformanceTest(90, 200)]
        public async Task Test(int minutes, int usersCount)
        {
            var app = new WebApplication();
            var user = new UserRequest(app.HttpClient);
            var plan = new ActiveUsersOnPeriod(user, usersCount, minutes.Minutes());

            await new Scenario()
                .AddSequentialPlans(plan)
                .StartAsync();
        }

        public class UserRequest : HttpClientUser
        {
            public UserRequest(HttpClient client)
                : base(client) { }

            protected override async Task PerformanceAsync()
            {
                await this.RequestAsJsonAsync(
                    HttpMethod.Post,
                    "/Test/TestWaitMethod",
                    new TestRequestContent { Timeout = 100 },
                    "100ms");
            }
        }
    }
}
