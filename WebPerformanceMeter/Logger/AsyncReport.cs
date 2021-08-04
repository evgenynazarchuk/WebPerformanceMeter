using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger
{
    public abstract class AsyncReport
    {
        public abstract Task WriteAsync(string message);
        public abstract Task WriteErrorAsync(string message);
        public virtual void Finish() { }
    }
}
