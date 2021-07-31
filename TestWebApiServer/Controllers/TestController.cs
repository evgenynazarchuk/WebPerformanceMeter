using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApiServer.Models;

namespace TestWebApiServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> TestWaitMethod([FromBody] TestRequestContent requestContent)
        {
            await Task.Delay(requestContent.Timeout);
            return Ok(new TestResponseContent { Text = $"Wait {requestContent.Timeout} ms" });
        }

        [HttpPost]
        public IActionResult TestPersonMethod([FromBody] PersonInfo personInfo)
        {
            return Ok(personInfo);
        }

        [HttpGet]
        public IActionResult TestGetMethod()
        {
            return Ok("Hello world");
        }
    }
}
