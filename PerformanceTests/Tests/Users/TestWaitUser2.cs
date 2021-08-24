namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;

    public class TestWaitUser2 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent Content = new () { Timeout = 200 };

        public TestWaitUser2(HttpClient client)
            : base(client) 
        {
        }

        protected override async Task PerformanceAsync()
        {
            // Action
            await this.TestWaitMethod2(this.Content, "200ms");
        }
    }
}
