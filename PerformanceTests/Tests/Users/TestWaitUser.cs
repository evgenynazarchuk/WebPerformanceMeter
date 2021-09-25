namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;
    using WebPerformanceMeter.Logger;

    public class TestWaitUser : TestUserFacade
    {
        public TestWaitUser(HttpClient client, ILogger? logger = null)
            : base(client, logger) { }

        protected override async Task PerformanceAsync()
        {
            await this.TestWaitMethod(new TestRequestContent { Timeout = 100 }, "100ms");
        }
    }
}
