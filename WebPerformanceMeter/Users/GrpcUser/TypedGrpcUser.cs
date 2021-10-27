using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter
{
    public abstract partial class GrpcUser<TData> : BasicGrpcUser, ITypedUser<TData>
        where TData : class
    {
        public GrpcUser(string address, string? userName = null)
            : base(address, userName ?? typeof(GrpcUser).Name) { }

        public async Task InvokeAsync(
            IDataReader<TData> dataReader,
            bool reuseDataInLoop = true,
            int userLoopCount = 1
            )
        {
            if (this.grpcClientType is null)
            {
                throw new ApplicationException("Grpc Client Type is not set. Try UseGrpcClient()");
            }

            using var client = new GrpcClientTool(this.address, this.grpcClientType, this.Watcher);

            var data = dataReader.GetData();

            for (int i = 0; i < userLoopCount; i++)
            {
                if (data is null)
                {
                    continue;
                }

                await PerformanceAsync(client, data);

                if (!reuseDataInLoop)
                {
                    data = dataReader.GetData();
                }
            }
        }

        protected abstract Task PerformanceAsync(GrpcClientTool client, TData entity);
    }
}
