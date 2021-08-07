using System.Net.Http;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace PerformanceTests.Tests.Users
{
    public class TestWaitUser3 : TestFacade
    {
        // Arange
        public readonly TestRequestContent content = new TestRequestContent { Timeout = 500 };

        public TestWaitUser3(HttpClient client)
            : base(client) { }

        public override async Task PerformanceAsync()
        {
            // Act
            await TestWaitMethod2(content, "500ms");
        }
    }
}
