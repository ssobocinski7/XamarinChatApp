using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApp.Services
{
    public interface IWebSocketMessage
    {
        string Type { get; set; }
    }
    public class WebsocketInitalMessage : IWebSocketMessage
    {
        public string Type { get; set; }

    }
    public class WebsocketFriendRequestMessage : IWebSocketMessage
    {
        public string Type { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }

    }
    public class WebsocketChatMessage : IWebSocketMessage
    {
        public string Type { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string Contents { get; set; }
    }
}
