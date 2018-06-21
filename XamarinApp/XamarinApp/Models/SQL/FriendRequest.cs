using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinApp.Models.SQL
{
    public class FriendRequest
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string SenderID { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverID { get; set; }
        public string ReceiverUsername { get; set; }
        public bool Accepted { get; set; }


        [Ignore]
        [JsonIgnore]
        public Command AcceptRequestCommand
        {
            get
            {
                return new Command<string>(async (SenderID) => await Connection.Friend.AcceptRequest(SenderID));
            }
                
            set { }
        }
        [Ignore]
        [JsonIgnore]
        public Command RejectRequestCommand
        {
            get
            {
                return new Command<string>(async (SenderID) => await Connection.Friend.RejectRequest(SenderID));
            }

            set { }
        }
    }
}
