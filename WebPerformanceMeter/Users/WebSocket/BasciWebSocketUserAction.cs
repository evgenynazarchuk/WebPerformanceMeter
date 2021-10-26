using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Extensions;
using System.Timers;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicWebSocketUser : BaseUser
    {
        public virtual ValueTask SendMessage(
            WebSocketTool client,
            string message,
            string label = "")
        {
            return client.SendMessageAsync(message, this.UserName, label);
        }

        public virtual ValueTask<string> ReceiveMessage(
            WebSocketTool client,
            string label = "")
        {
            return client.ReceiveMessageAsync(this.UserName, label);
        }

        public virtual async ValueTask<List<string>> ReceiveMessage(
            WebSocketTool client,
            int readMilliseconds,
            string label = "")
        {
            var messages = new List<string>();

            var currentTime = DateTime.UtcNow;
            var endTime = currentTime.AddMilliseconds(readMilliseconds);
            while (currentTime < endTime)
            {
                var message = await client.ReceiveMessageAsync(this.UserName, label);
                messages.Add(message);
            }

            return messages;
        }

        public virtual async ValueTask<List<string>> ReceiveMessage(
            WebSocketTool client,
            int messageCount,
            int readMilliseconds,
            string label = "")
        {
            var messages = new List<string>();

            var currentTime = DateTime.UtcNow;
            var endTime = currentTime.AddMilliseconds(readMilliseconds);

            while (messages.Count != messageCount && currentTime < endTime)
            {
                var message = await client.ReceiveMessageAsync(this.UserName, label);
                messages.Add(message);
            }

            return messages;
        }

        public virtual ValueTask<ValueWebSocketReceiveResult> Receive(
            WebSocketTool client,
            Memory<byte> buffer,
            string label = "")
        {
            return client.ReceiveAsync(buffer, this.UserName, label);
        }

        public virtual ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytes(
            WebSocketTool client,
            string label = "")
        {
            return client.ReceiveBytesAsync(this.UserName, label);
        }

        public virtual ValueTask Send(
            WebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage = true,
            string label = "")
        {
            return client.SendAsync(buffer, messageType, endOfMessage, this.UserName, label);
        }

        public virtual ValueTask SendBytes(
            WebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            string label = "")
        {
            return client.SendBytesAsync(buffer, this.UserName, label);
        }
    }
}
