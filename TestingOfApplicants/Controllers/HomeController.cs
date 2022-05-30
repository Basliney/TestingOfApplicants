using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                    var user = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType);

                    if (user == null) { return RedirectToAction("Login", "Authorization"); }

                    string myName = user.Value.ToString();

                    StaticData.Me = await _context.Users
                        .FirstOrDefaultAsync(x => x.mName.Equals(myName));
                }
                catch(NullReferenceException e)
                {
                    _logger.LogError(e.StackTrace);
                    return RedirectToAction("Login", "Authorization");
                }
            }
            List<TestHeader> testHeaders = await _context.TestHeaders.ToListAsync();
            StaticData.completedTestsDto = await _context.CompletedTestsDto.ToListAsync();

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
