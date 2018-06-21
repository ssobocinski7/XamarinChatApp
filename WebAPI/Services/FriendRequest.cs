using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class FriendRequest
    {
        public int ID { get; set; }
        public string SenderID { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverID { get; set; }
        public string ReceiverUsername { get; set; }
        public bool Accepted { get; set; }
    }
}
