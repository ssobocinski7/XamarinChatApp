using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.Models;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConversationPage : ContentPage
	{
        private ConversationViewModel _viewModel;
        private string FriendID;
		public ConversationPage (string friendID)
		{
            _viewModel = new ConversationViewModel(friendID, this);
            InitializeComponent();
            BindingContext = _viewModel;
            FriendID = friendID;
            MessagesScrollView.Content.SizeChanged += scrollToBottom;
            drawMessages();
            
        }

        private async void scrollToBottom(object sender, EventArgs e)
        {
            await MessagesScrollView.ScrollToAsync(0, MessagesScrollView.Content.Height, false);
        }

        private async Task drawMessages()
        {
            var db = App.LocalDatabase;
            var result = await db.GetLastMessagesWithFriend(FriendID);
            var layout = this.FindByName<StackLayout>("MessagesView");
            foreach (var item in result)
            {
                var frame = new Frame
                {
                    Padding = 10,
                    CornerRadius = 10
                };
                var label = new Label
                {
                    Text = item.Contents,

                };
                if (item.SenderID == App.User.ID.ToString())
                {
                    frame.HorizontalOptions = LayoutOptions.End;
                    frame.BackgroundColor = (Color)Application.Current.Resources["ThirdBackgroundColor"];
                }
                else
                {
                    frame.HorizontalOptions = LayoutOptions.Start;
                    frame.BackgroundColor = (Color)Application.Current.Resources["SecondaryBackgroundColor"];
                    label.TextColor = (Color)Application.Current.Resources["PrimaryFontColor"];
                }
                frame.Content = label;
                layout.Children.Add(frame);

            }

        }

        public void drawNewMessage(WebsocketChatMessage m)
        {
            var frame = new Frame
            {
                Padding = 10,
                CornerRadius = 10
            };
            var label = new Label
            {
                Text = m.Contents
            };
            if (m.SenderID == App.User.ID.ToString())
            {
                frame.HorizontalOptions = LayoutOptions.End;
                frame.BackgroundColor = (Color)Application.Current.Resources["ThirdBackgroundColor"];
            }
            else
            {
                frame.HorizontalOptions = LayoutOptions.Start;
                frame.BackgroundColor = (Color)Application.Current.Resources["SecondaryBackgroundColor"];
                label.TextColor = (Color)Application.Current.Resources["PrimaryFontColor"];
            }
            frame.Content = label;
            MessagesView.Children.Add(frame);
        }
        
	}
}