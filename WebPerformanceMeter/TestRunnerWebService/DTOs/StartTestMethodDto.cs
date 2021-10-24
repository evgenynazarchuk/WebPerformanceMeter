using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebPerformanceMeter.TestRunnerWebService
{
    public class StartTestMethodDto : TestMethodIdentityDto
    {
        //[JsonConverter(typeof(object?[]))]
        public object[]? ParametersValues { get; set; } = null;
    }
}
