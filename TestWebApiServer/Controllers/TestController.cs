namespace TestWebApiServer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TestWebApiServer.Models;

    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> TestWaitMethod1([FromBody] TestRequestContent requestContent)
        {
            await Task.Delay(requestContent.Timeout);
            return this.Ok(new TestResponseContent { Text = $"Wait {requestContent.Timeout} ms" });
        }

        [HttpPost]
        public async Task<IActionResult> TestWaitMethod2([FromBody] TestRequestContent requestContent)
        {
            await Task.Delay(requestContent.Timeout);
            return this.Ok(new TestResponseContent { Text = $"Wait {requestContent.Timeout} ms" });
        }
    }
}
