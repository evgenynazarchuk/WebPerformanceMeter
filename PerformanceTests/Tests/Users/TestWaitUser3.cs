namespace PerformanceTests.Tests.Users
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;

    public class TestWaitUser3 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent Content = new () { Timeout = 300 };

        public TestWaitUser3(HttpClient client)
            : base(client) 
        {
        }

        protected override async Task PerformanceAsync()
        {
            // Action
            await this.TestWaitMethod1(this.Content, "300ms");
        }
    }
}
