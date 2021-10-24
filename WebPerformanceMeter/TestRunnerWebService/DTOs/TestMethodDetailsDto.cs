using System.Collections.Generic;

namespace WebPerformanceMeter.TestRunnerWebService
{
    public class TestMethodDetailsDto : TestMethodSimpleDto
    {
        public List<object[]> ParametersValues { get; set; } = new();
    }
}
