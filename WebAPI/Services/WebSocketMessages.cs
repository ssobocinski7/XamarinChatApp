using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class WebSocketInitalMessage
    {
        public string Type { get; set; }

    }
    public class WebSocketLoginMessage
    {
        public string Type { get; set; }
        public string UserID { get; set; }
    }
    public class WebSocketRequestMessage
    {
        public string Type { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
    }
    public class WebSocketChatMessage
    {
        public string Type { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string Contents { get; set; }
    }
}
