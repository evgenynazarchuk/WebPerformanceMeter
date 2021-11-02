using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebPerformanceMeter.Reports;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Tools;

namespace WebPerformanceMeter
{
    public sealed class GrpcClientTool : Tool, IDisposable
    {
        public GrpcClientTool(string address, Type serviceClientType, Watcher watcher)
            : base(watcher)
        {
            this._grpcChannel = GrpcChannel.ForAddress(address);

            var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

            if (ctor is null)
            {
                throw new ApplicationException("gRpc client is not found");
            }

            this._grpcClient = ctor.Invoke(new[] { this._grpcChannel });

            if (this._grpcClient is null)
            {
                throw new ApplicationException("gRpc client is not create");
            }
        }

        public GrpcClientTool(GrpcChannel channel, Type serviceClientType, Watcher watcher)
            : base(watcher)

        {
            this._grpcChannel = channel;

            var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

            if (ctor is null)
            {
                throw new ApplicationException("gRpc client is not found");
            }

            this._grpcClient = ctor.Invoke(new[] { this._grpcChannel });

            if (this._grpcClient is null)
            {
                throw new ApplicationException("gRpc client is not create");
            }
        }

        public GrpcClientTool(HttpClient httpClient, Type grpcClientType, Watcher watcher)
            : base(watcher)
        {
            if (httpClient.BaseAddress is null)
            {
                throw new ApplicationException("Http Client Base address must be set");
            }

            this._grpcChannel = GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = httpClient,
                DisposeHttpClient = true
            });

            var ctor = grpcClientType.GetConstructor(new[] { typeof(GrpcChannel) });

            if (ctor is null)
            {
                throw new ApplicationException("gRpc client is not found");
            }

            this._grpcClient = ctor.Invoke(new[] { this._grpcChannel });

