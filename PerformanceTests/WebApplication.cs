﻿namespace PerformanceTests
{
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
    }
}
