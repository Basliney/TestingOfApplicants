using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TestingOfApplicants.Models;

namespace TestingOfApplicants.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _context;

        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Login(string name)
        {
            if (ModelState.IsValid)
            {
                name = name.Trim();

                if (name != null && !name.Equals(""))
                {
                    User user = await _context.Users
                        .FirstOrDefaultAsync(u => u.mName == name);

                    if (user != null)
                    {
                        await Authenticate(user);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        user = new User
                        {
                            mName = name
                        };

                        _context.Users.Add(user);

                        await _context.SaveChangesAsync();
                        await Authenticate(user);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.mName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                 ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
