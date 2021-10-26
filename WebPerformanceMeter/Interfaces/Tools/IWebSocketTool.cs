﻿using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface IWebSocketTool : ITool
    {
        ValueTask ConnectAsync(string userName = "");

        ValueTask DisconnectAsync(string userName = "");

        ValueTask SendMessageAsync(string message, string userName = "", string label = "");

        ValueTask<string> ReceiveMessageAsync(string userName = "", string label = "");

        ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer, string userName = "", string label = "");

        ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync(string userName = "", string label = "");

        ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage = true, string userName = "", string label = "");

        ValueTask SendBytesAsync(ReadOnlyMemory<byte> buffer, string userName = "", string label = "");
    }
}
