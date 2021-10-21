using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter
{
    public class WebSocketTool : Tool, IWebSocketTool, IAsyncDisposable
    {
        public readonly ClientWebSocket ClientWebSocket;

        public readonly Uri Uri;

        public readonly int ReceiveBufferSize;

        public readonly int SendBufferSize;

        public WebSocketTool(
            string host,
            int port,
            string path,
            int receiveBufferSize = 1024,
            int sendBufferSize = 1024,
            ILogger? logger = null)
            : base(logger)
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

        public async ValueTask ConnectAsync(string userName = "")
        {
            long startConnect;
            long endConnect;

            startConnect = ScenarioTimer.Time.Elapsed.Ticks;
            await this.ClientWebSocket.ConnectAsync(this.Uri, CancellationToken.None);
            endConnect = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.Logger is not null)
            {
                this.Logger.AddLogMessage(
                    "WebSocketLogMessage.json",
                    $"{userName},,connect,{startConnect},{endConnect}",
                    typeof(WebSocketLogMessage));
            }
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


        // base
        public async ValueTask SendAsync(
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            bool endOfMessage = true,
            string userName = "",
            string label = "")
        {
            long startRequest;
            long finishRequest;

            startRequest = ScenarioTimer.Time.Elapsed.Ticks;
            await this.ClientWebSocket.SendAsync(buffer, messageType, endOfMessage, CancellationToken.None);
            finishRequest = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.Logger is not null)
            {
                this.Logger.AddLogMessage(
                    "WebSocketLogMessage.json",
                    $"{userName},{label},send,{startRequest},{finishRequest}",
                    typeof(WebSocketLogMessage));
            }
        }

        public async ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(
            Memory<byte> buffer,
            string userName = "",
            string label = "")
        {
            long startReceive;
            long endReceive;

            startReceive = ScenarioTimer.Time.Elapsed.Ticks;
            var result = await this.ClientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
            endReceive = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.Logger is not null)
            {
                this.Logger.AddLogMessage(
                    $"WebSocketLogMessage.json",
                    $"{userName},{label},receive,{startReceive},{endReceive}",
                    typeof(WebSocketLogMessage));
            }
            
            return result;
        }

        // message
        public async ValueTask SendMessageAsync(string message, string userName = "", string label = "")
        {
            var buffer = new ReadOnlyMemory<byte>(
                array: Encoding.UTF8.GetBytes(message),
                start: 0,
                length: message.Length);

            await this.SendAsync(
                buffer: buffer,
                messageType: WebSocketMessageType.Text,
                userName: userName,
                label: label);
        }

        public async ValueTask<string> ReceiveMessageAsync(string userName = "", string label = "")
        {
            var buffer = WebSocket.CreateClientBuffer(1024, 1024);
            var result = await this.ReceiveAsync(buffer, userName, label);
            var message = Encoding.UTF8.GetString(buffer.ToArray(), 0, result.Count);

            return message;
        }


        // bytes
        public async ValueTask SendBytesAsync(
            ReadOnlyMemory<byte> buffer,
            string userName = "",
            string label = "")
        {
            await this.SendAsync(
                buffer: buffer,
                messageType: WebSocketMessageType.Binary,
                userName: userName,
                label: label);
        }

        public async ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync(
            string userName = "",
            string label = "")
        {
            var buffer = WebSocket.CreateClientBuffer(this.ReceiveBufferSize, this.SendBufferSize);
            var result = await this.ReceiveAsync(buffer, userName, label);

            return (buffer: buffer, bufferInfo: result);
        }

        // dispose
        public async ValueTask DisposeAsync()
        {
            await this.DisconnectAsync();
            this.ClientWebSocket.Dispose();
        }
    }
}
