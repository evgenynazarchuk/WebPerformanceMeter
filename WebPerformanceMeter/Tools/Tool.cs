using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.Tools
{
    public class Tool : ITool
    {
        protected readonly ILogger? logger;

        public ILogger? Logger { get => this.logger; }

        public Tool(ILogger? logger = null)
        {
            this.logger = logger;
        }
    }
}
