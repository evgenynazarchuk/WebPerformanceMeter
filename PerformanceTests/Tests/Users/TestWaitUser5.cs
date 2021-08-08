using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser5 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 500 };

        public TestWaitUser5(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync()
        {
            // Action
            await TestWaitMethod1(content, "500ms");
        }
    }
}
