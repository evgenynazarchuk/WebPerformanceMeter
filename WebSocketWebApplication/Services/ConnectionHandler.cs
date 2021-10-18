﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Net;
using System.Net.WebSockets;

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
            this._sockets.TryAdd(CreateConnectionId(), socket);
        }

        public async Task RemoveSocket(Guid id)
        {
            if (this._sockets.TryRemove(id, out WebSocket socket))
            {
                await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the Connection Manager",
                                    cancellationToken: CancellationToken.None);
            }
        }

        private Guid CreateConnectionId()
        {
            return Guid.NewGuid();
        }
    }
}
