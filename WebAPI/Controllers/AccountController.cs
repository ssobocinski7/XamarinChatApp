using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        // GET api/values
        [HttpPost]
        public async Task<ActionResult> Login(string UserName, string Password)
        {
            var respond = new { ID = "", UserName = "" };

            LoginViewModel vm = new LoginViewModel();
            await TryUpdateModelAsync<LoginViewModel>(vm);
            UserViewModel user = vm.Validate();

            if(user != null)
            {
                respond = new
                {
                    ID = user.ID.ToString(),
                    UserName = user.UserName
                };
            }

            return Json(respond);
        }
        [HttpPost]
        public async Task<ActionResult> Register(string UserName, string Password)
        {
            var respond = new { Success = false };
            RegisterViewModel vm = new RegisterViewModel();
            await TryUpdateModelAsync<RegisterViewModel>(vm);

            if (await vm.Validate())
            {
                respond = new { Success = true };
            }
            return Json(respond);
        }
        
    }
}
