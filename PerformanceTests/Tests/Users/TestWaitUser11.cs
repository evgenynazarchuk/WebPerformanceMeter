using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser11 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 1100 };

        public TestWaitUser11(HttpClient client)
            : base(client) { }

        protected override async Task PerformanceAsync()
        {
            // Action
            await TestWaitMethod1(content, "1100ms");
        }
    }
}
