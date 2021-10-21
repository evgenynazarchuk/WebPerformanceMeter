using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter
{
    public class Tool : ITool
    {
        public readonly ILogger? Logger;

        public Tool(ILogger? logger = null)
        { 
            this.Logger = logger;
        }
    }
}
