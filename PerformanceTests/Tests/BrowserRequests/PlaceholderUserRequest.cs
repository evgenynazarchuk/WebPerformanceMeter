namespace PerformanceTests.Tests.BrowserRequests
{
    using System.Threading.Tasks;
    using WebPerformanceMeter.Users.HttpBrowser;

    public class PlaceholderUserRequest : BrowserRequest
    {
        protected override async Task PerformanceAsync()
        {
            await this.PostRequestAsync();
        }
    }
}
