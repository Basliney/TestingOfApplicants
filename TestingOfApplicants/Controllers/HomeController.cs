using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using TestingOfApplicants.Models;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationContext _context { get; set; }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            this._context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            if (StaticData.Me == null)
            {
                try
                {
                    string myName = User
                            .FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value.ToString();

                    StaticData.Me = await _context.Users
                        .FirstOrDefaultAsync(x => x.mName.Equals(myName));

                    RedirectToAction("Login", "Account");
                }
                catch(NullReferenceException e)
                {
                    _logger.LogError(e.StackTrace);
                    return Redirect("https://Kubsu.ru");
                }
            }
            return View(await _context.TestHeaders.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
