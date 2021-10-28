using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter;

namespace GrpcWebApplication.PerformanceTests.Users
{
    public class UnaryGrpcUser : GrpcUser
    {
        public UnaryGrpcUser(string address, string? userName = null)
            : base(address, typeof(UserMessagerService.UserMessagerServiceClient), userName) 
        {
            //UseGrpcClient(typeof(UserMessagerService.UserMessagerServiceClient));
        }

        protected override async Task PerformanceAsync(GrpcClientTool client)
        {
            await UnaryCall<MessageIdentityDto, MessageCreateDto>(client, "SendMessageAsync", new MessageCreateDto { Text = "Hello world" });
        }
    }
}
