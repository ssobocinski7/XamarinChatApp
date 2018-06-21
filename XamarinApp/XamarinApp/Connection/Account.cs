using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.Connection
{
    public class Account
    {
        public async static Task<bool> Login(string _UserName, string _Password)
        {

            var client = new RestClient("http://webapi20180614125427.azurewebsites.net");
            var request = new RestRequest("api/Account/Login", Method.POST);
            request.AddParameter("UserName", _UserName);
            request.AddParameter("Password", _Password);

            var respond = await client.ExecuteTaskAsync(request);
            User r = JsonConvert.DeserializeObject<User>(respond.Content);
            if (r != null)
            {
                App.User = r;
                return true;

            }
            return false;
        }
        public async static Task<bool> Register(string _UserName, string _Password)
        {
            var client = new RestClient("http://webapi20180614125427.azurewebsites.net");
            var request = new RestRequest("api/Account/Register", Method.POST);
            request.AddParameter("UserName", _UserName);
            request.AddParameter("Password", _Password);

            var respond = await client.ExecuteTaskAsync(request);
            var r = JsonConvert.DeserializeObject<APIRegisterMessage>(respond.Content);
            if (r.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
