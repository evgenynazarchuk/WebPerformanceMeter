namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;
    using WebPerformanceMeter.Users;

    public class TestUserFacade : HttpUser
    {
        public TestUserFacade(HttpClient client, string userName = "")
            : base(client, userName)
        {
        }

        public async Task<TestResponseContent?> TestWaitMethod1(
            TestRequestContent content,
            string requestLabel = "")
        {
            return await this.RequestAsJsonAsync<TestRequestContent, TestResponseContent>(
                HttpMethod.Post,
                "/Test/TestWaitMethod1",
                content,
                requestLabel);
        }

        public async Task<TestResponseContent?> TestWaitMethod2(
            TestRequestContent content,
            string requestLabel = "")
        {
            return await this.RequestAsJsonAsync<TestRequestContent, TestResponseContent>(
                HttpMethod.Post,
                "/Test/TestWaitMethod2",
                content,
                requestLabel);
        }
    }
}
