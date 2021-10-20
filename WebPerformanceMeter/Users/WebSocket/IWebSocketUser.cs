using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter.Logger;
using WebPerformanceMeter.Tools.WebSocketTool;

namespace WebPerformanceMeter.Users.WebSocket
{
    public interface IWebSocketUser : IUser
    {
        void SetClientBuffer(int receiveBufferSize = 1024, int sendBufferSize = 1024);
    }
}
