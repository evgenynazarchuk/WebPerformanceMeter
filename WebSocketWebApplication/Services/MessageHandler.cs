using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System;

namespace WebSocketWebApplication.Services
{
    public class MessageHandler : WebSocketHandler
    {
        public MessageHandler(IConnectionHandler connectionHandler) : base(connectionHandler) { }

        public override async Task OnConnectedAsync(WebSocket socket)
        {
            await base.OnConnectedAsync(socket);
            var socketId = this.connectionHandler.GetSocketId(socket);
            await this.SendMessageToAllAsync($"{socketId} is now connected");
        }

        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = this.connectionHandler.GetSocketId(socket);
            var text = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var message = $"{socketId} said: {text}";
            return this.SendMessageToAllAsync(message);
        }
    }
}
