namespace WebPerformanceMeter.Runner
{
    public class StartTestMethodDto : TestMethodIdentityDto
    {
        public long TestRunIdentifier { get; set; }

        public object[]? ParametersValues { get; set; } = null;
    }
}
