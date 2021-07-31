using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TestWebApiServer;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;

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
