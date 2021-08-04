using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using TestWebApiServer;

namespace Tests
{
    public class WebApp : WebApplicationFactory<Startup>
    {
        public HttpClient Client { get; set; }

        public WebApp()
        {
            Client = CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
        }
    }
}
