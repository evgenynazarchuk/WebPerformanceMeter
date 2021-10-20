using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebSocketWebApplication.IntegrationTest.Support.Tool
{
    public interface IWebSocketClientTool
    {
        Task ConnectAsync();

        ValueTask DisconnectAsync();

        ValueTask SendMessageAsync(string message);

        ValueTask<string> ReceiveMessageAsync();

        ValueTask<ValueWebSocketReceiveResult> ReceiveBytesAsync(Memory<byte> buffer);

        ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync();

        ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage = true);

        ValueTask SendBytesAsync(ReadOnlyMemory<byte> buffer);
    }
}
