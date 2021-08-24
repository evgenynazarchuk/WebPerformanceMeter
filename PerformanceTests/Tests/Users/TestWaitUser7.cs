namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;

    public class TestWaitUser7 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent Content = new () { Timeout = 700 };

        public TestWaitUser7(HttpClient client)
            : base(client) 
        {
        }

        protected override async Task PerformanceAsync()
        {
            // Action
            await this.TestWaitMethod1(this.Content, "700ms");
        }
    }
}
