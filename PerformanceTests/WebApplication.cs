using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using TestWebApiServer;

namespace PerformanceTests
{
    public class WebApplication : WebApplicationFactory<Startup>
    {
        public HttpClient Client { get; set; }

        public WebApplication()
        {
            Client = CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
        }
    }
}
