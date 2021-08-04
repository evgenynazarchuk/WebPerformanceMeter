using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;
using WebPerformanceMeter.PerformancePlans;
using WebPerformanceMeter.Scenario;
using WebPerformanceMeter.Tools.HttpTool;
using WebPerformanceMeter.Users;

namespace Tests.Tests.ConstantUsersTests
{
    // Test description
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

    // User description
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
