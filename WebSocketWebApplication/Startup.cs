using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebSocketWebApplication.Services;
using WebSocketWebApplication.Middleware;
using System.Net.WebSockets;
using System;
using System.Linq;
using System.Reflection;
using WebSocketWebApplication.Extensions;
using System.Net;

namespace WebSocketWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ConnectionHandler>();
            services.AddSingleton<ChatHandler>();

            //services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebSocketWebApplication", Version = "v1" });
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
            var chatHandler = serviceProvider.GetService<ChatHandler>();

            //app.UseHsts();
            //app.UseHttpsRedirection();

            app.UseWebSockets();
            app.MapWebSocketManager("/ws", chatHandler);

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
