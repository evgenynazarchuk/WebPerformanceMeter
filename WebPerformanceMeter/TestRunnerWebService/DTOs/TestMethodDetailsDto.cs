using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.TestRunnerWebService
{
    public class TestMethodDetailsDto : TestMethodSimpleDto
    {
        public List<object[]> ParametersValues { get; set; } = new();
    }
}
