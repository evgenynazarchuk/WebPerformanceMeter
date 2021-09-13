namespace GrpcWebApplication
{
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using GrpcWebApplication.Models;
    using GrpcWebApplication.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class UserMessagerService : UserMessager.UserMessagerBase
    {
        protected readonly ILogger<UserMessagerService> logger;

        protected readonly WritableDataAccess writableDataAccess;

        protected readonly ReadableDataAccess readableDataAccess;

        public UserMessagerService(ILogger<UserMessagerService> logger,
            WritableDataAccess writableDataAccess,
            ReadableDataAccess readableDataAccess)
        {
            this.logger = logger;
            this.writableDataAccess = writableDataAccess;
            this.readableDataAccess = readableDataAccess;
        }

        public override async Task<Empty> SendMessage(
            MessageRequest request,
            ServerCallContext context)
        {
            await this.writableDataAccess.Set<Message>().AddAsync(new Message
            {
                Text = request.Text
            });
            await this.writableDataAccess.SaveChangesAsync();

            return new Empty();
        }

        public override async Task<Empty> SendMessages(
            IAsyncStreamReader<MessageRequest> requestStream,
            ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                await this.writableDataAccess.Set<Message>().AddAsync(new Message
                {
                    Text = requestStream.Current.Text
                });


            }
            await this.writableDataAccess.SaveChangesAsync();

            return new Empty();
        }

        public override async Task GetMessages(
            Empty request,
            IServerStreamWriter<MessageReply> responseStream,
            ServerCallContext context)
        {
            var messages = await this.readableDataAccess.Set<Message>().ToListAsync();

            foreach (var message in messages)
            {
                await responseStream.WriteAsync(new MessageReply
                {
                    Text = message.Text
                });
            }
        }

        public override async Task Messages(
            IAsyncStreamReader<MessageRequest> requestStream,
            IServerStreamWriter<MessageReply> responseStream,
            ServerCallContext context)
        {
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                await this.writableDataAccess.Set<Message>().AddAsync(new Message
                {
                    Text = requestStream.Current.Text
                });

                await this.writableDataAccess.SaveChangesAsync();
            }

            if (!context.CancellationToken.IsCancellationRequested)
            {
                var messagesTask = await this.readableDataAccess.Set<Message>().ToListAsync();

                foreach (var message in messagesTask)
                {
                    await responseStream.WriteAsync(new MessageReply
                    {
                        Text = message.Text
                    });
                }
            }
        }
    }
}
