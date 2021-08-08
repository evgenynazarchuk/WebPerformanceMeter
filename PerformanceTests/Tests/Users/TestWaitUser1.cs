using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser1 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 100 };

        public TestWaitUser1(HttpClient client)
            : base(client) { }

        protected override async Task PerformanceAsync()
        {
            // Action
            await TestWaitMethod1(content, "100ms");
        }
    }
}
