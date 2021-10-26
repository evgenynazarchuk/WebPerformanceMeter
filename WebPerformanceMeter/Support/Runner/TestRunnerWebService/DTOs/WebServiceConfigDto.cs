using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Runner
{
    public class WebServiceConfigDto
    {
        public int TestRunnerPort { get; set; }

        public string? LogServiceAddress { get; set; }
    }
}
