using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestingOfApplicants.Models;

namespace TestingOfApplicants.Controllers
{
    public class AuthorizationController : Controller
    {
        private ApplicationContext _context;
        public AuthorizationController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(string name, string mail, string authorize_password, string authorize_password_again)
        {
            try
            {
                if (name.Equals("") || mail.Equals("") || authorize_password.Equals(""))
                {
                    return View();
                }
                if (_context.Users.FirstOrDefault(x=>x.Email.Equals(mail)) != null)
                {
                    return View();
                }
                User user = new User() 
                { 
                    mName = name, 
                    Email = mail, 
                    Password = authorize_password, 
                    Role = 0 
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login", "Account", new { name = user.mName });
            }
            catch
            {
                return RedirectToAction("SignIn", "Authorization");
            }
        }

        [HttpGet]
        new public async Task<IActionResult> SignOut()
        {
            StaticData.Me = null;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn", "Authorization");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string mail, string authorize_password)
        {
            try
            {
                if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(authorize_password))
                {
                    throw new Exception();
                }

                User user = await _context.Users.FirstOrDefaultAsync(
                    x => x.Email.Equals(mail) && x.Password.Equals(authorize_password)
                    );

                if (user == null)
                {
                    throw new Exception();
                }
                return RedirectToAction("Login", "Account", new { name = user.mName });
            }
            catch
            {
                return RedirectToAction("Login", "Authorization");
            }
        }
    }
}
