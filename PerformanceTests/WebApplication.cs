using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using TestWebApiServer;

namespace PerformanceTests
{
    public class WebApplication : WebApplicationFactory<Startup>
    {
        public HttpClient HttpClient { get; set; }

        public WebApplication()
        {
            HttpClient = CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
        }
    }
}
