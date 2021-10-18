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

namespace WebSocketWebApplication.Extensions
{
    public static class IApplicationBuilderExt
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                        PathString path,
                                                        WebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketHandlerMiddleware>(handler));
        }
    }
}
