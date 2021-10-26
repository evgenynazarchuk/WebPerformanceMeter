using System.Collections.Generic;

namespace WebPerformanceMeter.Runner
{
    public class TestMethodDetailsDto : TestMethodSimpleDto
    {
        public List<object[]> ParametersValues { get; set; } = new();
    }
}
