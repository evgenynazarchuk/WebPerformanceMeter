using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
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
            string? userName = null)
            : base(host, port, path, userName ?? typeof(WebSocketUser<TData>).Name) { }

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
                this.Watcher,
                this.sendBufferSize,
                this.receiveBufferSize);

            TData? data = dataReader.GetData();

            for (int i = 0; i < userLoopCount; i++)
            {
                if (data is null)
                {
                    continue;
                }

                await client.ConnectAsync(this.UserName);
                await PerformanceAsync(client, data);
                await client.DisconnectAsync(this.UserName);

                if (!reuseDataInLoop)
                {
                    data = dataReader.GetData();
                }
            }
        }

        protected abstract Task PerformanceAsync(WebSocketTool client, object entity);
    }
}
