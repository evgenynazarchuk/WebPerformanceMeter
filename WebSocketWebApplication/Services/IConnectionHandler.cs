using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

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
