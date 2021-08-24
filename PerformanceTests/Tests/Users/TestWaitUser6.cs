namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;

    public class TestWaitUser6 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent Content = new () { Timeout = 600 };

        public TestWaitUser6(HttpClient client)
            : base(client) 
        {
        }

        protected override async Task PerformanceAsync()
        {
            // Action
            await this.TestWaitMethod2(this.Content, "600ms");
        }
    }
}
