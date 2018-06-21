using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class FriendLink
    {
        [Key]
        public int ID { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public bool Accepted { get; set; }
    }
}
