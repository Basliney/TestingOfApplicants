using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingOfApplicants.Models;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Controllers
{
    public class UserController : Controller
    {
        private ApplicationContext _context;
        private User _user = null;

        public UserController(ApplicationContext context)
        {
            _context = context;
        }

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

        [HttpGet]
        public async Task <IActionResult> UserInfo(int id)
        {
            _user = GetUser();

            if (_user == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (id != _user.Id && _user.Role < 2)
            {
                return RedirectToAction("Index", "Home");
            }

            if (await _context.Users.FirstOrDefaultAsync(x=>x.Id == id) == null)
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
            ViewBag.CompletedTests = _context.CompletedTestsDto.Where(x=>x.UserId==id).ToList();
            ViewBag.Subjects = _context.subjects.ToList();

            return View();
        }
    }
}
