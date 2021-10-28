using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPerformanceMeter;

namespace GrpcWebApplication.PerformanceTests.Users
{
    public class ClientStreamUser : GrpcUser
    {
        public ClientStreamUser(string address, string? userName = null)
            : base(address, typeof(UserMessagerService.UserMessagerServiceClient), userName)
        {
            UseGrpcClient(typeof(UserMessagerService.UserMessagerServiceClient));
        }

        protected override async Task PerformanceAsync(GrpcClientTool client)
        {
            var messages = new List<MessageCreateDto>
            {
                new MessageCreateDto { Text = "test 1" },
                new MessageCreateDto { Text = "test 2" },
                new MessageCreateDto { Text = "test 3" },
                new MessageCreateDto { Text = "test 4" },
                new MessageCreateDto { Text = "test 5" },
                new MessageCreateDto { Text = "test 6" },
                new MessageCreateDto { Text = "test 7" },
                new MessageCreateDto { Text = "test 8" },
                new MessageCreateDto { Text = "test 9" },
            };

            await ClientStream<Empty, MessageCreateDto>(client, "SendMessages", messages);
        }
    }
}
