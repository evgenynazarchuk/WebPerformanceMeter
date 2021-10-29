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

        protected override async Task PerformanceAsync()
        {
            await UnaryCall<MessageIdentityDto, MessageCreateDto>("SendMessageAsync", new MessageCreateDto { Text = "Hello world" });
        }
    }
}
