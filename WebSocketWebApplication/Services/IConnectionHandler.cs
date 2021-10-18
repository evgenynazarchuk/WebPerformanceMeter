using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace WebSocketWebApplication.Services
{
    public interface IConnectionHandler
    {
        IEnumerable<WebSocket> GetAllSocket();

        Guid GetSocketId(WebSocket socket);

        WebSocket GetSocketById(Guid id);

        void AddSocket(WebSocket socket);

        Task RemoveSocket(Guid id);
    }
}
