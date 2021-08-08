using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser1 : TestUserFacade
    {
        // Arange
        public readonly TestRequestContent content = new() { Timeout = 50 };

        public TestWaitUser1(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync()
        {
            // Act
            await TestWaitMethod1(content, "50ms");
        }
    }
}
