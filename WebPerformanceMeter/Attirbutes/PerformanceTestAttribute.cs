using System;

namespace WebPerformanceMeter.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PerformanceTestAttribute : Attribute
    {
        public object[]? Parameters { get; private set; } = null;

        public PerformanceTestAttribute() { }

        public PerformanceTestAttribute(params object[] parametrs)
        {
            Parameters = parametrs;
        }
    }
}
