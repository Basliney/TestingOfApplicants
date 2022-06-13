using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingOfApplicants.Models;
using TestingOfApplicants.Models.Statistics;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Controllers
{
    public class StatisticsController : Controller
    {
        private ApplicationContext _context;
        public StatisticsController(ApplicationContext context)
        {
            this._context = context;
        }

        public ActionResult AllStatistics()
        {
            User user = null;
            try
            {
                user = _context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name));
            }
            catch
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (user == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (_context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name)).Role != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            List<User> userList = _context.Users.ToList();
            List<CompletedTestDto> completedTests = _context.CompletedTestsDto.ToList();
            List<TestHeader> headers = _context.TestHeaders.ToList();
            List<SubjectDto> subjects = _context.subjects.ToList();

            Dictionary<TestHeader, int> dict = new Dictionary<TestHeader, int>();

            foreach (var item in headers)
            {
                dict.Add(item, completedTests.Where(x => x.TestId == item.Id).Count());
            }

            var sortedDict = from d in dict
                             orderby d.Value descending
                             select d;

            AllStatistics statistics = new AllStatistics(userList, completedTests, headers);
            ViewBag.ActiveUser = user;
            ViewBag.Subjects = subjects;
            ViewBag.Questions = _context.Questions.ToList();
            ViewBag.Headers = sortedDict;

            return View(statistics);
        }

        public IActionResult DisplaySearchResults(string FIOfinder)
        {
            User user = null;
            try
            {
                user = _context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name));
            }
            catch
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (user == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (user.Role < 2 || string.IsNullOrEmpty(FIOfinder))
            {
                return RedirectToAction("AllStatistics", "Statistics");
            }

            ViewBag.ActiveUser = user;
            List<User> userList = _context.Users.Where(x => x.mName.ToLower().Contains(FIOfinder.ToLower())).ToList();

            return View(userList);

        }

        [HttpGet]
        public async Task<IActionResult> TestDetails(int id)
        {
            User user = null;
            try
            {
                user = _context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name));
            }
            catch
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (_context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name)) == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (_context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name)).Role < 1)
            {
                return RedirectToAction("AllStatistics", "Statistics");
            }

            //try
            //{
                List<User> users = new List<User>();

                List<CompletedTestDto> tests = _context.CompletedTestsDto.Where(x => x.TestId == id).ToList();

                foreach (var item in tests)
                {
                    users.Add(await _context.Users.FirstOrDefaultAsync(x => x.Id == item.UserId));
                }

                var test = await _context.TestHeaders.FirstOrDefaultAsync(x => x.Id == id);
                var subject = await _context.subjects.FirstOrDefaultAsync(x => x.id == test.Subjectid);
                List<Question> questions = _context.Questions.Where(x => x.HeaderId == id).ToList();

                ViewBag.ActiveUser = user;
                ViewBag.Questions = questions;
                ViewBag.Test = test;
                ViewBag.Users = users;
                ViewBag.CompletedTests = tests;
                ViewBag.Subject = subject;
                return View();
            //}
            //catch
            //{
            //    return RedirectToAction("AllStatistics", "Statistics");
            //}
        }
    }
}
