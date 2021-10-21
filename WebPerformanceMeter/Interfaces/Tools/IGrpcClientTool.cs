﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface IGrpcClientTool : ITool
    {
        ValueTask<TResponse> UnaryCallAsync<TResponse, TRequest>(
            string methodCall,
            TRequest requestBody,
            string userName = "",
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<TResponse> ClientStreamAsync<TResponse, TRequest>(
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string userName = "",
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<IReadOnlyCollection<TResponse>> ServerStreamAsync<TResponse, TRequest>(
            string methodCall,
            TRequest requestBody,
            string userName = "",
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();

        ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStreamAsync<TResponse, TRequest>(
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string userName = "",
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new();
    }
}
