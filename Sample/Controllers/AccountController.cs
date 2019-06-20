using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string type,string returnUrl)
        {
            AuthenticationProperties properties = new AuthenticationProperties();
            properties.RedirectUri = returnUrl;
            await HttpContext.ChallengeAsync(type, properties);
            return View();
        }
        public IActionResult PocketFail()
        {
            return View();
        }
        public IActionResult Pocket()
        {
            return View();
        }
    }
}