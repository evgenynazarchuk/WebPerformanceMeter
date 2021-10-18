using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Text;

namespace WebSocketWebApplication.Services
{
    public class ChatHandler : WebSocketHandler
    {
        public ChatHandler(ConnectionHandler connectionHandler) : base(connectionHandler) { }

        public override async Task OnConnected(WebSocket socket)
        {
            // add socket
            await base.OnConnected(socket);

            // send message
            var socketId = this.connectionManager.GetSocketId(socket);
            await SendMessageToAllAsync($"{socketId} is now connected");
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = this.connectionManager.GetSocketId(socket);
            var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            await SendMessageToAllAsync(message);
        }
    }
}
