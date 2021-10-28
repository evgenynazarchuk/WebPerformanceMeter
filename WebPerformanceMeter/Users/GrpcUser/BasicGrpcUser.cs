using System;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicGrpcUser : BasicUser
    {
        public BasicGrpcUser(string address, string userName)
            : base(userName)
        {
            this.address = address;
        }

        public void UseGrpcClient(Type grpcClient)
        {
            this.grpcClientType = grpcClient;
        }

        protected readonly string address;

        protected Type? grpcClientType = null;
    }
}
