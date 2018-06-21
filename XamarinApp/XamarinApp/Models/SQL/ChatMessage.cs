using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApp.Models
{
    public class ChatMessage
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string Contents { get; set; }
    }
}
