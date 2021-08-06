using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Tools.HttpTool;
using WebPerformanceMeter.Users;

namespace Tests.Tests.UserOnPeriodBase
{
    public class UserOnPeriodBaseTest
    {
        public async Task RunAsync()
        {
            var app = new WebApp();
            var user = new TestUser(app.Client);
            var plan = new UsersOnPeriod(user, 180000, TimeSpan.FromMinutes(30));
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
            var content = new TestRequestContent { Timeout = 200 };

            // Act
            await Tool.TestWaitMethod(content);
        }
    }

    // Facade extension
    public static class TestUserExt
    {
        public static async Task<TestResponseContent?> TestWaitMethod(this HttpTool tool, TestRequestContent content)
        {
            return await tool.RequestAsJsonAsync<TestRequestContent, TestResponseContent>(
                HttpMethod.Post,
                "/Test/TestWaitMethod",
                content);
        }
    }
}
