using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<ActionResult> AllStatistics()
        {
            if (StaticData.Me.Role == 2)
            {
                List<User> userList = _context.Users.ToList();
                List<CompletedTestDto> completedTests = _context.CompletedTestsDto.ToList();
                List<TestHeader> headers = _context.TestHeaders.ToList();

                AllStatistics statistics = new AllStatistics(userList, completedTests, headers);

                return View(statistics);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> DisplaySearchResults(string FIOfinder)
        {
            if (StaticData.Me.Role == 2 && FIOfinder != null && !FIOfinder.Equals(""))
            {
                List<User> userList = _context.Users.Where(x=>x.mName.ToLower().Contains(FIOfinder.ToLower())).ToList();

                return View(userList);
            }
            return RedirectToAction("AllStatistics", "Statistics");
        }

        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
