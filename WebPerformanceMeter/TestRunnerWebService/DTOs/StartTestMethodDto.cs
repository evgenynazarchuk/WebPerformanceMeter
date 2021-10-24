namespace WebPerformanceMeter.TestRunnerWebService
{
    public class StartTestMethodDto : TestMethodIdentityDto
    {
        //[JsonConverter(typeof(object?[]))]
        public object[]? ParametersValues { get; set; } = null;
    }
}
