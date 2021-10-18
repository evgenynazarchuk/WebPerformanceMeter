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
        protected readonly ConnectionHandler connectionManager;

        public WebSocketHandler(ConnectionHandler connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public virtual Task OnConnected(WebSocket socket)
        {
            this.connectionManager.AddSocket(socket);
            return Task.CompletedTask;
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            var socketId = this.connectionManager.GetSocketId(socket);
            await this.connectionManager.RemoveSocket(socketId);
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
            await SendMessageAsync(this.connectionManager.GetSocketById(socketId), message);
        }

        public Task SendMessageToAllAsync(string message)
        {
            var sendTasks = new List<Task>();

            foreach (var socket in this.connectionManager.GetAllSocket())
            {
                if (socket.State == WebSocketState.Open)
                {
                    sendTasks.Add(Task.Run(async () =>
                    {
                        await this.SendMessageAsync(socket, message);
                    }));
                }
            }

            Task.WaitAll(sendTasks.ToArray());

            return Task.CompletedTask;
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
