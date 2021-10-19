using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace WebSocketWebApplication.IntegrationTest.Support.Tool
{
    public interface IWebSocketClientTool
    {
        Task ConnectAsync();

        Task DisconnectAsync();

        ValueTask SendMessageAsync(string message);

        ValueTask<string> ReceiveMessageAsync();

        ValueTask<ValueWebSocketReceiveResult> ReceiveBytesAsync(Memory<byte> buffer);

        ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync();

        ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage = true);

        ValueTask SendBytesAsync(ReadOnlyMemory<byte> buffer);
    }
}
