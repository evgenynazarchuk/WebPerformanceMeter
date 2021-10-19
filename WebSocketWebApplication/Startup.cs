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
            services.AddTransient<ConnectionHandler>();
            services.AddSingleton<MessageHandler>();

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
            var chatHandler = serviceProvider.GetService<MessageHandler>();

            //app.UseHsts();
            //app.UseHttpsRedirection();

            app.UseWebSockets();
            app.MapWebSocketManager("/ws", chatHandler);

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
