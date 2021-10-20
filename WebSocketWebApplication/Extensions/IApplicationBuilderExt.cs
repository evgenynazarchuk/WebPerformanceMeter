using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebSocketWebApplication.Middleware;
using WebSocketWebApplication.Services;

namespace WebSocketWebApplication.Extensions
{
    public static class IApplicationBuilderExt
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                        PathString path,
                                                        IWebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketHandlerMiddleware>(handler));
        }
    }
}
