namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;

    public class TestWaitUser9 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent Content = new () { Timeout = 900 };

        public TestWaitUser9(HttpClient client)
            : base(client) 
        {
        }

        protected override async Task PerformanceAsync()
        {
            // Action
            await this.TestWaitMethod1(this.Content, "900ms");
        }
    }
}
