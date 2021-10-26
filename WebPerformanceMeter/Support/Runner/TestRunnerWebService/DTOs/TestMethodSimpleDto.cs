using System.Collections.Generic;

namespace WebPerformanceMeter.Runner
{
    public class TestMethodSimpleDto : TestMethodIdentityDto
    {
        public List<string?> ParametersNames { get; set; } = new();
    }
}
