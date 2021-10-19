using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Net.WebSockets;

namespace WebSocketWebApplication.Services
{
    public abstract class WebSocketHandler : IWebSocketHandler
    {
        protected readonly IConnectionHandler connectionHandler;

        public WebSocketHandler(ConnectionHandler connectionHandler)
        {
            this.connectionHandler = connectionHandler;
        }

        public virtual Task OnConnected(WebSocket socket)
        {
            this.connectionHandler.AddSocket(socket);
            return Task.CompletedTask;
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            var socketId = this.connectionHandler.GetSocketId(socket);
            await this.connectionHandler.RemoveSocket(socketId);
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
            {
                return;
            }

            await socket.SendAsync(
                buffer: new ArraySegment<byte>(
                    array: Encoding.UTF8.GetBytes(message),
                    offset: 0,
                    count: message.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(Guid socketId, string message)
        {
            await SendMessageAsync(this.connectionHandler.GetSocketById(socketId), message);
        }

        public Task SendMessageToAllAsync(string message)
        {
            var tasks = new List<Task>();

            foreach (var toWebSocket in this.connectionHandler.GetAllSocket())
            {
                if (toWebSocket.State == WebSocketState.Open)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        await this.SendMessageAsync(toWebSocket, message);
                    }));
                }
            }

            Task.WaitAll(tasks.ToArray());

            return Task.CompletedTask;
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
