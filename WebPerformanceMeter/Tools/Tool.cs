namespace WebPerformanceMeter.Tools
{
    using WebPerformanceMeter.Logger;

    public class Tool
    {
        protected Watcher Watcher => Watcher.Instance.Value;
    }
}
