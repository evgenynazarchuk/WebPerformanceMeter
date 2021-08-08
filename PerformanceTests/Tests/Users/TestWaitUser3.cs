using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser3 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 500 };

        public TestWaitUser3(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync()
        {
            // Action
            await TestWaitMethod2(content, "500ms");
        }
    }
}
