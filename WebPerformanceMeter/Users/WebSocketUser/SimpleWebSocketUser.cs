using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class WebSocketUser : BasicWebSocketUser, ISimpleUser
    {
        public WebSocketUser(
            string host,
            int port,
            string path,
            string? userName = null)
            : base(host, port, path, userName ?? typeof(WebSocketUser).Name) { }

        public async Task InvokeAsync(int userLoopCount = 1)
        {
            var client = new WebSocketTool(
                this.host,
                this.port,
                this.path,
                this.Watcher,
                this.sendBufferSize,
                this.receiveBufferSize);

            for (int i = 0; i < userLoopCount; i++)
            {
                await client.ConnectAsync(this.UserName);
                await PerformanceAsync(client);
                await client.DisconnectAsync(this.UserName);
            }
        }

        protected abstract Task PerformanceAsync(WebSocketTool client);
    }
}
