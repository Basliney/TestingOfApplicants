using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TestingOfApplicants.Models;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Controllers
{
    public class UserController : Controller
    {
        private ApplicationContext _context;

        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult UserInfo(int id)
        {
            if (id != StaticData.Me.Id && StaticData.Me.Role < 2)
            {
                return RedirectToAction("Index", "Home");
            }

            List<CompletedTestDto> completedTests = _context.CompletedTestsDto.Where(x => x.UserId == id).ToList();
            List<TestHeader> headers = new List<TestHeader>();

            foreach (var test in completedTests)
            {
                headers.Add(_context.TestHeaders.FirstOrDefault(x => x.Id == test.TestId));
            }

            ViewBag.Headers = headers;
            ViewBag.User = _context.Users.FirstOrDefault(x => x.Id == id);

            return View();
        }
    }
}
