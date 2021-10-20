using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketWebApplication.Services
{
    public class ConnectionHandler : IConnectionHandler
    {
        private readonly ConcurrentDictionary<Guid, WebSocket> _sockets;

        public ConnectionHandler()
        {
            this._sockets = new();
        }

        public IEnumerable<WebSocket> GetAllSocket()
        {
            return this._sockets.Values;
        }

        public Guid GetSocketId(WebSocket socket)
        {
            return this._sockets.FirstOrDefault(x => x.Value == socket).Key;
        }

        public WebSocket GetSocketById(Guid id)
        {
            return this._sockets.FirstOrDefault(x => x.Key == id).Value;
        }

        public void AddSocket(WebSocket socket)
        {
            this._sockets.TryAdd(Guid.NewGuid(), socket);
        }

        public Task RemoveSocketAsync(Guid id)
        {
            if (this._sockets.TryRemove(id, out WebSocket socket))
            {
                return socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the Connection Manager",
                                    cancellationToken: CancellationToken.None);
            }

            return Task.CompletedTask;
        }
    }
}
