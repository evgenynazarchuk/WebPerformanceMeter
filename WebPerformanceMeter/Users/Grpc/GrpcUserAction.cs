using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.Tools.GrpcTool;

namespace WebPerformanceMeter.Users.Grpc
{
    public abstract partial class GrpcUser : User, IGrpcUser
    {
        public virtual ValueTask<TResponse> UnaryCallAsync<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            TRequest requestBody,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            return client.UnaryCallAsync<TResponse, TRequest>(
                methodCall,
                requestBody,
                this.UserName,
                label);
        }

        public virtual ValueTask<TResponse> ClientStreamAsync<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            return client.ClientStreamAsync<TResponse, TRequest>(
                methodCall,
                requestBodyList,
                this.UserName,
                label);
        }

        public virtual ValueTask<IReadOnlyCollection<TResponse>> ServerStreamAsync<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            TRequest requestBody,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            return client.ServerStreamAsync<TResponse, TRequest>(
                methodCall,
                requestBody,
                this.UserName,
                label);
        }

        public virtual ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStreamAsync<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            return client.BidirectionalStreamAsync<TResponse, TRequest>(
                methodCall,
                requestBodyList,
                this.UserName,
                label);
        }
    }
}
