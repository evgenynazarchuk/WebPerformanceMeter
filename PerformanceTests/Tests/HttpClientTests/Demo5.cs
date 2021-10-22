using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;
using WebPerformanceMeter;
using WebPerformanceMeter.Attributes;
using WebPerformanceMeter.Support;

namespace PerformanceTests.Tests.HttpClientTests
{
    public class Demo5
    {
        [PerformanceTest(200)]
        [PerformanceTest(1000)]
        public async Task ConstantUsersTest(int usersCount)
        {
            var app = new WebApplication();
            var user = new UserRequest(app.HttpClient);
            var plan = new ConstantUsers(user, usersCount);

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
                    new TestRequestContent { Timeout = 100 },
                    "100ms");
            }
        }
    }
}
