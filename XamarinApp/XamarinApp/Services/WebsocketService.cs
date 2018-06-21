using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamarinApp.Models;
using Xamarin.Forms;
using System.Net.WebSockets;
using XamarinApp.Services.Interfaces;
using XamarinApp.Models.SQL;

namespace XamarinApp.Services
{
    public class WebsocketService
    {
        public delegate void ChatMessageEventHandler(object sender, MessageArgs args);
        public event ChatMessageEventHandler ChatMessage;

        public delegate void NewRequestEventHandler(object sender, RequestArgs args);
        public event NewRequestEventHandler NewFriendRequest;

        public delegate void AcceptRequestEventHandler(object sender, RequestArgs args);
        public event AcceptRequestEventHandler AcceptFriendRequest;

        private CancellationTokenSource ts;
        private CancellationToken ct;

        private ClientWebSocket _client;
        public WebsocketService()
        {
            ConnectToServer();
        }
        public async Task ConnectToServer()
        {
            _client = new ClientWebSocket();
            await _client.ConnectAsync(new Uri("ws://webapi20180614125427.azurewebsites.net/"), CancellationToken.None);
            var loginMessage = new
            {
                Type = "login",
                UserID = App.User.ID
            };
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(loginMessage)));
            await _client.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            ts = new CancellationTokenSource();
            ct = ts.Token;
            Task.Factory.StartNew(ListenForMessage, ct);
        }
        public async Task SendMessage(string message)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await _client.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        private async Task ListenForMessage()
        {
            while (true)
            {
                byte[] buffer = new byte[1024 * 4];
                while (_client.State == WebSocketState.Open)
                {
                    var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                    else
                    {
                        var stringMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var initMessage = JsonConvert.DeserializeObject<WebsocketInitalMessage>(stringMessage);
                        if (initMessage.Type == "newRequest")
                        {
                            var requestMessage = JsonConvert.DeserializeObject<FriendRequest>(stringMessage);
                            OnNewFriendRequest(requestMessage);

                        }
                        else if(initMessage.Type == "acceptRequest")
                        {
                            var requestMessage = JsonConvert.DeserializeObject<FriendRequest>(stringMessage);
                            OnAcceptFriendRequest(requestMessage);
                        }
                        else if(initMessage.Type == "chat")
                        {
                            var chatMessage = JsonConvert.DeserializeObject<WebsocketChatMessage>(stringMessage);
                            OnChatMessage(chatMessage);
                        }
                    }
                }
            }
        }
        public async void CloseConnection()
        {
            ts.Cancel();
            await _client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            //await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
        }
        protected virtual void OnNewFriendRequest(FriendRequest m)
        {
            if(NewFriendRequest != null)
            {
                RequestArgs args = new RequestArgs
                {
                    SenderID = m.SenderID,
                    SenderUsername = m.SenderUsername,
                    ReceiverID = m.ReceiverID,
                    ReceiverUsername = m.ReceiverUsername,
                    Accepted = m.Accepted
                };
                NewFriendRequest(this, args);
            }
        }
        protected virtual void OnAcceptFriendRequest(FriendRequest m)
        {
            if (AcceptFriendRequest != null)
            {
                RequestArgs args = new RequestArgs
                {
                    SenderID = m.SenderID,
                    SenderUsername = m.SenderUsername,
                    ReceiverID = m.ReceiverID,
                    ReceiverUsername = m.ReceiverUsername,
                    Accepted = m.Accepted
                };
                AcceptFriendRequest(this, args);
            }
        }
        protected virtual void OnChatMessage(WebsocketChatMessage m)
        {
            if(ChatMessage != null)
            {
                MessageArgs args = new MessageArgs
                {
                    SenderID = m.SenderID,
                    ReceiverID = m.ReceiverID,
                    Contents = m.Contents
                };
                ChatMessage(this, args);
            }
        }
    }
}
