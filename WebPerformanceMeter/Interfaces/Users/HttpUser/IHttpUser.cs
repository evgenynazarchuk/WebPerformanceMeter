using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Tools.HttpTool;

namespace WebPerformanceMeter.Interfaces
{
    public interface IHttpUser : IBaseHttpUser, IUser
    {
    }
}
