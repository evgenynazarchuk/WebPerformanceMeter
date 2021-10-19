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

        private readonly WebSocketHandler _webSocketHandler;

        public WebSocketHandlerMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
        {
            this._next = next;
            this._webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await this._webSocketHandler.OnConnected(socket);

            var buffer = new byte[1024];

            while (socket.State == WebSocketState.Open)
            {
                // ожидаем сообщение от клиента
                var result = await socket.ReceiveAsync(
                    buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);


                if (result.MessageType == WebSocketMessageType.Text)
                {
                    // отправить прочитанное сообщение
                    await _webSocketHandler.ReceiveAsync(socket, result, buffer);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    // закрыть соединение с клиентом
                    await _webSocketHandler.OnDisconnected(socket);
                }
            }

            // TODO
            // await this._next(context);
        }
    }
}
