﻿using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Support;

namespace WebPerformanceMeter.Tools.GrpcTool
{
    public sealed class GrpcClientTool : IGrpcClientTool, IDisposable
    {
        private readonly GrpcChannel _grpcChannel;

        private readonly object _grpcClient;

        private readonly ILogger _logger;

        public GrpcClientTool(
            string address,
            ILogger logger,
            Type serviceClientType)
        {
            this._grpcChannel = GrpcChannel.ForAddress(address);

            var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

            this._grpcClient = ctor is not null
                ? ctor.Invoke(new[] { this._grpcChannel })
                : throw new ApplicationException("gRpc client is not create");

            if (this._grpcClient is null)
            {
                throw new ApplicationException("gRpc client is not create");
            }

            this._logger = logger;
        }

        public GrpcClientTool(
            HttpClient httpClient,
            ILogger logger,
            Type serviceClientType)
        {
            this._grpcChannel = httpClient.BaseAddress is not null
                ? GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions { HttpClient = httpClient })
                : throw new ApplicationException("Base address is not set");

            var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

            this._grpcClient = ctor is not null
                ? ctor.Invoke(new[] { this._grpcChannel })
                : throw new ApplicationException("gRpc client is not create");

            this._logger = logger;
        }

        public async ValueTask<TResponse> UnaryCallAsync<TResponse, TRequest>(
            string methodCall,
            TRequest requestBody,
            string userName = "",
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            var method = this._grpcClient
                .GetType()
                .GetMethods()
                .Where(x => x.Name == methodCall)
                .Single(x => x.GetParameters().Count() == 4);

            long startMethodCall;
            long endMethodCall;
            startMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            //
            var response = await (AsyncUnaryCall<TResponse>)method.Invoke(this._grpcClient, new object[] { requestBody, null, null, null });
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;
            this._logger.AppendLogMessage("GrpcLogMessage.json", $"{userName},{method.Name},{label},{startMethodCall},{endMethodCall}", typeof(GrpcLogMessage));

            return response;
        }

        public async ValueTask<TResponse> ClientStreamAsync<TResponse, TRequest>(
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string userName = "",
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            var method = this._grpcClient
                .GetType()
                .GetMethods()
                .Where(x => x.Name == methodCall)
                .Single(x => x.GetParameters().Count() == 3);

            long startMethodCall;
            long endMethodCall;
            startMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            //
            using var grpcConnect = (AsyncClientStreamingCall<TRequest, TResponse>)method.Invoke(this._grpcClient, new object[] { null, null, null });

            foreach (var requestBody in requestBodyList)
            {
                await grpcConnect.RequestStream.WriteAsync(requestBody);
            }

            await grpcConnect.RequestStream.CompleteAsync();
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;
            this._logger.AppendLogMessage("GrpcLogMessage.json", $"{userName},{method.Name},{label},{startMethodCall},{endMethodCall}", typeof(GrpcLogMessage));

            return grpcConnect.ResponseAsync.Result;
        }

        public async ValueTask<IReadOnlyCollection<TResponse>> ServerStreamAsync<TResponse, TRequest>(
            string methodCall,
            TRequest requestBody,
            string userName = "",
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            var method = this._grpcClient
                .GetType()
                .GetMethods()
                .Where(x => x.Name == methodCall)
                .Single(x => x.GetParameters().Count() == 4);

            long startMethodCall;
            long endMethodCall;
            startMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            //
            using var grpcConnect = (AsyncServerStreamingCall<TResponse>)method.Invoke(this._grpcClient, new object[] { requestBody, null, null, null });
            var messages = new List<TResponse>();

            while (await grpcConnect.ResponseStream.MoveNext())
            {
                messages.Add(grpcConnect.ResponseStream.Current);
            }
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;
            this._logger.AppendLogMessage("GrpcLogMessage.json", $"{userName},{method.Name},{label},{startMethodCall},{endMethodCall}", typeof(GrpcLogMessage));

            return messages;
        }

        public async ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStreamAsync<TResponse, TRequest>(
            string methodCall,
            ICollection<TRequest> requestBodyList,
            string userName = "",
            string label = "")
            where TRequest : class, new()
            where TResponse : class, new()
        {
            var method = this._grpcClient
                .GetType()
                .GetMethods()
                .Where(x => x.Name == methodCall)
                .Single(x => x.GetParameters().Count() == 3);

            long startMethodCall;
            long endMethodCall;
            startMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            //
            using var grpcConnect = (AsyncDuplexStreamingCall<TRequest, TResponse>)method.Invoke(this._grpcClient, new object[] { null, null, null });
            var responseMessages = new List<TResponse>();

            var readMessageTask = Task.Run(async () =>
            {
                await foreach (var responseMessage in grpcConnect.ResponseStream.ReadAllAsync())
                {
                    responseMessages.Add(responseMessage);
                }
            });


            foreach (var requestBody in requestBodyList)
            {
                await grpcConnect.RequestStream.WriteAsync(requestBody);
            }

            await grpcConnect.RequestStream.CompleteAsync();
            await readMessageTask;
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;
            this._logger.AppendLogMessage("GrpcLogMessage.json", $"{userName},{method.Name},{label},{startMethodCall},{endMethodCall}", typeof(GrpcLogMessage));

            return responseMessages;
        }

        public void Dispose()
        {
            this._grpcChannel.Dispose();
        }
    }
}
