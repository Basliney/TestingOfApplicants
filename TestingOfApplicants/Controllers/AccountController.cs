using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestingOfApplicants.Models;

namespace TestingOfApplicants.Controllers
{
    /// <summary>
    /// Класс для определения пользователя
    /// </summary>
    public class AccountController : Controller
    {
        private ApplicationContext _context;    // База данных

        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ассинхронный метод для обработки входящего пользователя.
        /// </summary>
        /// <param name="name">ФИО пользователя (с сайта КубГУ)</param>
        /// <returns></returns>
        public async Task<IActionResult> Login(string name, string password)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("https://kubsu.ru");
            }
            /* 
             * На сайт КубГУ установлено расширение, которое не позволяет нажимать на ссылку 
             * тестирования, если пользователь не аутентифицирован.
             * Но если набрать в поисковой строке localhost:44390/Account/Login то ссылка направит на 
             * аутентификацию в системе тестирования. Поэтому следует проверять параметр name на пустоту
            */
            if (name == null || name.Trim().Split().Length < 2 || string.IsNullOrEmpty(password))
            {
                return Redirect("https://kubsu.ru");
            }

            name = name.Trim(); // Удаляем лишние пробелы

            // Ищем пользователя в базе данных
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.mName.Equals(name));

           

            // Если не нашли пользователя, то надо регистрировать
            if (user == null)
            {
                user = new User
                {
                    mName = name,
                    Password = password
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();
            }
            else
            {
                if (user.Password == null)
                {
                    user.Password = password;
                    await _context.SaveChangesAsync();
                }
                if (!user.Password.Equals(password))
                {
                    return Redirect("https://kubsu.ru");
                }
            }
            await Authenticate(user);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddMail([FromForm] string mail)
        {
            if (await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(mail)) == null){
                _context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name)).Email = mail;
                _context.Users.Update(_context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name)));
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserInfo", "User", new { id = _context.Users.FirstOrDefault(x=>x.Email.Equals(HttpContext.User.Identity.Name)).Id});
        }

        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                 ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
