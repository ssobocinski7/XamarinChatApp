using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FriendController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> SendRequest(string SenderID, string ReceiverUsername)
        {
            var result = new { Success = false, Message = "Error during sending request.", SenderID = "", SenderUsername = "", ReceiverID = "", ReceiverUsername = "", Accepted = false};
            DataContext db = new DataContext();
            var Receiver = db.Users
                         .Where(c => c.UserName.Equals(ReceiverUsername, StringComparison.CurrentCultureIgnoreCase))
                         .FirstOrDefault();
            
            if(Receiver != null)
            {
                
                string ReceiverID = Receiver.ID.ToString();
                if (ReceiverID != SenderID)
                {
                    var Pending = db.FriendLinks
                                    .Where(c => c.SenderID == SenderID || c.ReceiverID == SenderID)
                                    .Where(c => c.SenderID == ReceiverID || c.ReceiverID == ReceiverID)
                                    .FirstOrDefault();

                    if (Pending == null)
                    {
                        var newRequest = new FriendLink
                        {
                            SenderID = SenderID,
                            ReceiverID = ReceiverID,
                            Accepted = false
                        };
                        db.FriendLinks.Add(newRequest);
                        await db.SaveChangesAsync();

                        string _senderUsername = db.Users.Where(c => SenderID == c.ID.ToString()).FirstOrDefault().UserName;
                        result = new { Success = true, Message = "Request sent successfully!", SenderID = SenderID, SenderUsername = _senderUsername, ReceiverID = ReceiverID, ReceiverUsername = ReceiverUsername, Accepted = false};
                    }
                    else
                    {
                        result = new { Success = false, Message = "Request already sent.", SenderID = "", SenderUsername = "", ReceiverID = "", ReceiverUsername = "", Accepted = false };
                    }
                }
            }
            else
            {
                result = new { Success = false, Message = "Couldn't find user.", SenderID = "", SenderUsername = "", ReceiverID = "", ReceiverUsername = "", Accepted = false };
            }
            return Json(result);
        }
        [HttpPost]
        public async Task<ActionResult> CheckIfHasPending(string SenderID)
        {
            DataContext db = new DataContext();
            var result = db.FriendLinks
                           .Where(c => c.ReceiverID == SenderID)
                           .Where(c => c.Accepted == false)
                           .ToList();


            if(result.Count > 0)
            {
                List<FriendRequest> _requests = new List<FriendRequest>();
                foreach (var link in result)
                {
                    string _senderUsername = db.Users.Where(c => link.SenderID == c.ID.ToString()).FirstOrDefault().UserName;
                    string _receiverUsername = db.Users.Where(c => link.ReceiverID == c.ID.ToString()).FirstOrDefault().UserName;
                    var _entry = new FriendRequest
                    {
                        SenderID = link.SenderID,
                        SenderUsername = _senderUsername,
                        ReceiverID = link.ReceiverID,
                        ReceiverUsername = _receiverUsername,
                        Accepted = link.Accepted
                    };
                    _requests.Add(_entry);
                }
                var respond = new
                {
                    HasPending = true,
                    List = _requests
                };
                return Json(respond);
            }
            else
            {
                var respond = new APIPendingMessage
                {
                    HasPending = false
                };
                return Json(respond);
            }
            
        }
        [HttpPost]
        public async Task<ActionResult> AcceptRequest(string SenderID, string ReceiverID)
        {
            var respond = new { Success = false };
            var db = new DataContext();
            var result = db.FriendLinks
                           .Where(c => c.SenderID == SenderID && c.ReceiverID == ReceiverID)
                           .FirstOrDefault();

            if(result != null)
            {
                result.Accepted = true;
                await db.SaveChangesAsync();
                respond = new { Success = true };
            }

            return Json(respond);
        }
        [HttpPost]
        public async Task<ActionResult> GetFriends(string UserID)
        {
            var db = new DataContext();
            var result = db.FriendLinks
                           .Where(c => c.SenderID == UserID || c.ReceiverID == UserID)
                           .Where(c => c.Accepted == true)
                           .ToList();

            List<FriendRequest> temp = new List<FriendRequest>();
            foreach (var item in result)
            {
                string senderUsername = db.Users.Where(c => item.SenderID == c.ID.ToString()).FirstOrDefault().UserName;
                string receiverUsername = db.Users.Where(c => item.ReceiverID == c.ID.ToString()).FirstOrDefault().UserName;
                var _entry = new FriendRequest
                {
                    SenderID = item.SenderID,
                    SenderUsername = senderUsername,
                    ReceiverID = item.ReceiverID,
                    ReceiverUsername = receiverUsername,
                    Accepted = item.Accepted
                };
                temp.Add(_entry);
            }

            if(temp.Count > 0)
            {
                var respond = new { Success = true, List = temp };
                return Json(respond);
            }
            else
            {
                var respond = new { Success = false };
                return Json(respond);
            }


        }
        [HttpPost]
        public async Task<ActionResult> RejectRequest(string SenderID, string ReceiverID)
        {
            var respond = new { Success = false };
            var db = new DataContext();
            var result = db.FriendLinks
                           .Where(c => c.SenderID == SenderID && c.ReceiverID == ReceiverID)
                           .FirstOrDefault();

            if (result != null)
            {
                db.Remove<FriendLink>(result);
                await db.SaveChangesAsync();
                respond = new { Success = true };
            }
            return Json(respond);
        }
    }
}