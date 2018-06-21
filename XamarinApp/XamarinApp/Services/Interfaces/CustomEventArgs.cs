using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApp.Services.Interfaces
{
    public class MessageArgs : EventArgs
    {
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string Contents { get; set; }
    }
    public class RequestArgs : EventArgs
    {
        public string SenderID { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverID { get; set; }
        public string ReceiverUsername { get; set; }
        public bool Accepted { get; set; }
    }
}
