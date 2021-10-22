using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicWebSocketUser : BaseUser, IBaseWebSocketUser
    {
        public virtual ValueTask SendMessage(
            IWebSocketTool client,
            string message,
            string label = "")
        {
            return client.SendMessageAsync(message, this.UserName, label);
        }

        public virtual ValueTask<string> ReceiveMessage(
            IWebSocketTool client, 
            string label = "")
        {
            return client.ReceiveMessageAsync(this.UserName, label);
        }

        public virtual ValueTask<ValueWebSocketReceiveResult> Receive(
            IWebSocketTool client,
            Memory<byte> buffer,
            string label = "")
        {
            return client.ReceiveAsync(buffer, this.UserName, label);
        }

        public virtual ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytes(
            IWebSocketTool client,
            string label = "")
        {
            return client.ReceiveBytesAsync(this.UserName, label);
        }

        public virtual ValueTask Send(
            IWebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage = true,
            string label = "")
        {
            return client.SendAsync(buffer, messageType, endOfMessage, this.UserName, label);
        }

        public virtual ValueTask SendBytes(
            IWebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            string label = "")
        {
            return client.SendBytesAsync(buffer, this.UserName, label);
        }
    }
}
