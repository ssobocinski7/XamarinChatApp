using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Testing.ViewModels;
using Xamarin.Forms;
using XamarinApp.Connection;
using XamarinApp.Models;
using XamarinApp.Models.SQL;
using XamarinApp.Services;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string UserName { get;  set; }
        public string Password { get;  set; }
        public Command LoginCommand { get; set; }
        public Command RegisterCommand { get; set; }
        public LoginViewModel()
        {
            UserName = "";
            Password = "";
            LoginCommand = new Command(async () => await TryLogin(UserName, Password));
            RegisterCommand = new Command(async () => await NavigationService.NavigateTo(new RegisterPage()));
        }
        public async Task TryLogin(string _UserName, string _Password)
        {
            if(await Account.Login(_UserName, _Password))
            {
                await Application.Current.MainPage.DisplayAlert("Login", "Loged in sucessfully!", "OK");
                await NavigationService.NavigateTo(new MainPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Login", "Wrong username or password!", "OK");
                await NavigationService.NavigateTo(new LoginPage());
            }
        }
    }
}
