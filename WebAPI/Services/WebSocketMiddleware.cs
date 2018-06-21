using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketManager _socketManager;

        public WebSocketMiddleware(RequestDelegate next, WebSocketManager manager)
        {
            _next = next;
            _socketManager = manager;
        }
        public async Task Invoke(HttpContext context)
        {
            if(!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }
            
            var socket = await context.WebSockets.AcceptWebSocketAsync();

            await Receive(socket, async (result, buffer) => 
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    WebSocketInitalMessage messageObj = JsonConvert.DeserializeObject<WebSocketInitalMessage>(message);

                    if(messageObj.Type == "login")
                    {
                        WebSocketLoginMessage loginMessage = JsonConvert.DeserializeObject<WebSocketLoginMessage>(message);
                        var test = _socketManager.GetWebSocketByKey(loginMessage.UserID);
                        if(test != null)
                        {
                            await _socketManager.RemoveSocketAsync(loginMessage.UserID);
                        }
                        _socketManager.AddSocket(loginMessage.UserID, socket);
                    }
                    else if(messageObj.Type == "newRequest")
                    {
                        WebSocketRequestMessage r = JsonConvert.DeserializeObject<WebSocketRequestMessage>(message);
                        var target = _socketManager.GetWebSocketByKey(r.ReceiverID);
                        if (target != null)
                        {
                            var db = new DataContext();
                            string senderUsername = db.Users.Where(c => r.SenderID == c.ID.ToString()).FirstOrDefault().UserName;
                            string receiverUsername = db.Users.Where(c => r.ReceiverID == c.ID.ToString()).FirstOrDefault().UserName;
                            var msg = new
                            {
                                Type = "newRequest",
                                SenderID = r.SenderID,
                                SenderUsername = senderUsername,
                                ReceiverID = r.ReceiverID,
                                ReceiverUsername = receiverUsername,
                                Accepted = false
                            };
                            string jsonmsg = JsonConvert.SerializeObject(msg);
                            await _socketManager.SendMessageAsync(target, jsonmsg);
                        }
                    }
                    else if(messageObj.Type == "acceptRequest")
                    {
                        WebSocketRequestMessage r = JsonConvert.DeserializeObject<WebSocketRequestMessage>(message);
                        var target = _socketManager.GetWebSocketByKey(r.SenderID);
                        if(target != null)
                        {
                            var db = new DataContext();
                            string senderUsername = db.Users.Where(c => r.SenderID == c.ID.ToString()).FirstOrDefault().UserName;
                            string receiverUsername = db.Users.Where(c => r.ReceiverID == c.ID.ToString()).FirstOrDefault().UserName;
                            var msg = new
                            {
                                Type = "acceptRequest",
                                SenderID = r.SenderID,
                                SenderUsername = senderUsername,
                                ReceiverID = r.ReceiverID,
                                ReceiverUsername = receiverUsername,
                                Accepted = true
                            };
                            string jsonmsg = JsonConvert.SerializeObject(msg);
                            await _socketManager.SendMessageAsync(target, jsonmsg);
                        }
                        
                    }
                    else if(messageObj.Type == "chat")
                    {
                        WebSocketChatMessage chatMessage = JsonConvert.DeserializeObject<WebSocketChatMessage>(message);
                        WebSocket target = _socketManager.GetWebSocketByKey(chatMessage.ReceiverID);
                        if (target != null)
                        {
                            var respond = new WebSocketChatMessage
                            {
                                Type = "chat",
                                SenderID = chatMessage.SenderID,
                                ReceiverID = chatMessage.ReceiverID,
                                Contents = chatMessage.Contents
                            };
                            await _socketManager.SendMessageAsync(target, JsonConvert.SerializeObject(respond));
                        }
                    }
                    else if(messageObj.Type == "test")
                    {
                        await _socketManager.SendMessageToAllAsync(JsonConvert.SerializeObject(_socketManager._sockets));
                    }
                    
                }
                if(result.MessageType == WebSocketMessageType.Close)
                {
                    String key = _socketManager.GetKey(socket);
                    if (key != null)
                    {
                        await _socketManager.RemoveSocketAsync(key);
                    }
                }
            });

        }
        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while(socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(
                            buffer: new ArraySegment<byte>(buffer),
                            cancellationToken: CancellationToken.None
                            );

                handleMessage(result, buffer);
            }
        }
    }
}
