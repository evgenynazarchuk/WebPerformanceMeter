using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketWebApplication.Services
{
    public abstract class WebSocketHandler : IWebSocketHandler
    {
        protected readonly IConnectionHandler connectionHandler;

        public WebSocketHandler(IConnectionHandler connectionHandler)
        {
            this.connectionHandler = connectionHandler;
        }

        public virtual Task OnConnectedAsync(WebSocket socket)
        {
            this.connectionHandler.AddSocket(socket);
            return Task.CompletedTask;
        }

        public virtual Task OnDisconnectedAsync(WebSocket socket)
        {
            var socketId = this.connectionHandler.GetSocketId(socket);
            return this.connectionHandler.RemoveSocketAsync(socketId);
        }

        public Task SendMessageAsync(WebSocket socket, string message)
        {
            return socket.SendAsync(
                buffer: new ArraySegment<byte>(
                    array: Encoding.UTF8.GetBytes(message),
                    offset: 0,
                    count: message.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var toWebSocket in this.connectionHandler.GetAllSocket())
            {
                if (toWebSocket.State == WebSocketState.Open)
                {
                    try
                    {
                        await this.SendMessageAsync(toWebSocket, message);
                    }
                    catch
                    {
                        Console.WriteLine($"Error send message, to {toWebSocket.State.ToString()}");
                    }
                }
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
