namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;

    public class TestWaitUser8 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent Content = new () { Timeout = 800 };

        public TestWaitUser8(HttpClient client)
            : base(client) 
        {
        }

        protected override async Task PerformanceAsync()
        {
            // Action
            await this.TestWaitMethod2(this.Content, "800ms");
        }
    }
}
