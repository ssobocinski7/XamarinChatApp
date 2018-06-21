using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public UserViewModel Validate()
        {
            DataContext db = new DataContext();

            User u = db.Users
                     .Where(c => c.UserName == UserName && c.Password == Password)
                     .FirstOrDefault();

            UserViewModel uvm = new UserViewModel();
            uvm.ID = u.ID;
            uvm.UserName = u.UserName;

            return uvm;

        }
    }
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public async Task<bool> Validate()
        {
            DataContext db = new DataContext();

            User u = db.Users
                       .Where(c => c.UserName == UserName)
                       .FirstOrDefault();

            if(u == null)
            {
                var entry = new User
                {
                    UserName = UserName,
                    Password = Password
                };
                db.Users.Add(entry);
                await db.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
