using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Users.WebSocket
{
    public abstract partial class WebSocketUser : BaseUser, IWebSocketUser
    {
        public virtual ValueTask SendMessageAsync(
            WebSocketTool client,
            string message,
            string label = "")
        {
            return client.SendMessageAsync(message, this.UserName, label);
        }

        public virtual ValueTask<string> ReceiveMessageAsync(WebSocketTool client, string label = "")
        {
            return client.ReceiveMessageAsync(this.UserName, label);
        }

        public virtual ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(
            WebSocketTool client,
            Memory<byte> buffer,
            string label = "")
        {
            return client.ReceiveAsync(buffer, this.UserName, label);
        }

        public virtual ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync(
            WebSocketTool client,
            string label = "")
        {
            return client.ReceiveBytesAsync(this.UserName, label);
        }

        public virtual ValueTask SendAsync(
            WebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage = true,
            string label = "")
        {
            return client.SendAsync(buffer, messageType, endOfMessage, this.UserName, label);
        }

        public virtual ValueTask SendBytesAsync(
            WebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            string label = "")
        {
            return client.SendBytesAsync(buffer, this.UserName, label);
        }
    }
}
