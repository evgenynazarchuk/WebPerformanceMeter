using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Tools.HttpTool;
using WebPerformanceMeter.Users;
using WebPerformanceMeter.Extensions;

namespace Tests.Tests.ActiveUserOnPeriodBase
{
    public class ActiveUserOnPeriodBaseTest
    {
        public async Task RunAsync()
        {
            var app = new WebApp();
            var user = new TestUser(app.Client);
            var plan = new ActiveUsersOnPeriod(user, 200, 1.Minutes());
            var scenario = new Scenario();

            scenario.AddPerformancePlan(plan);
            await scenario.RunAsync();
        }
    }

    // User description
    public class TestUser : HttpUser
    {
        public TestUser(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync()
        {
            // Arange
            var content1 = new TestRequestContent { Timeout = 50 };
            var content2 = new TestRequestContent { Timeout = 150 };

            // Act
            await this.TestWaitMethod(content1, "50ms");
            await this.TestWaitMethod(content2, "150ms");
        }
    }

    // Facade extension
    public static class TestUserExt
    {
        public static async Task<TestResponseContent?> TestWaitMethod(
            this HttpUser user, 
            TestRequestContent content,
            string requestLabel = "")
        {
            return await user.RequestAsJsonAsync<TestRequestContent, TestResponseContent>(
                HttpMethod.Post,
                "/Test/TestWaitMethod",
                content,
                requestLabel);
        }
    }
}
