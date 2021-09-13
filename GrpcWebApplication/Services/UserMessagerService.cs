namespace GrpcWebApplication
{
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Google.Protobuf.WellKnownTypes;
    using GrpcWebApplication.Models;
    using GrpcWebApplication.Services;
    using Microsoft.EntityFrameworkCore;
    
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
            var requestTask = Task.Run(async () =>
            {
                while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
                {
                    await this.writableDataAccess.Set<Message>().AddAsync(new Message
                    {
                        Text = requestStream.Current.Text
                    });
            
                    await this.writableDataAccess.SaveChangesAsync();
                }
            });
            
            var responseTask = Task.Run(async () =>
            {
                while (!context.CancellationToken.IsCancellationRequested)
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
            });

            //while (await requestStream.MoveNext())
            //{
            //    var message = requestStream.Current;
            //    //List<RouteNote> prevNotes = AddNoteForLocation(note.Location, note);
            //    var messages = await this.readableDataAccess.Set<Message>().ToListAsync();
            //    messages.Add(new Message { Id = message.});
            //    foreach (var prevNote in prevNotes)
            //    {
            //        await responseStream.WriteAsync(prevNote);
            //    }
            //}


            await requestTask;
            await responseTask;
        }
    }
}
