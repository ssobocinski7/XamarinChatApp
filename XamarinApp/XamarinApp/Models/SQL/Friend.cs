using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.ViewModels;
using XamarinApp.Views;

namespace XamarinApp.Models.SQL
{
    public class Friend
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        
        [Ignore]
        public Command OpenConversationCommand {
            get
            {
                return new Command<string>(async (f) => await NavigationService.NavigateTo(new ConversationPage(f)));

            }
            set { }
        }

    }
}