            if (this._grpcClient is null)
            {
                throw new ApplicationException("gRpc client is not create");
            }
        }

        public async ValueTask<ActionResult<TResponse>> UnaryCallAsync<TResponse, TRequest>(
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
                .Single(x => x.GetParameters().Length == 4);

            long startMethodCall;
            long endMethodCall;
            startMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            //
            AsyncUnaryCall<TResponse>? grpcCall = null;

            try
            {
                grpcCall = (AsyncUnaryCall<TResponse>?)method.Invoke(this._grpcClient, new object[] { requestBody, default!, default!, default! });
            }
            catch (Exception e)
            {
                Console.WriteLine("error invoke grpcCall: " + e.Message);
                return new ActionResult<TResponse>(e.Message);
            }

            if (grpcCall is null)
            {
                return new ActionResult<TResponse>("gRPC is not created");
            }

            TResponse? response = null;
            try
            {
                response = await grpcCall;
            }
            catch (Exception e)
            {
                Console.WriteLine("error await grpcCall: " + e.Message);
                return new ActionResult<TResponse>(e.Message);
            }
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.Watcher is not null)
            {
                this.Watcher.SendMessage(
                    "GrpcLogMessage.json",
                    $"{userName},{method.Name},{label},{startMethodCall},{endMethodCall}",
                    typeof(GrpcLogMessage));
            }

            grpcCall.Dispose();

            return new ActionResult<TResponse>(response);
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
                .Single(x => x.GetParameters().Length == 3);

            long startSendMessage;
            long endSendMessage;
            long startMethodCall;
            long endMethodCall;
            startMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            //
            using var grpcCall = (AsyncClientStreamingCall<TRequest, TResponse>?)method.Invoke(this._grpcClient, new object[] { default!, default!, default! });

            if (grpcCall is null)
            {
                throw new ApplicationException("gRPC call error");
            }

            foreach (var requestBody in requestObjects)
            {
                startSendMessage = ScenarioTimer.Time.Elapsed.Ticks;
                await grpcCall.RequestStream.WriteAsync(requestBody);
                endSendMessage = ScenarioTimer.Time.Elapsed.Ticks;

                if (this.Watcher is not null)
                {
                    this.Watcher.SendMessage(
                        "GrpcLogMessage.json",
                        $"{userName},send,{label},{startSendMessage},{endSendMessage}",
                        typeof(GrpcLogMessage));
                }

                await Task.Delay(millisecondsDelay);
            }

            await grpcCall.RequestStream.CompleteAsync();

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.Watcher is not null)
            {
                this.Watcher.SendMessage(
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
                .Single(x => x.GetParameters().Length == 4);

            long startReadMessage;
            long endReadMessage;
            long startMethodCall;
            long endMethodCall;
            startMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            using var grpcCall = (AsyncServerStreamingCall<TResponse>?)method.Invoke(this._grpcClient, new object[] { requestBody, default!, default!, default! });
            if (grpcCall is null)
            {
                throw new ApplicationException("gRPC call error");
            }

            var messages = new List<TResponse>();

            while (true)
            {
                startReadMessage = ScenarioTimer.Time.Elapsed.Ticks;
                if (!await grpcCall.ResponseStream.MoveNext())
                {
                    break;
                }
                endReadMessage = ScenarioTimer.Time.Elapsed.Ticks;

                if (this.Watcher is not null)
                {
                    this.Watcher.SendMessage(
                        "GrpcLogMessage.json",
                        $"{userName},receive,{label},{startReadMessage},{endReadMessage}",
                        typeof(GrpcLogSendMessage));
                }

                messages.Add(grpcCall.ResponseStream.Current);
                await Task.Delay(millisecondsDelay);
            }
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.Watcher is not null)
            {
                this.Watcher.SendMessage(
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
                .Single(x => x.GetParameters().Length == 3);

            long startReadMessage;
            long endReadMessage;
            long startSendMessage;
            long endSendMessage;
            long startMethodCall;
            long endMethodCall;

            startMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            //
            using var grpcCall = (AsyncDuplexStreamingCall<TRequest, TResponse>?)method.Invoke(this._grpcClient, new object[] { default!, default!, default! });
            if (grpcCall is null)
            {
                throw new ApplicationException("gRPC call error");
            }

            var responseMessages = new List<TResponse>();

            var readMessageTask = Task.Run(async () =>
            {
                startReadMessage = ScenarioTimer.Time.Elapsed.Ticks;
                await foreach (var responseMessage in grpcCall.ResponseStream.ReadAllAsync())
                {
                    endReadMessage = ScenarioTimer.Time.Elapsed.Ticks;

                    if (this.Watcher is not null)
                    {
                        this.Watcher.SendMessage(
                            "GrpcLogMessage.json",
                            $"{userName},receive,{label},{startReadMessage},{endReadMessage}",
                            typeof(GrpcLogMessage));
                    }

                    responseMessages.Add(responseMessage);
                    await Task.Delay(readMillisecondsDelay);

                    startReadMessage = ScenarioTimer.Time.Elapsed.Ticks;
                }
            });


            foreach (var requestBody in requestObjects)
            {
                startSendMessage = ScenarioTimer.Time.Elapsed.Ticks;
                await grpcCall.RequestStream.WriteAsync(requestBody);
                endSendMessage = ScenarioTimer.Time.Elapsed.Ticks;

                if (this.Watcher is not null)
                {
                    this.Watcher.SendMessage(
                        "GrpcLogMessage.json",
                        $"{userName},send,{label},{startSendMessage},{endSendMessage}",
                        typeof(GrpcLogMessage));
                }

                await Task.Delay(sendMillisecondsDelay);
            }

            await grpcCall.RequestStream.CompleteAsync();
            await readMessageTask;
            //

            endMethodCall = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.Watcher is not null)
            {
                this.Watcher.SendMessage(
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

        private readonly GrpcChannel _grpcChannel;

        private readonly object _grpcClient;
    }
}
