﻿using System;
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
            WebSocketTool client,
            string message,
            string label = "");

        ValueTask<string> ReceiveMessageAsync(
            WebSocketTool client,
            string label = "");

        ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(
            WebSocketTool client,
            Memory<byte> buffer,
            string label = "");

        ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync(
            WebSocketTool client,
            string label = "");

        ValueTask SendAsync(
            WebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage = true,
            string label = "");

        ValueTask SendBytesAsync(
            WebSocketTool client,
            ReadOnlyMemory<byte> buffer,
            string label = "");
    }
}
