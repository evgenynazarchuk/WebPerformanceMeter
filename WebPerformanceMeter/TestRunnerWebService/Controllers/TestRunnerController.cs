using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebPerformanceMeter.TestRunnerWebService
{
    [Controller]
    [Route("[controller]/[action]")]
    public class TestRunnerController : ControllerBase
    {
        private TestRunner _testRunner;

        public TestRunnerController(TestRunner testRunner)
        { 
            this._testRunner = testRunner;
        }

        [HttpGet]
        public IActionResult GetIdentitiesTests()
        {
            var tests = this._testRunner.GetIdentitiesTests();
            return Ok(tests);
        }

        [HttpGet]
        public IActionResult GetTestDetail(string testClassName, string testClassMethod)
        {
            var test = this._testRunner.GetTestDetail(testClassName, testClassMethod);
            return Ok(test);
        }

        [HttpGet]
        public IActionResult GetTestsDetails()
        {
            var testsDetails = this._testRunner.GetTestsDetails();
            return Ok(testsDetails);
        }

        [HttpPost]
        public async Task<IActionResult> StartTest([FromBody] StartTestMethodDto startTestDto)
        {
            await this._testRunner.StartTestAsync(startTestDto);
            return Ok();
        }
    }
}
