using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebPerformanceMeter.Tools.WebSocketTool;

namespace WebPerformanceMeter.Users.WebSocket
{
    public abstract partial class WebSocketUser : User, IWebSocketUser
    {
        public virtual ValueTask SendMessageAsync(
            WebSocketClientTool client,
            string message,
            string label = "")
        {
            return client.SendMessageAsync(message, this.UserName, label);
        }

        public virtual ValueTask<string> ReceiveMessageAsync(WebSocketClientTool client, string label = "")
        {
            return client.ReceiveMessageAsync(this.UserName, label);
        }

        public virtual ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(
            WebSocketClientTool client,
            Memory<byte> buffer,
            string label = "")
        {
            return client.ReceiveAsync(buffer, this.UserName, label);
        }

        public virtual ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync(
            WebSocketClientTool client,
            string label = "")
        {
            return client.ReceiveBytesAsync(this.UserName, label);
        }

        public virtual ValueTask SendAsync(
            WebSocketClientTool client,
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage = true,
            string label = "")
        {
            return client.SendAsync(buffer, messageType, endOfMessage, this.UserName, label);
        }

        public virtual ValueTask SendBytesAsync(
            WebSocketClientTool client,
            ReadOnlyMemory<byte> buffer,
            string label = "")
        {
            return client.SendBytesAsync(buffer, this.UserName, label);
        }
    }
}
