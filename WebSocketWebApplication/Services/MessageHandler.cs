using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketWebApplication.Services
{
    public class MessageHandler : WebSocketHandler
    {
        public MessageHandler(ConnectionHandler connectionHandler) : base(connectionHandler) { }

        public override async Task OnConnected(WebSocket socket)
        {
            // add socket
            await base.OnConnected(socket);

            // send message
            var socketId = this.connectionHandler.GetSocketId(socket);
            await SendMessageToAllAsync($"{socketId} is now connected");
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = this.connectionHandler.GetSocketId(socket);
            var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            await SendMessageToAllAsync(message);
        }
    }
}
