using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private User _user { get; set; }

        private User GetUser()
        {
            User user = null;
            try
            {
                user = _context.Users.FirstOrDefault(x => x.Id.Equals(int.Parse(HttpContext.User.Identity.Name)));
            }
            catch
            {
                return null;
            }
            ViewBag.ActiveUser = user;
            return user;
        }

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            this._context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            _user = GetUser();
            if (_user == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            List<TestHeader> testHeaders = await _context.TestHeaders.ToListAsync();

            ViewBag.completedTestsDto = await _context.CompletedTestsDto.ToListAsync();

            return View(testHeaders);
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
