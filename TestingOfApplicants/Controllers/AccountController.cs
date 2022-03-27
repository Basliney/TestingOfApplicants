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
        public async Task<IActionResult> Login(string name)
        {
            if (ModelState.IsValid)
            {
                /* 
                 * На сайт КубГУ установлено расширение, которое не позволяет нажимать на ссылку 
                 * тестирования, если пользователь не аутентифицирован.
                 * Но если набрать в поисковой строке localhost:44390/Account/Login то ссылка направит на 
                 * аутентификацию в системе тестирования. Поэтому следует проверять параметр name на пустоту
                */
                if (name != null && !name.Equals(""))
                {
                    name = name.Trim(); // Удаляем лишние пробелы

                    // Ищем пользователя в базе данных
                    User user = await _context.Users
                        .FirstOrDefaultAsync(u => u.mName == name);

                    // Если нашли, то вызываем метод аутентификации
                    if (user != null)
                    {
                        await Authenticate(user);

                        StaticData.Me = user;
                        return RedirectToAction("Index", "Home");
                    }
                    // Если не нашли, то надо регистрировать в базе данных
                    else
                    {
                        user = new User
                        {
                            mName = name
                        };

                        _context.Users.Add(user);

                        await _context.SaveChangesAsync();
                        await Authenticate(user);

                        StaticData.Me = user;
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Если ФИО нет в ссылке, то переходим на сайт КубГУ
                    return Redirect("https://www.kubsu.ru/");
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
