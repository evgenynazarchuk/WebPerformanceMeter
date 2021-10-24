using System.Collections.Generic;

namespace WebPerformanceMeter.TestRunnerWebService
{
    public class TestMethodSimpleDto : TestMethodIdentityDto
    {
        public List<string?> ParametersNames { get; set; } = new();
    }
}
