using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.TestRunnerWebService
{
    public class TestMethodSimpleDto : TestMethodIdentityDto
    {
        public List<string?> ParametersNames { get; set; } = new();
    }
}
