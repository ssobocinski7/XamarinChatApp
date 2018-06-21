using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Models.SQL;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using XamarinApp.Views;

namespace XamarinApp.Connection
{
    public static class Friend
    {
        public async static Task SendRequest(string ReceiverUsername)
        {
            var client = new RestClient("http://webapi20180614125427.azurewebsites.net");
            var request = new RestRequest("api/Friend/SendRequest", Method.POST);
            request.AddParameter("SenderID", App.User.ID);
            request.AddParameter("ReceiverUsername", ReceiverUsername);

            var respond = await client.ExecuteTaskAsync(request);
            var r = JsonConvert.DeserializeObject<APIRequestMessage>(respond.Content);
            //
            if(r != null)
            {
                if (r.Success)
                {
                    var socket = App.Socket;
                    var msg = new
                    {
                        Type = "newRequest",
                        SenderID = r.SenderID,
                        ReceiverID = r.ReceiverID,
                    };
                    await socket.SendMessage(JsonConvert.SerializeObject(msg));
                }

                await Application.Current.MainPage.DisplayAlert("Request", r.Message, "OK");
            }
            
        }
        public async static Task GetFriends()
        {
            var client = new RestClient("http://webapi20180614125427.azurewebsites.net");
            var request = new RestRequest("api/Friend/GetFriends", Method.POST);
            request.AddParameter("UserID", App.User.ID);

            var respond = await client.ExecuteTaskAsync(request);
            var r = JsonConvert.DeserializeObject<APIGetFriendsMessage>(respond.Content);

            var db = App.LocalDatabase;
            await db.ResetFriends();

            if (r.Success)
            {
                foreach (var item in r.List)
                {
                    if (item.Accepted)
                    {
                        if (item.SenderID == App.User.ID.ToString())
                        {
                            var entry = new Models.SQL.Friend
                            {
                                UserID = item.ReceiverID,
                                UserName = item.ReceiverUsername
                            };
                            await db.SaveItemAsync(entry);
                        }
                        else
                        {
                            var entry = new Models.SQL.Friend
                            {
                                UserID = item.SenderID,
                                UserName = item.SenderUsername
                            };
                            await db.SaveItemAsync(entry);
                        }
                      
                    }
                }
                
            }

        }
        public async static Task CheckIfHasPending()
        {
            var client = new RestClient("http://webapi20180614125427.azurewebsites.net");
            var request = new RestRequest("api/Friend/CheckIfHasPending", Method.POST);
            request.AddParameter("SenderID", App.User.ID);

            var respond = await client.ExecuteTaskAsync(request);
            var r = JsonConvert.DeserializeObject<APIPendingMessage>(respond.Content);
            if (r != null)
            {
                var db = App.LocalDatabase;
                await db.ResetRequests();

                if (r.List.Count > 0)
                {
                    foreach (var _temp in r.List)
                    {
                        var entry = new FriendRequest
                        {
                            SenderID = _temp.SenderID,
                            SenderUsername = _temp.SenderUsername,
                            ReceiverID = _temp.ReceiverID,
                            ReceiverUsername = _temp.ReceiverUsername,
                            Accepted = _temp.Accepted
                        };
                        await db.SaveItemAsync(entry);
                    }
                    await Application.Current.MainPage.DisplayAlert("Pending", "You have "+r.List.Count+" pending friend request[s]", "OK");
                }
            }

        }
        public async static Task AcceptRequest(string SenderID)
        {
            var client = new RestClient("http://webapi20180614125427.azurewebsites.net");
            var request = new RestRequest("api/Friend/AcceptRequest", Method.POST);
            request.AddParameter("SenderID", SenderID);
            request.AddParameter("ReceiverID", App.User.ID);

            var respond = await client.ExecuteTaskAsync(request);
            var r = JsonConvert.DeserializeObject<APIAcceptRequestMessage>(respond.Content);
            if (r.Success)
            {
                var socket = App.Socket;
                var msg = new
                {
                    Type = "acceptRequest",
                    SenderID = SenderID,
                    ReceiverID = App.User.ID
                };
                await socket.SendMessage(JsonConvert.SerializeObject(msg));
                var db = App.LocalDatabase;
                await db.RemoveRequest(SenderID, App.User.ID.ToString());
                await Application.Current.MainPage.DisplayAlert("Request", "Added sucessfully!", "OK");
                await NavigationService.NavigateTo(new MainPage());

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Request", "Something went wrong.", "OK");
            }

        }
        public async static Task RejectRequest(string SenderID)
        {
            var client = new RestClient("http://webapi20180614125427.azurewebsites.net");
            var request = new RestRequest("api/Friend/RejectRequest", Method.POST);
            request.AddParameter("SenderID", SenderID);
            request.AddParameter("ReceiverID", App.User.ID);

            var respond = await client.ExecuteTaskAsync(request);
            var r = JsonConvert.DeserializeObject<APIRejectRequestMessage>(respond.Content);

            if (r.Success)
            {
                var db = App.LocalDatabase;
                await db.RemoveRequest(SenderID, App.User.ID.ToString());
                await Application.Current.MainPage.DisplayAlert("Request", "Request rejected successfully", "OK");
                await NavigationService.BackToRecentPage();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Request", "Something went wrong", "OK");
            }
        }
    }
}
