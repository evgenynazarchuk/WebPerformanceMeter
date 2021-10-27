using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Reports;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicWebSocketUser : BasicUser
    {
        public BasicWebSocketUser(
            string host,
            int port,
            string path,
            string? userName = null)
            : base(userName ?? typeof(BasicChromiumUser).Name)
        {
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

        protected readonly string host;

        protected readonly int port;

        protected readonly string path;

        protected int receiveBufferSize = 1024;

        protected int sendBufferSize = 1024;
    }
}
