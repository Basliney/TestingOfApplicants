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

        private User activeUser { get; set; }

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            this._context = context;
            _logger = logger;
            if (HttpContext == null)
            {
                RedirectToAction("Login", "Authorization");
            }
            else
            {
                activeUser = _context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name));
                ViewBag.AcriveUser = activeUser;
            }
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name));
                ViewBag.ActiveUser = user;

                if (user == null) { return RedirectToAction("Login", "Authorization"); }

                string myName = user.mName.ToString();
            }
            catch (NullReferenceException e)
            {
                _logger.LogError(e.StackTrace);
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
