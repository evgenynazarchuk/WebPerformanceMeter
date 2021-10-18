using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace WebSocketWebApplication.Services
{
    public interface IWebSocketHandler
    {
        Task OnConnected(WebSocket socket);

        Task OnDisconnected(WebSocket socket);

        Task SendMessageAsync(WebSocket socket, string message);

        Task SendMessageAsync(Guid socketId, string message);

        Task SendMessageToAllAsync(string message);

        Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
