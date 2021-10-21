using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Tools.WebSocketTool;

namespace WebPerformanceMeter.Users.WebSocket
{
    public abstract partial class WebSocketUser : BaseUser, IWebSocketUser
    {
        protected readonly string host;

        protected readonly int port;

        protected readonly string path;

        protected int receiveBufferSize = 1024;

        protected int sendBufferSize = 1024;

        public WebSocketUser(
            string host,
            int port,
            string path,
            ILogger? logger = null,
            string userName = "")
            : base(logger ?? WebSocketLoggerSingleton.GetInstance())
        {
            this.SetUserName(string.IsNullOrEmpty(userName) ? this.GetType().Name : userName);
            this.host = host;
            this.port = port;
            this.path = path;
        }

        public void SetClientBuffer(
            int receiveBufferSize = 1024,
            int sendBufferSize = 1024)
        {
            this.sendBufferSize = sendBufferSize;
            this.receiveBufferSize = receiveBufferSize;
        }

        public override async Task InvokeAsync(
            int loopCount = 1,
            IDataReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            var client = new WebSocketTool(
                this.host,
                this.port,
                this.path,
                this.Logger,
                this.sendBufferSize,
                this.receiveBufferSize);

            object? entity = null;

            if (dataReader is not null)
            {
                entity = dataReader.GetEntity();

                if (entity is null)
                {
                    return;
                }
            }

            for (int i = 0; i < loopCount; i++)
            {
                await client.ConnectAsync(this.UserName);
                if (entity is null)
                {
                    await PerformanceAsync(client);
                }
                else
                {
                    await PerformanceAsync(client, entity);
                }
                await client.DisconnectAsync();

                if (dataReader is not null && !reuseDataInLoop)
                {
                    entity = dataReader.GetEntity();

                    if (entity is null)
                    {
                        return;
                    }
                }
            }
        }

        protected virtual Task PerformanceAsync(WebSocketTool client, object entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PerformanceAsync(WebSocketTool client)
        {
            return Task.CompletedTask;
        }
    }
}
