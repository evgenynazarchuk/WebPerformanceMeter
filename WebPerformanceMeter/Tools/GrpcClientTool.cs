using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Tools;

namespace WebPerformanceMeter
{
    public sealed class GrpcClientTool : Tool, IDisposable
    {
        private readonly GrpcChannel _grpcChannel;

        private readonly object _grpcClient;

        public GrpcClientTool(
            string address,
            Type serviceClientType,
            ILogger? logger = null)
            : base(logger)

        {
            this._grpcChannel = GrpcChannel.ForAddress(address);

            var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

            if (ctor is not null)
            {
                this._grpcClient = ctor.Invoke(new[] { this._grpcChannel });

                if (this._grpcClient is null)
                {
                    throw new ApplicationException("gRpc client is not create");
                }
            }
            else
            {
                throw new ApplicationException("gRpc client is not create");
            }
        }

        public GrpcClientTool(
            HttpClient httpClient,
            Type serviceClientType,
            ILogger? logger = null)
            : base(logger)
        {
            if (httpClient.BaseAddress is not null)
            {
                this._grpcChannel = GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions { HttpClient = httpClient });
            }
            else
            {
                throw new ApplicationException("Base address is not set");
            }

            var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

            if (ctor is not null)
            {
                this._grpcClient = ctor.Invoke(new[] { this._grpcChannel });

                if (this._grpcClient is null)
                {
                    throw new ApplicationException("gRpc client is not create");
                }
            }
            else
            {
                throw new ApplicationException("gRpc client is not create");
            }
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
            using var grpcCall = (AsyncUnaryCall<TResponse>?)method.Invoke(this._grpcClient, new object[] { requestBody, null, null, null });

            if (grpcCall is null)
            {
                throw new ApplicationException("gRPC call error");
            }

            var response = await grpcCall;
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.logger is not null)
            {
                this.logger.AddLogMessage(
                    "GrpcLogMessage.json",
                    $"{userName},{method.Name},{label},{startMethodCall},{endMethodCall}",
                    typeof(GrpcLogMessage));
            }

            return response;
        }

        public async ValueTask<TResponse> ClientStreamAsync<TResponse, TRequest>(
            string methodCall,
            ICollection<TRequest> requestObjects,
            int millisecondsDelay = 0,
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
            using var grpcCall = (AsyncClientStreamingCall<TRequest, TResponse>?)method.Invoke(this._grpcClient, new object[] { null, null, null });

            if (grpcCall is null)
            {
                throw new ApplicationException("gRPC call error");
            }

            foreach (var requestBody in requestObjects)
            {
                await grpcCall.RequestStream.WriteAsync(requestBody);
                await Task.Delay(millisecondsDelay);
            }

            await grpcCall.RequestStream.CompleteAsync();
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.logger is not null)
            {
                this.logger.AddLogMessage(
                    "GrpcLogMessage.json",
                    $"{userName},{method.Name},{label},{startMethodCall},{endMethodCall}",
                    typeof(GrpcLogMessage));
            }

            return grpcCall.ResponseAsync.Result;
        }

        public async ValueTask<IReadOnlyCollection<TResponse>> ServerStreamAsync<TResponse, TRequest>(
            string methodCall,
            TRequest requestBody,
            int millisecondsDelay = 0,
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
            // TODO add using
            using var grpcCall = (AsyncServerStreamingCall<TResponse>?)method.Invoke(this._grpcClient, new object[] { requestBody, null, null, null });
            if (grpcCall is null)
            {
                throw new ApplicationException("gRPC call error");
            }

            var messages = new List<TResponse>();

            while (await grpcCall.ResponseStream.MoveNext())
            {
                messages.Add(grpcCall.ResponseStream.Current);
                await Task.Delay(millisecondsDelay);
            }
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.logger is not null)
            {
                this.logger.AddLogMessage(
                    "GrpcLogMessage.json",
                    $"{userName},{method.Name},{label},{startMethodCall},{endMethodCall}",
                    typeof(GrpcLogMessage));
            }

            return messages;
        }

        public async ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStreamAsync<TResponse, TRequest>(
            string methodCall,
            ICollection<TRequest> requestObjects,
            int sendMillisecondsDelay = 0,
            int readMillisecondsDelay = 0,
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
            using var grpcCall = (AsyncDuplexStreamingCall<TRequest, TResponse>?)method.Invoke(this._grpcClient, new object[] { null, null, null });
            if (grpcCall is null)
            {
                throw new ApplicationException("gRPC call error");
            }

            var responseMessages = new List<TResponse>();

            var readMessageTask = Task.Run(async () =>
            {
                await foreach (var responseMessage in grpcCall.ResponseStream.ReadAllAsync())
                {
                    responseMessages.Add(responseMessage);
                    await Task.Delay(readMillisecondsDelay);
                }
            });


            foreach (var requestBody in requestObjects)
            {
                await grpcCall.RequestStream.WriteAsync(requestBody);
                await Task.Delay(sendMillisecondsDelay);
            }

            await grpcCall.RequestStream.CompleteAsync();
            await readMessageTask;
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.logger is not null)
            {
                this.logger.AddLogMessage(
                    "GrpcLogMessage.json",
                    $"{userName},{method.Name},{label},{startMethodCall},{endMethodCall}",
                    typeof(GrpcLogMessage));
            }

            return responseMessages;
        }

        public void Dispose()
        {
            this._grpcChannel.Dispose();
        }
    }
}
