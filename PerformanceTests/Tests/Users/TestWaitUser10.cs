namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;

    public class TestWaitUser10 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent Content = new() { Timeout = 1000 };

        public TestWaitUser10(HttpClient client)
            : base(client)
        {
        }

        protected override async Task PerformanceAsync()
        {
            // Action
            await this.TestWaitMethod2(this.Content, "1000ms");
        }
    }
}
