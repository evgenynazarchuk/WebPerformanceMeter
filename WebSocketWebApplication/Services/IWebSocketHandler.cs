using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebSocketWebApplication.Services
{
    public interface IWebSocketHandler
    {
        Task OnConnectedAsync(WebSocket socket);

        Task OnDisconnectedAsync(WebSocket socket);

        Task SendMessageAsync(WebSocket socket, string message);

        Task SendMessageToAllAsync(string message);

        Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
