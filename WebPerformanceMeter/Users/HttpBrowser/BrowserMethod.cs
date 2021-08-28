namespace WebPerformanceMeter.Users.HttpBrowser
{
    using System.Threading.Tasks;

    public abstract partial class BrowserRequest : PerformanceUser
    {
        public async Task PostRequestAsync()
        {
            await this.Page.EvaluateAsync(@"
fetch('https://jsonplaceholder.typicode.com/posts', {
  method: 'POST',
  body: JSON.stringify({
    title: 'foo',
    body: 'bar',
    userId: 1,
  }),
  headers: {
    'Content-type': 'application/json; charset=UTF-8',
  },
}).then((response) => response.json());
");
        }
    }
}
