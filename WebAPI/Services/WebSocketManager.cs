using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class WebSocketManager
    {
        public ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetWebSocketByKey(string key)
        {
            return _sockets
                    .Where(c => c.Key == key)
                    .FirstOrDefault()
                    .Value;
        }
        public string GetKey(WebSocket socket)
        {
            return _sockets
                    .Where(c => c.Value == socket)
                    .FirstOrDefault()
                    .Key;
        }
        public bool AddSocket(string key, WebSocket socket)
        {
            bool result = _sockets.TryAdd(key, socket);

            return result;
        }
        public async Task RemoveSocketAsync(string key)
        {
            WebSocket socket;
            _sockets.TryRemove(key, out socket);

            if (socket != null)
            {
                await socket.CloseAsync(
                    closeStatus: WebSocketCloseStatus.NormalClosure,
                    statusDescription: "Connection close by server.",
                    cancellationToken: CancellationToken.None
                    );
            }

        }
        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in _sockets)
            {
                if(pair.Value.State == WebSocketState.Open)
                {
                    await SendMessageAsync(pair.Value, message);
                }
            }
        }
        public async Task SendMessageAsync(WebSocket target, string message)
        {
            if (target.State != WebSocketState.Open)
                return;

            await target.SendAsync(
                        buffer:
                        new ArraySegment<byte>
                        (
                            array: Encoding.UTF8.GetBytes(message),
                            offset: 0,
                            count: message.Length
                        ),
                        messageType: WebSocketMessageType.Text,
                        endOfMessage: true,
                        cancellationToken: CancellationToken.None
                );
        }
    }
}
