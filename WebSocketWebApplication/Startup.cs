using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebSocketWebApplication.Extensions;
using WebSocketWebApplication.Services;

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
            services.AddTransient<IConnectionHandler, ConnectionHandler>();
            services.AddSingleton<IWebSocketHandler, MessageHandler>();

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
            var webSocketHandler = serviceProvider.GetService<IWebSocketHandler>();

            //app.UseHsts();
            //app.UseHttpsRedirection();

            app.UseWebSockets();
            app.MapWebSocketManager("/ws", webSocketHandler);

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
