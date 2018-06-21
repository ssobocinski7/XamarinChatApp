using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Testing.ViewModels;
using XamarinApp.Models.SQL;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
    public class PendingRequestViewModel : BaseViewModel
    {
        public ObservableCollection<FriendRequest> Requests { get; set; }
        private WebsocketService _socket;
        public PendingRequestViewModel()
        {
            Requests = new ObservableCollection<FriendRequest>();
            _socket = App.Socket;
            _socket.NewFriendRequest += NewFriendRequest;
            Init();
        }
        public async Task Init()
        {
            var db = App.LocalDatabase;
            var _temp = await db.GetAllFriendRequests();

            foreach (var item in _temp)
            {
                Requests.Add(item);
            }
        }
        private async void NewFriendRequest(object sender, RequestArgs args)
        {
            var entry = new FriendRequest
            {
                SenderID = args.SenderID,
                SenderUsername = args.SenderUsername,
                ReceiverID = args.ReceiverID,
                ReceiverUsername = args.ReceiverUsername,
                Accepted = args.Accepted
            };
            Requests.Add(entry);
        }
    }
}
