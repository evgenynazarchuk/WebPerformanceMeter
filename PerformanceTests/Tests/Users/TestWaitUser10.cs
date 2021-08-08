using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser10 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 1000 };

        public TestWaitUser10(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync()
        {
            // Action
            await TestWaitMethod2(content, "1000ms");
        }
    }
}
