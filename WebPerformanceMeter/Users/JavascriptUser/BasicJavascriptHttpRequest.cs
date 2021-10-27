using System.Threading.Tasks;

namespace WebPerformanceMeter.Users
{
    public abstract partial class BasicJavascriptUser : BasicUser
    {
        public async Task Post()
        {
            //
            await this.page.EvaluateAsync(@"
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
