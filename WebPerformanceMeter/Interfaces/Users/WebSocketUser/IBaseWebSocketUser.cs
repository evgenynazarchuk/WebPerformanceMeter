using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface IBaseWebSocketUser : IBasicUser
    {
        void SetClientBuffer(
            int receiveBufferSize = 1024,
            int sendBufferSize = 1024);

        ValueTask SendMessage(
            IWebSocketTool client,
            string message,
            string label = "");

        ValueTask<string> ReceiveMessage(
            IWebSocketTool client,
            string label = "");

        ValueTask<ValueWebSocketReceiveResult> Receive(
            IWebSocketTool client,
            Memory<byte> buffer,
            string label = "");

        ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytes(
            IWebSocketTool client,
            string label = "");

        ValueTask Send(
            IWebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage = true,
            string label = "");

        ValueTask SendBytes(
            IWebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            string label = "");
    }
}
