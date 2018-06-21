using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Testing.ViewModels;
using Xamarin.Forms;
using XamarinApp.Data;
using XamarinApp.Models;
using XamarinApp.Models.SQL;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public string UserName { get; set; }
        public string Requests { get; set; }
        public ObservableCollection<Friend> Friends { get; set; }
        public Command AddFriendCommand { get; set; }
        public Command PendingRequestCommand { get; set; }

        private WebsocketService _socket;
        public MainViewModel()
        {
            Friends = new ObservableCollection<Friend>();
            AddFriendCommand = new Command(async () => await NavigationService.NavigateTo(new AddFriendPage()));
            PendingRequestCommand = new Command(async () => await NavigationService.NavigateTo(new PendingRequestPage()));
            Init();
        }
        private async Task Init()
        {
            _socket = App.Socket;
            _socket.NewFriendRequest += NewFriendRequest;
            _socket.AcceptFriendRequest += AcceptFriendRequest;
            _socket.ChatMessage += NewChatMessage;

            await Connection.Friend.GetFriends();
            List<Friend> friends = await App.LocalDatabase.GetAllFriends();
            foreach(Friend f in friends)
            {
                Friends.Add(f);
            }
            await Connection.Friend.CheckIfHasPending();
        }
        private async void NewFriendRequest(object sender, RequestArgs args)
        {
            var db = App.LocalDatabase;
            var entry = new FriendRequest
            {
                SenderID = args.SenderID,
                SenderUsername = args.SenderUsername,
                ReceiverID = args.ReceiverID,
                ReceiverUsername = args.ReceiverUsername,
                Accepted = args.Accepted
            };
            await db.SaveItemAsync(entry);
        }
        private async void AcceptFriendRequest(object sender, RequestArgs args)
        {
            var db = App.LocalDatabase;
            string friendID = "";
            string friendUsername = "";
            if (args.ReceiverID == App.User.ID.ToString())
            {
                friendID = args.SenderID;
                friendUsername = args.SenderUsername;
            }
            else
            {
                friendID = args.ReceiverID;
                friendUsername = args.ReceiverUsername;
            }
            var entry = new Friend
            {
                UserID = friendID,
                UserName = friendUsername
            };
            await db.SaveItemAsync(entry);
            Friends.Add(entry);
        }
        
        private async void NewChatMessage(object sender, MessageArgs args)
        {
            var db = App.LocalDatabase;
            var entry = new ChatMessage
            {
                SenderID = args.SenderID,
                ReceiverID = args.ReceiverID,
                Contents = args.Contents
            };
            await db.SaveItemAsync(entry);
        }

    }
}
