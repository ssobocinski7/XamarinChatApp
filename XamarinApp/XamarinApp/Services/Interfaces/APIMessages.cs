using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinApp.Models;
using XamarinApp.Models.SQL;

namespace XamarinApp.Services.Interfaces
{
    public class APIRequestMessage
    {
        [JsonProperty("Success")]
        public bool Success { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("SenderID")]
        public string SenderID { get; set; }
        [JsonProperty("SenderUsername")]
        public string SenderUsername { get; set; }
        [JsonProperty("ReceiverID")]
        public string ReceiverID { get; set; }
        [JsonProperty("ReceiverUsername")]
        public string ReceiverUsername { get; set; }
        [JsonProperty("Accepted")]
        public bool Accepted { get; set; }
    }
    public class APIPendingMessage
    {
        [JsonProperty("HasPending")]
        public bool HasPending { get; set; }
        [JsonProperty("List")]
        public List<FriendRequest> List { get; set; }
    }
    public class APIGetFriendsMessage
    {
        [JsonProperty("Success")]
        public bool Success{ get; set; }
        [JsonProperty("List")]
        public List<FriendRequest> List { get; set; }
    }
    public class APIAcceptRequestMessage
    {
        [JsonProperty("Success")]
        public bool Success { get; set; }
    }
    public class APIRejectRequestMessage
    {
        [JsonProperty("Success")]
        public bool Success { get; set; }
    }
    public class APIRegisterMessage
    {
        [JsonProperty("Success")]
        public bool Success { get; set; }
    }
}
