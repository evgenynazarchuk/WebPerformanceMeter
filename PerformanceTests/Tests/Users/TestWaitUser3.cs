using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser3 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 300 };

        public TestWaitUser3(HttpClient client)
            : base(client) { }

        protected override async Task PerformanceAsync()
        {
            // Action
            await TestWaitMethod1(content, "300ms");
        }
    }
}
