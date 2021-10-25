using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.TestRunnerWebService;

namespace WebPerformanceMeter.TestRunnerWebService
{
    public class TestRunnertStatusDto : TestMethodSimpleDto
    {
        public long TestRunIdentifier { get; set; }

        public object[]? ParametersValues { get; set; } = null;

        public DateTime StartTime { get; set; }
    }
}
