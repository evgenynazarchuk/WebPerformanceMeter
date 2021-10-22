using System.Collections.Concurrent;
using System.IO;
using System.Text;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.DataReader
{
    public class DataReader<TData> : IDataReader<TData>
        where TData : class
    {
        protected readonly bool cyclicalData;

        protected readonly StreamReader reader;

        protected readonly ConcurrentQueue<TData> queue;

        public DataReader(string filePath, bool cyclicalData = false)
        {
            this.reader = new StreamReader(filePath, Encoding.UTF8, true, 65535);
            this.queue = new();
            this.cyclicalData = cyclicalData;
        }

        public TData? GetData()
        {
            if (this.queue is null)
            {
                return null;
            }

            this.queue.TryDequeue(out TData? result);

            // put again
            if (this.cyclicalData && result is not null)
            {
                this.queue.Enqueue(result);
            }

            return result;
        }
    }
}
