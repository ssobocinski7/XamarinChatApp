using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Testing.ViewModels;
using Xamarin.Forms;
using XamarinApp.Connection;

namespace XamarinApp.ViewModels
{
    public class AddFriendViewModel : BaseViewModel
    {
        public string EntryUserName { get; set; }
        public Command SendRequestCommand { get; set; }
        public AddFriendViewModel()
        {
            EntryUserName = "";
            SendRequestCommand = new Command(async () => await SendRequest(EntryUserName));

        }
        public async Task SendRequest(string Username)
        {
            await Friend.SendRequest(Username);
        }
    }
}
