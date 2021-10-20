﻿using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Tools.WebSocketTool
{
    public class WebSocketClientTool : IWebSocketClientTool, IAsyncDisposable
    {
        public readonly ClientWebSocket ClientWebSocket;

        public readonly Uri Uri;

        public readonly int ReceiveBufferSize;

        public readonly int SendBufferSize;

        public WebSocketClientTool(
            string host,
            int port,
            string path,
            int receiveBufferSize = 1024, 
            int sendBufferSize = 1024)
        {
            this.ClientWebSocket = new ClientWebSocket();
            this.Uri = new UriBuilder()
            {
                Scheme = "ws",
                Host = host,
                Port = port,
                Path = path
            }.Uri;
            this.ReceiveBufferSize = receiveBufferSize;
            this.SendBufferSize = sendBufferSize;
        }

        public static WebSocketClientTool Create(
            string host,
            int port,
            string path,
            int receiveBufferSize = 1024,
            int sendBufferSize = 1024)
            => new WebSocketClientTool(host, port, path, receiveBufferSize, sendBufferSize);

        public async ValueTask ConnectAsync()
        {
            await this.ClientWebSocket.ConnectAsync(this.Uri, CancellationToken.None);
        }

        public async ValueTask DisconnectAsync()
        {
            if (this.ClientWebSocket.State == WebSocketState.Open)
            {
                await this.ClientWebSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Close from web socket client tool",
                    CancellationToken.None);
            }
        }

        // read
        public async ValueTask<ValueWebSocketReceiveResult> ReceiveBytesAsync(Memory<byte> buffer)
        {
            // TODO logging
            var watch = new Stopwatch();

            watch.Start();
            var result = await this.ClientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
            watch.Stop();

            return result;
        }

        public async ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync()
        {
            var buffer = WebSocket.CreateClientBuffer(this.ReceiveBufferSize, this.SendBufferSize);
            var result = await this.ReceiveBytesAsync(buffer);

            return (buffer: buffer, bufferInfo: result);
        }

        public async ValueTask<string> ReceiveMessageAsync()
        {
            var buffer = WebSocket.CreateClientBuffer(1024, 1024);
            var result = await this.ReceiveBytesAsync(buffer);
            var message = Encoding.UTF8.GetString(buffer.ToArray(), 0, result.Count);

            return message;
        }

        // send
        public async ValueTask SendAsync(
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            bool endOfMessage = true)
        {
            // log
            var watch = new Stopwatch();

            watch.Start();
            await this.ClientWebSocket.SendAsync(buffer, messageType, endOfMessage, CancellationToken.None);
            watch.Stop();
        }

        public async ValueTask SendMessageAsync(string message)
        {
            var buffer = new ReadOnlyMemory<byte>(
                array: Encoding.UTF8.GetBytes(message),
                start: 0,
                length: message.Length);

            await this.SendAsync(buffer: buffer, messageType: WebSocketMessageType.Text);
        }

        public async ValueTask SendBytesAsync(ReadOnlyMemory<byte> buffer)
        {
            await this.SendAsync(buffer: buffer, messageType: WebSocketMessageType.Binary);
        }

        // TODO
        // wait and receive for time
        // wait for n message
        // 

        // dispose
        public async ValueTask DisposeAsync()
        {
            await this.DisconnectAsync();
            this.ClientWebSocket.Dispose();
        }
    }
}
