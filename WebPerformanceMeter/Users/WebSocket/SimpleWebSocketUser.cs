using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class WebSocketUser : BasicWebSocketUser, ISimpleUser
    {
        public WebSocketUser(
            string host,
            int port,
            string path,
            string userName = "",
            ILogger? logger = null)
            : base(host, port, path, userName, logger) { }

        public async Task InvokeAsync(int userLoopCount = 1)
        {
            var client = new WebSocketTool(
                this.host,
                this.port,
                this.path,
                this.sendBufferSize,
                this.receiveBufferSize,
                this.Logger);

            for (int i = 0; i < userLoopCount; i++)
            {
                await client.ConnectAsync(this.UserName);
                await PerformanceAsync(client);
                await client.DisconnectAsync();
            }
        }

        protected abstract Task PerformanceAsync(IWebSocketTool client);
    }
}
