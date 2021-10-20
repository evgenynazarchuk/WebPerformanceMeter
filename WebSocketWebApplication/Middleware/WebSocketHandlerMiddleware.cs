using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebSocketWebApplication.Services;

namespace WebSocketWebApplication.Middleware
{
    public class WebSocketHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IWebSocketHandler _webSocketHandler;

        public WebSocketHandlerMiddleware(RequestDelegate next, IWebSocketHandler webSocketHandler)
        {
            this._next = next;
            this._webSocketHandler = webSocketHandler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            using var socket = await context.WebSockets.AcceptWebSocketAsync();
            await this._webSocketHandler.OnConnectedAsync(socket);

            var buffer = new byte[1024];

            while (socket.State == WebSocketState.Open)
            {
                // ожидаем сообщение от клиента
                var result = await socket.ReceiveAsync(
                    buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    // отправить всем клиентам полученное сообщение
                    await _webSocketHandler.ReceiveAsync(socket, result, buffer);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    // закрыть соединение с клиентом
                    await _webSocketHandler.OnDisconnectedAsync(socket);
                }
            }

            // TODO
            //await this._next(context);
        }
    }
}
