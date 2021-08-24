namespace WebPerformanceMeter.Logger
{
    using System.Threading.Tasks;

    public abstract class AsyncReport
    {
        public abstract Task WriteAsync(string message);
        public virtual void Finish() { }
    }
}
