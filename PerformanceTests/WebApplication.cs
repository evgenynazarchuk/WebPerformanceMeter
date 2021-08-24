namespace PerformanceTests
{
    using System.Net.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using TestWebApiServer;

    public class WebApplication : WebApplicationFactory<Startup>
    {
        public WebApplication()
        {
            this.HttpClient = this.CreateClient();
        }

        public HttpClient HttpClient { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
        }
    }
}
