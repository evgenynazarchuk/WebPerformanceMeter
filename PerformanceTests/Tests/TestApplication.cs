using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using TestWebApiServer;

namespace PerformanceTests
{
    public class TestApplication : WebApplicationFactory<Startup>
    {
        public TestApplication()
        {
            this.HttpClient = this.CreateClient();
        }

        public HttpClient HttpClient { get; set; }
    }
}
