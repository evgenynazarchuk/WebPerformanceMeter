using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Logger
{
    public class BytesCount
    {
        public long EndResponse { get; set; }

        public int Count { get; set; }

        public BytesCount(long EndResponse, int count)
        {
            this.EndResponse = EndResponse;
            Count = count;
        }
    }
}
