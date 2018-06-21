using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Testing.ViewModels;
using Xamarin.Forms;
using XamarinApp.Connection;

namespace XamarinApp.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordRe { get; set; }

        public Command SendRegisterCommand { get; set; }
        public RegisterViewModel()
        {
            Username = "";
            Password = "";
            PasswordRe = "";
            SendRegisterCommand = new Command(SendRegister);
        }
        private async void SendRegister()
        {
            if(Password == PasswordRe)
            {
                if(await Account.Register(Username, Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Register", "Registered successfully!", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Register", "Something went wrong", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Register", "Passwords don't match", "OK");
            }
        }
    }
}
