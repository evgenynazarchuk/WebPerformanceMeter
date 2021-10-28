using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Support
{
    public class ActionResult<TResult>
        where TResult : class
    {
        public ActionResult(TResult value)
        {
            this.Value = value;
        }

        public ActionResult(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public readonly TResult? Value = null;

        public readonly string? ErrorMessage = null;

        public bool IsError => this.ErrorMessage is not null;
    }
}
