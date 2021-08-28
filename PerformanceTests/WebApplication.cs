namespace PerformanceTests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Net.Http;
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
