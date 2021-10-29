using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWebApplication.Models;
using GrpcWebApplication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GrpcWebApplication
{
    public class UserMessagerHandler : UserMessagerService.UserMessagerServiceBase
    {
        public UserMessagerHandler(
            ILogger<UserMessagerHandler> logger,
            DataContext dataContext)
        {
            this._logger = logger;
            this._dataContext = dataContext;
        }

        public override async Task<MessageIdentityDto> SendMessage(
            MessageCreateDto request,
            ServerCallContext context)
        {
            //var message = await this.dataContext.Set<Message>().AddAsync(new Message { Text = request.Text });
            //await this.dataContext.SaveChangesAsync();
            //var messageIdentityDto = new MessageIdentityDto { Id = message.Entity.Id };
            //
            //return messageIdentityDto;
            await Task.Delay(200);

            return await Task.FromResult(new MessageIdentityDto { Id = 1 });
        }

        public override async Task<Empty> SendMessages(
            IAsyncStreamReader<MessageCreateDto> requestStream,
            ServerCallContext context)
        {
            // read stream
            while (await requestStream.MoveNext())
            {
                await this._dataContext.Set<Message>().AddAsync(new Message
                {
                    Text = requestStream.Current.Text
                });
            }

            // save
            await this._dataContext.SaveChangesAsync();

            return new Empty();
        }

        public override async Task<MessageSimpleDto> GetMessage(
            MessageIdentityDto request,
            ServerCallContext context)
        {
            var message = await this._dataContext.Set<Message>().FindAsync(request.Id);
            var simpleDto = new MessageSimpleDto
            {
                Id = message.Id,
                Text = message.Text
            };

            return simpleDto;
        }

        public override async Task GetMessages(
            Empty request,
            IServerStreamWriter<MessageSimpleDto> responseStream,
            ServerCallContext context)
        {
            var messages = await this._dataContext.Set<Message>().ToListAsync();

            foreach (var message in messages)
            {
                await responseStream.WriteAsync(new MessageSimpleDto
                {
                    Id = message.Id,
                    Text = message.Text
                });
            }
        }

        public override async Task Messages(
            IAsyncStreamReader<MessageCreateDto> requestStream,
            IServerStreamWriter<MessageSimpleDto> responseStream,
            ServerCallContext context)
        {
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                await this._dataContext.Set<Message>().AddAsync(new Message
                {
                    Text = requestStream.Current.Text
                });
            }
            await this._dataContext.SaveChangesAsync();

            if (!context.CancellationToken.IsCancellationRequested)
            {
                var messagesTask = await this._dataContext.Set<Message>().ToListAsync();

                foreach (var message in messagesTask)
                {
                    await responseStream.WriteAsync(new MessageSimpleDto
                    {
                        Id = message.Id,
                        Text = message.Text
                    });
                }
            }
        }

        private readonly ILogger<UserMessagerHandler> _logger;

        private readonly DataContext _dataContext;
    }
}
