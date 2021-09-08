namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;
    using WebPerformanceMeter.Users;
    using WebPerformanceMeter.Logger;

    public class TestUserFacade : HttpClientUser
    {
        public TestUserFacade(HttpClient client, ILogger? logger = null, string userName = "")
            : base(client, logger, userName) { }

        public async Task<TestResponseContent?> TestWaitMethod(
            TestRequestContent content,
            string requestLabel = "")
        {
            return await this.RequestAsJsonAsync<TestRequestContent, TestResponseContent>(
                HttpMethod.Post,
                "/Test/TestWaitMethod",
                content,
                requestLabel);
        }
    }
}
