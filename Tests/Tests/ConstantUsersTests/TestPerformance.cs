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

namespace Tests.Tests.ConstantUsersTests
{
    public class TestPerformance
    {
        public async Task RunAsync()
        {
            var app = new WebApp();
            var user = new TestUser(app.Client);
            var plan = new ConstantUsers(user, 10);
            var scenario = new Scenario();

            scenario.AddPerformancePlan(plan);
            await scenario.RunAsync();
        }
    }

    public class TestUser : HttpUser
    {
        public TestUser(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync()
        {
            // Arange
            var content = new TestRequestContent { Timeout = 2000 };

            // Act
            await Tool.TestWaitMethod(content);
        }
    }

    public static class TestUserExt
    {
        public static async Task<TestResponseContent?> TestWaitMethod(this HttpTool tool, TestRequestContent content)
        {
            return await tool.RequestAsJsonAsync<TestResponseContent, TestRequestContent>(
                HttpMethod.Post,
                "/Test/TestWaitMethod",
                content);
        }
    }
}
