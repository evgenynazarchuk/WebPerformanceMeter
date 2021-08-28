namespace PerformanceTests.Tests.BrowserRequests
{
    using System.Threading.Tasks;
    using WebPerformanceMeter.Users.Javascript;

    public class PlaceholderUserRequest : JavascriptUser
    {
        protected override async Task PerformanceAsync()
        {
            await this.PostRequestAsync();
        }
    }
}
