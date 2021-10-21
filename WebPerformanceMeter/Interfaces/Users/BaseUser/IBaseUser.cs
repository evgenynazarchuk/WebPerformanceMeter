using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface IBaseUser
    {
        string UserName { get; }

        ILogger? Logger { get; }

        void SetLogger(ILogger logger);

        void SetUserName(string userName);
    }
}
