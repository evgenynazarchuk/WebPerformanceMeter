using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Scenario;
using WebPerformanceMeter;
using WebPerformanceMeter.Users;
using System.Net.Http;
using WebPerformanceMeter.Tools.HttpTool;
using TestWebApiServer.Models;

namespace Tests.Tests.ActiveUserOnPeriodBase
{
    public class ActiveUserOnPeriodBaseTest
    {
        public async Task RunAsync()
        {
            var app = new WebApp();
            var user = new TestUser(app.Client);
            var plan = new ActiveUsersOnPeriod(user, 10, TimeSpan.FromSeconds(5));
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
