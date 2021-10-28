using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicGrpcUser : BasicUser
    {
        public ValueTask<ActionResult<TResponse>> UnaryCall<TResponse, TRequest>(
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

        public ValueTask<TResponse> ClientStream<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            ICollection<TRequest> requestBodyList,
            int millisecondsDelay = 0,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            return client.ClientStreamAsync<TResponse, TRequest>(
                methodCall,
                requestBodyList,
                millisecondsDelay,
                this.UserName,
                label);
        }

        public ValueTask<IReadOnlyCollection<TResponse>> ServerStream<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            TRequest requestBody,
            int millisecondsDelay = 0,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            return client.ServerStreamAsync<TResponse, TRequest>(
                methodCall,
                requestBody,
                millisecondsDelay,
                this.UserName,
                label);
        }

        public ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStream<TResponse, TRequest>(
            GrpcClientTool client,
            string methodCall,
            ICollection<TRequest> requestBodyList,
            int sendMillisecondsDelay = 0,
            int readMillisecondsDelay = 0,
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            return client.BidirectionalStreamAsync<TResponse, TRequest>(
                methodCall,
                requestBodyList,
                sendMillisecondsDelay,
                readMillisecondsDelay,
                this.UserName,
                label);
        }
    }
}
