using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.WebSockets;
using XamarinApp.Views;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Data;
using XamarinApp.Services.Interfaces;
using XamarinApp.Services;
using XamarinApp.Models.SQL;

namespace XamarinApp
{
	public partial class App : Application
	{
        public static User User;

        private static LocalDatabase _localDatabase;
        public static LocalDatabase LocalDatabase
        {
            get
            {
                if(_localDatabase == null)
                {
                    var fileHelper = DependencyService.Get<IFileHelper>();
                    var fileName = fileHelper.GetLocalFilePath("AppDatabase.db3");
                    _localDatabase = new LocalDatabase(fileName);
                }
                return _localDatabase;
            }
            
        }

        private static WebsocketService _socket;
        public static WebsocketService Socket
        {
            get
            {
                if(_socket == null)
                {
                    _socket = new WebsocketService();
                }
                return _socket;
            }
        }
        public App ()
		{

			InitializeComponent();
			MainPage = new NavigationPage(new LoginPage());


		}
        
        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
            if(_socket != null)
            {
                _socket.CloseConnection();
            }
        }

		protected override async void OnResume ()
		{
            if(_socket != null)
            {
                await _socket.ConnectToServer();
            }
		}
	}
}
