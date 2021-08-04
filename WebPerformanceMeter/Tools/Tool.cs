using WebPerformanceMeter.Logger;

namespace WebPerformanceMeter.Tools
{
    public class Tool
    {
        protected Watcher Watcher => Watcher.Instance.Value;
    }
}
