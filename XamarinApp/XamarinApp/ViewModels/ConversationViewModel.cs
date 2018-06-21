using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Testing.ViewModels;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class ConversationViewModel : BaseViewModel
    {
        public WebsocketService _socket;
        private string FriendID;
        private ConversationPage Page;
        public string FriendUsername { get; set; }

        private string _messageContents;
        public string MessageContents
        {
            get
            {
                return _messageContents;
            }
            set
            {
                _messageContents = value;
                RaisePropertyChanged(MessageContents);
            }
         }
        public Command SendMessageCommand { get; set; }
        public ConversationViewModel(string friendID, ConversationPage page)
        {
            FriendID = friendID;
            FriendUsername = "";
            MessageContents = "";
            SendMessageCommand = new Command(SendMessage);
            _socket = App.Socket;
            _socket.ChatMessage += GetNewMessage;
            Page = page;
            Init();
        }
        private async void Init()
        {
            var db = App.LocalDatabase;
            FriendUsername = await db.GetFriendUsername(FriendID);
        }
        private async void GetNewMessage(object sender, MessageArgs args)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var message = new WebsocketChatMessage
                {
                    Type = "chat",
                    SenderID = args.SenderID,
                    ReceiverID = args.ReceiverID,
                    Contents = args.Contents
                };
                Page.drawNewMessage(message);
            });
        }
        private async void SendMessage()
        {
            var message = new WebsocketChatMessage
            {
                Type = "chat",
                SenderID = App.User.ID.ToString(),
                ReceiverID = FriendID,
                Contents = MessageContents
            };
            var entry = new ChatMessage
            {
                SenderID = message.SenderID,
                ReceiverID = message.ReceiverID,
                Contents = message.Contents
            };
            var db = App.LocalDatabase;
            await db.SaveItemAsync(entry);
            await _socket.SendMessage(JsonConvert.SerializeObject(message));
            Page.drawNewMessage(message);

        }
       
    }
}
