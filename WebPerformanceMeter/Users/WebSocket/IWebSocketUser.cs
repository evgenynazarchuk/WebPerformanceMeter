using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebPerformanceMeter.Tools.WebSocketTool;

namespace WebPerformanceMeter.Users.WebSocket
{
    public interface IWebSocketUser : IUser
    {
        void SetClientBuffer(
            int receiveBufferSize = 1024,
            int sendBufferSize = 1024);

        ValueTask SendMessageAsync(
            WebSocketClientTool client,
            string message,
            string label = "");

        ValueTask<string> ReceiveMessageAsync(
            WebSocketClientTool client,
            string label = "");

        ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(
            WebSocketClientTool client,
            Memory<byte> buffer,
            string label = "");

        ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync(
            WebSocketClientTool client,
            string label = "");

        ValueTask SendAsync(
            WebSocketClientTool client,
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage = true,
            string label = "");

        ValueTask SendBytesAsync(
            WebSocketClientTool client,
            ReadOnlyMemory<byte> buffer,
            string label = "");
    }
}
