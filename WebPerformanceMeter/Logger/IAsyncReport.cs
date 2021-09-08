namespace WebPerformanceMeter.Logger
{
    using System.Threading.Tasks;

    public interface IAsyncReport
    {
        public abstract Task WriteAsync(string fileName, string message);
        public virtual void Finish() { }
    }
}
