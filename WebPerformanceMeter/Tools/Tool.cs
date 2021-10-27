using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Reports;

namespace WebPerformanceMeter.Tools
{
    public class Tool
    {
        public readonly Watcher Watcher;

        public Tool(Watcher watcher)
        {
            this.Watcher = watcher;
        }
    }
}
