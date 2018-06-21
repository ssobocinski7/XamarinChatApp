using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class APIPendingMessage
    {
        public bool HasPending { get; set; }
        public List<FriendRequest> List { get; set; }
    }
}
