using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class WebSocketUser<TData> : BasicWebSocketUser, ITypedUser<TData>
        where TData : class
    {
        public WebSocketUser(
            string host,
            int port,
            string path,
            string userName = "",
            ILogger? logger = null)
            : base(host, port, path, userName, logger) { }

        public async Task InvokeAsync(
            IDataReader<TData> dataReader,
            bool reuseDataInLoop = true,
            int userLoopCount = 1
            )
        {
            var client = new WebSocketTool(
                this.host,
                this.port,
                this.path,
                this.sendBufferSize,
                this.receiveBufferSize,
                this.Logger);

            TData? data = dataReader.GetData();

            for (int i = 0; i < userLoopCount; i++)
            {
                if (data is null)
                {
                    continue;
                }

                await client.ConnectAsync(this.UserName);
                await PerformanceAsync(client, data);
                await client.DisconnectAsync();

                if (!reuseDataInLoop)
                {
                    data = dataReader.GetData();
                }
            }
        }

        protected abstract Task PerformanceAsync(IWebSocketTool client, object entity);
    }
}
