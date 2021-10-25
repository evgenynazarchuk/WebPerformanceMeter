using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace WebPerformanceMeter.TestRunnerWebService
{
    [Controller]
    [Route("[controller]/[action]")]
    public class TestRunnerController : ControllerBase
    {
        private TestRunnerService _testRunner;

        public TestRunnerController(TestRunnerService testRunner)
        {
            this._testRunner = testRunner;
        }

        //[HttpGet]
        //public IActionResult GetTestsIdentifiers()
        //{
        //    var testsIdentifiers = this._testRunner.GetTestsIdentifiers();

        //    return Ok(testsIdentifiers);
        //}

        //[HttpGet]
        //public IActionResult GetTestDetail(string testClassName, string testClassMethod)
        //{
        //    var testDetail = this._testRunner.GetTestDetail(testClassName, testClassMethod);

        //    return Ok(testDetail);
        //}

        [HttpGet]
        public IActionResult GetTestsDetails()
        {
            var testsDetails = this._testRunner.GetTestsDetails();

            return Ok(testsDetails);
        }

        [HttpPost]
        public async Task<IActionResult> StartTest([FromBody] StartTestMethodDto startTestDto)
        {
            try
            {
                await this._testRunner.StartTestAsync(startTestDto);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
            return Ok();
        }

        //[HttpGet]
        //public IActionResult GetSimpleStatus()
        //{
        //    var status = this._testRunner.GetStatus();

        //    if (status is not null)
        //    {
        //        return BadRequest("Test Runner is busy");
        //    }
        //    else
        //    {
        //        return Ok("Test Runner is available");
        //    }
        //}

        [HttpGet]
        public IActionResult GetStatus()
        {
            var status = this._testRunner.GetStatus();

            if (status is not null)
            {
                return BadRequest(status);
            }
            else
            {
                return Ok("Test Runner is available");
            }
        }
    }
}
